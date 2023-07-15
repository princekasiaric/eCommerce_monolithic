using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Construmart.Core.Commons;
using Construmart.Core.Commons.Utils;
using Construmart.Core.Configurations;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models;
using Construmart.Core.ProcessorContracts.FileStorage;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Notification;
using Construmart.Core.ProcessorContracts.Paystack;
using Construmart.Infrastructure.Data.EfCore;
using Construmart.Infrastructure.Data.EfCore.Repositories;
using Construmart.Infrastructure.Processors;
using Construmart.Infrastructure.Processors.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Construmart.Api.Installers
{
    /// <summary>
    /// 
    /// </summary>
    public class InfrastructureLayer
    {
        private static AuthConfig _authConfigOptions;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="env"></param>
        public static void GetServices(IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            //appsettings
            RegisterAppsettingsMappings(services, config);

            //Database
            RegisterDatabaseService(services, config, env);

            //Repositories
            RegisterRepositories(services);

            //DotNet Identity
            RegisterIdentity(services);

            //Processor services
            RegisterProcessorServices(services);

            //HttpClientFactory
            RegisterHttpClientFactory(services);
        }

        private static void RegisterAppsettingsMappings(IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailConfig>(config.GetSection(nameof(EmailConfig)));
            services.Configure<AuthConfig>(config.GetSection(nameof(AuthConfig)));
            services.Configure<PayStackConfig>(config.GetSection(nameof(PayStackConfig)));
            _authConfigOptions = config.GetSection(nameof(AuthConfig)).Get<AuthConfig>();
        }

        private static void RegisterDatabaseService(IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                var consoleLogger = LoggerFactory.Create(builder =>
                {
                    builder.AddFilter((category, LogLevel) => category == DbLoggerCategory.Database.Command.Name && LogLevel == LogLevel.Information).AddConsole();
                });

                services.AddDbContext<RepositoryContext>(builder => builder.UseLoggerFactory(consoleLogger).UseSqlServer(Env.ConstrumartDb ?? config.GetConnectionString("CONSTRUMART_DB")));
            }
            else
            {
                services.AddDbContext<RepositoryContext>(builder => builder.UseSqlServer(Env.ConstrumartDb ?? config.GetConnectionString("CONSTRUMART_DB")));
            }

            //run automated migration
#if DEBUG
            Console.WriteLine("********* DEBUG ********");
#else
            Console.WriteLine("********* RELEASE ********");
            Console.WriteLine("Initializing database migration...");
            services.BuildServiceProvider().GetRequiredService<RepositoryContext>().Database?.Migrate();
            Console.WriteLine("Database migration complete...");
#endif

            // services.AddDistributedSqlServerCache(x =>
            // {
            //     x.ConnectionString = config.GetConnectionString("CacheConnection");
            //     x.SchemaName = _cacheConfig.CacheSchema;
            //     x.TableName = _cacheConfig.CacheTable;
            //     x.DefaultSlidingExpiration = TimeSpan.FromSeconds(_cacheConfig.DefaultSlidingExpirationInSeconds);
            // });
            services.AddDistributedMemoryCache();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            // services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        private static void RegisterIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = true;
                opt.User.RequireUniqueEmail = false;
                opt.Password.RequiredLength = 8;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<RepositoryContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Env.JwtValidIssuer ?? _authConfigOptions.JwtValidIssuer,
                        ValidAudience = Env.JwtValidAudience ?? _authConfigOptions.JwtValidAudience,
                        RequireExpirationTime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.JwtSecret ?? _authConfigOptions.JwtSecret)),
                    };
                });
        }

        private static void RegisterProcessorServices(IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSingleton<IApplicationUtility, ApplicationUtility>();
            services.AddTransient<IEncryptionUtility, EncryptionUtility>();
            services.AddScoped<IFileStorageService, CloudinaryService>();
            services.AddScoped<IVerifyTransactionService, PaystackService>();
        }

        private static void RegisterHttpClientFactory(IServiceCollection services)
        {
            services.AddHttpClient("PayStack", x =>
            {
                x.Timeout = new TimeSpan(0, 0, 30);
                x.DefaultRequestHeaders.Clear();
                x.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            });
        }
    }
}