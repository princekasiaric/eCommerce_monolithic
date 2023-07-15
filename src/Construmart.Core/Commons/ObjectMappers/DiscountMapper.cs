using AutoMapper;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Response;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class DiscountMapper : Profile
    {
        public DiscountMapper()
        {
            CreateMap<Discount, DiscountResponse>();
        }
    }
}
