using AutoMapper;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.ValueObjects;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.CustomerUseCases;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, CustomerAccountInfoResponse>()
                .ForMember(x => x.Gender, opt => opt.MapFrom(x => x.Gender.DisplayName));
        }
    }
}