using AutoMapper;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Response;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class NigerianStateMapper : Profile
    {
        public NigerianStateMapper()
        {
            CreateMap<NigerianState, NigerianStateResponse>();
        }
    }
}
