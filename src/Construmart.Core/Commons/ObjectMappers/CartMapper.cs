using AutoMapper;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Response;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class CartMapper : Profile
    {
        public CartMapper()
        {
            CreateMap<Cart, CartResponse>();
        }
    }
}
