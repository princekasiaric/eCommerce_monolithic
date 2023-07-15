using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Construmart.Api.Installers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConstrumartServiceInstaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="env"></param>
        public static void InstallServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            ApiLayer.GetServices(services);
            InfrastructureLayer.GetServices(services, config, env);
            CoreLayer.GetServices(services);
        }
    }
}