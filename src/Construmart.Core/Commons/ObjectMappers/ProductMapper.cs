using AutoMapper;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.DTOs.Response;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.ProductImage.ImageFile.UploadUrl))
                .ForMember(x => x.SecureImageUrl, opt => opt.MapFrom(x => x.ProductImage.ImageFile.SecureUploadUrl))
                .ForMember(x => x.CurrencyCode, opt => opt.MapFrom(x => x.CurrencyCode.DisplayName));
        }
    }
}
