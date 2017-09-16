using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly DbConnection _dbConnection;
        private readonly IntegrationEventLogContext _integrationEventLogContext;

        public IntegrationEventLogService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException("dbConnection");
            _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseSqlServer(_dbConnection)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    .Options);
        }

        public Task MarkEventAsPublishedAsync(IntegrationEvent @event)
        {
            var eventLogEntry = _integrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == @event.Id);
            eventLogEntry.TimesSent++;
            eventLogEntry.State = EventStateEnum.Published;

            _integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }

        public Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction", $"A {typeof(DbTransaction).FullName} is required as a pre-requisite to save the event.");
            }

            var eventLogEntry = new IntegrationEventLogEntry(@event);

            _integrationEventLogContext.Database.UseTransaction(transaction);
            _integrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }
    }
}