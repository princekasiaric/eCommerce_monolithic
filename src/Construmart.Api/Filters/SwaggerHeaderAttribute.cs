using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Construmart.Api.Filters
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class SwaggerHeaderAttribute : Attribute
    {
        // See the attribute guidelines at
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string HeaderName { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Description { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string DefaultValue { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool IsRequired { get; }

        // This is a positional argument
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="description"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isRequired"></param>
        public SwaggerHeaderAttribute(string headerName, string description = null, string defaultValue = null, bool isRequired = false)
        {
            HeaderName = headerName;
            Description = description;
            DefaultValue = defaultValue;
            IsRequired = isRequired;
        }

        // This is a named argument
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int NamedInt { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            if (context.MethodInfo.GetCustomAttribute(typeof(SwaggerHeaderAttribute)) is SwaggerHeaderAttribute attribute)
            {
                var existingParam = operation
                    .Parameters
                    .FirstOrDefault(p => p.In == ParameterLocation.Header && p.Name == attribute.HeaderName);
                if (existingParam != null)
                {
                    operation.Parameters.Remove(existingParam);
                }
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = attribute.HeaderName,
                    Description = attribute.Description,
                    Required = attribute.IsRequired,
                    In = ParameterLocation.Header,
                    Schema = string.IsNullOrEmpty(attribute.DefaultValue)
                        ? null
                        : new OpenApiSchema
                        {
                            Type = nameof(String),
                            Default = new OpenApiString(attribute.DefaultValue),
                        }
                });
            }
        }
    }
}