using AutoMapper;
using GameStore.Core.Dto.Responses;
using GameStore.Core.Payment.Options.PaymentGateway;
using GameStore.Models.Entities;

namespace GameStore.Core.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Game, GameResponse>()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(x => x.GamePrice.Price))
                .ForMember(
                    dest => dest.Discontinued,
                    opt => opt.MapFrom(x => x.GamePrice.Discount));
            CreateMap<Genre, GenreResponse>();
            CreateMap<Genre, SubGenreResponse>();
            CreateMap<Platform, PlatformResponse>();
            CreateMap<Publisher, PublisherResponse>();
            CreateMap<Order, OrderResponse>();
            CreateMap<OrderProduct, OrderProductResponse>();
            CreateMap<PaymentMethodInfo, PaymentMethodResponse>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<Role, RoleResponse>();
            CreateMap<User, UserResponse>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(x => x.Username));
        }
    }
}