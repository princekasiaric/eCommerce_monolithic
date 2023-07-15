using System;
using System.Collections.Generic;
using Construmart.Api.Pipelines;
using MediatR;
using System.IO;
using System.Reflection;
using Construmart.Api.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using static Construmart.Core.Commons.Constants;
using Microsoft.AspNetCore.Mvc;
using Construmart.Core.DTOs.Response;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Construmart.Core;

namespace Construmart.Api.Installers
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiLayer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void GetServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddCors(x =>
            {
                x.AddDefaultPolicy(x =>
                {
                    // x.WithOrigins(Cors.Origin.CONSTRUMART_FRONTEND);
                    x.AllowAnyOrigin();
                    x.AllowAnyHeader();
                    x.AllowAnyMethod();
                });
            });

            services.AddRouting(x =>
            {
                x.LowercaseUrls = true;
                x.LowercaseQueryStrings = true;
            });

            services.AddControllers(x =>
            {
                x.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status500InternalServerError));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status400BadRequest));
            })
                .AddFluentValidation(x =>
                {
                    x.DisableDataAnnotationsValidation = true;
                    x.RegisterValidatorsFromAssembly(typeof(RequestContext<object>).GetTypeInfo().Assembly);
                })
                .AddJsonOptions(x => x.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

            services.AddResponseCaching();
            services.AddHealthChecks();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Construmart.Api", Version = "v1" });

                opt.AddSecurityDefinition("JWTBearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                    Description = "Input jwt token to access authenticated endpoints",
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "JWTBearer"
                            }
                        }, new List<string>()
                    }
                });

                opt.OperationFilter<SwaggerHeaderFilter>();

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                opt.IncludeXmlComments(xmlCommentsFullPath);
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}