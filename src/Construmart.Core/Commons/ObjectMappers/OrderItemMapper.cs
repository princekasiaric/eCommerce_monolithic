using AutoMapper;
using Construmart.Core.Domain.Models.OrderAggregate;
using Construmart.Core.DTOs.Response;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class OrderItemMapper : Profile
    {
        public OrderItemMapper()
        {
            CreateMap<OrderItem, OrderItemResponse>();
        }
    }
}
