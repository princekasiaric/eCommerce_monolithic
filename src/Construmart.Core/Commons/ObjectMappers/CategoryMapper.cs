using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.Domain.ValueObjects;
using Construmart.Core.DTOs.Response;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CategoryResponse>();
            // .ForPath(x => x.Audit.CreatedByUserId, opt => opt.MapFrom(x => x.CreatedByUserId))
            // .ForPath(x => x.Audit.DateCreated, opt => opt.MapFrom(x => x.DateCreated))
            // .ForPath(x => x.Audit.DateUpdated, opt => opt.MapFrom(x => x.DateUpdated))
            // .ForPath(x => x.Audit.UpdatedByUserId, opt => opt.MapFrom(x => x.UpdatedByUserId));

            // CreateMap<Category, AuditResponse>();
        }
    }
}