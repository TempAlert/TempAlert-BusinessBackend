﻿using API.Helpers.Errors;
using AspNetCoreRateLimit;
using Core.Interfaces;
using Infraestructure.Repositories;
using Infraestructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                        builder.AllowAnyOrigin()   //WithOrigins("https://dominio.com")
                        .AllowAnyMethod()          //WithMethods("GET","POST")
                        .AllowAnyHeader());        //WithHeaders("accept","content-type")
                });
    public static void AddAplicacionServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureRateLimitiong(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();

        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
            options.RealIpHeader = "X-Real-IP";
            options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint ="*",
                        Period = "10s",
                        Limit = 20 
                    }
                };
        });
    }

    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;

            //options.ApiVersionReader = new QueryStringApiVersionReader("ver");
            //options.ApiVersionReader = new HeaderApiVersionReader("X-Version");

            options.ApiVersionReader = ApiVersionReader.Combine(
                //Mediante un Query Param se puede elegir que version de la api utilizar
                new QueryStringApiVersionReader("ver"),
                //Mediante el encabezado se puede elegir que version de la api utilizar
                new HeaderApiVersionReader("X-Version"));
            //Observar en los encabezados la version de la apis que soporta
            options.ReportApiVersions = true;
        });
    }


    public static void AddValidationErrors(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {

                var errors = actionContext.ModelState.Where(u => u.Value.Errors.Count > 0)
                                                .SelectMany(u => u.Value.Errors)
                                                .Select(u => u.ErrorMessage).ToArray();

                var errorResponse = new ApiValidation()
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });
    }
}
