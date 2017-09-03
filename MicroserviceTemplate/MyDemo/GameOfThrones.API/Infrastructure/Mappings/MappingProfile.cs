//using AutoMapper;

//namespace MicroserviceArchitecture.GameOfThrones.API.Infrastructure.Mappings
//{
//    public class MappingProfile : Profile
//    {
//        public MappingProfile()
//        {
//            // Character
//            CreateMap<Domain.AggregatesModel.OrderAggregate.Order, Order>(MemberList.Destination)
//                .ForMember(dest => dest.Friends, opt => opt.Ignore())
//                .ForMember(dest => dest.AppearsIn, opt => opt.Ignore()
//            );

// // Droid CreateMap<Domain.AggregatesModel.OrderAggregate.OrderItem,
// OrderItem>(MemberList.Destination).IncludeBase<Core.Models.Character, Character>();

//            // Human
//            CreateMap<Core.Models.Human, Human>(MemberList.Destination)
//                .IncludeBase<Core.Models.Character, Character>()
//                .ForMember(
//                    dest => dest.HomePlanet,
//                    opt => opt.MapFrom(src => src.HomePlanet.Name)
//                );
//        }
//    }
//}