using System.Reflection;
using Construmart.Api.Pipelines;
using Construmart.Core;
using Construmart.Core.Commons;
using Construmart.Core.Commons.ObjectMappers;
using Construmart.Core.Commons.Utils;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Construmart.Api.Installers
{
    /// <summary>
    /// Registers dependencies for the core project
    /// </summary>
    public class CoreLayer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void GetServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(RequestContext<object>).GetTypeInfo().Assembly);
            // services.AddValidatorsFromAssembly(typeof(RequestContext<object>).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(ApplicationUserMapper));

            services.AddTransient<IResult, Result>();

            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}