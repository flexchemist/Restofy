﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
#pragma warning disable 1591

namespace WebApp
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var apiDescription in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    apiDescription.GroupName,
                    new OpenApiInfo()
                    {
                        Title = $"Sample Api{apiDescription.ApiVersion}",
                        Version = apiDescription.ApiVersion.ToString()
                    }
                );
            }

            // include xml comments (enable creation in csproj file)
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            
            // use FullName for schemaId - avoids conflicts between classes using the same name (which are in different namespaces)
            options.CustomSchemaIds(i => i.FullName);

            //Swagger authentication support
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description =
                    "JWT Authorization header using the Bearer scheme.\r\n<br>" +
                    "Enter 'Bearer'[space] and then your token in the text box below.\r\n<br>" +
                    "Example: <b>Bearer eyJhbGciOiJIUzUxMiIsIn...</b>\r\n<br>" +
                    "You will get the bearer from the <i>account/login</i> or <i>account/register</i> endpoint.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }
    }
}