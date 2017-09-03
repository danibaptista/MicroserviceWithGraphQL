using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.API
{
    using GameOfThrones.Infrastructure;
    using Infrastructure;
    using Infrastructure.AutofacModules;
    using Infrastructure.Filters;
    using Infrastructure.Services;

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"settings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets(typeof(Startup).GetTypeInfo().Assembly);
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors("CorsPolicy");

            // ConfigureAuth(app);
            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                   // c.ConfigureOAuth2("orderingswaggerapi", "aspnet-Ordering.API-20161122013547",
                   // "", "Ordering Swagger UI");
               });

            WaitForSqlAvailabilityAsync(loggerFactory, app, env).Wait();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddControllersAsServices();  //Injecting Controllers themselves thru DI
                                            //For further info see: http://docs.autofac.org/en/latest/integration/aspnetcore.html#controllers-as-services

            services.AddHealthChecks(checks =>
            {
                var minutes = 1;
                if (int.TryParse(Configuration["HealthCheck:Timeout"], out var minutesParsed))
                {
                    minutes = minutesParsed;
                }
                checks.AddSqlCheck("OrderingDb", Configuration["ConnectionString"], TimeSpan.FromMinutes(minutes));
            });

            services.Configure<OrderingSettings>(Configuration);

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Ordering HTTP API",
                    Version = "v1",
                    Description = "The Ordering Service HTTP API",
                    TermsOfService = "Terms Of Service"
                });

                //options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Type = "oauth2",
                //    Flow = "implicit",
                //    AuthorizationUrl = $"{Configuration.GetValue<string>("IdentityUrlExternal")}/authorize",
                //    TokenUrl = $"{Configuration.GetValue<string>("IdentityUrlExternal")}/token",
                //    Scopes = new Dictionary<string, string>()
                //    {
                //        { "orderingapi", "Ordering API" }
                //    }
                //});

                // options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Add application services.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddEntityFrameworkSqlServer()
                  .AddDbContext<OrderingContext>(options =>
                  {
                      options.UseSqlServer(Configuration["ConnectionString"],
                          sqlServerOptionsAction: sqlOptions =>
                          {
                              sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                              sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                          });
                  },
                      ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                  );

            services.AddOptions();

            //configure autofac

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new BusinessQueryModule());
            container.RegisterModule(new BusinessCommandModule());
            container.RegisterModule(new EventSourcingModule(services, Configuration));
            container.RegisterModule(new InfrastructureModule());

            return new AutofacServiceProvider(container.Build());
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            var identityUrl = Configuration.GetValue<string>("IdentityUrlExternal");
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = identityUrl.ToString(),
                ApiName = "orderingapi",
                RequireHttpsMetadata = false,
            });
        }

        private Policy CreatePolicy(int retries, ILogger logger, string prefix)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                    }
                );
        }

        private async Task WaitForSqlAvailabilityAsync(ILoggerFactory loggerFactory, IApplicationBuilder app, IHostingEnvironment env, int retries = 0)
        {
            var logger = loggerFactory.CreateLogger(nameof(Startup));
            var policy = CreatePolicy(retries, logger, nameof(WaitForSqlAvailabilityAsync));
            await policy.ExecuteAsync(async () =>
            {
                await OrderingContextSeed.SeedAsync(app, env, loggerFactory);
            });
        }
    }
}