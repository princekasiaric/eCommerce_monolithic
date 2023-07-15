using AutoMapper;
using Construmart.Core.Domain.Models.OrderAggregate;
using Construmart.Core.DTOs.Response;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<Order, OrderResponse>()
                .ForMember(x => x.OrderStatus, opt => opt.MapFrom(x => x.OrderStatus.DisplayName))
                .ForMember(x => x.OrderItems, opt => opt.MapFrom(x => x.OrderItems));
        }
    }
}
