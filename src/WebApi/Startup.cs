using System.Reflection;
using Application.Common.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Config;
using WebApi.Filters;
using WebApi.Middleware;

namespace WebApi;

public static class Startup
{
    public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRouting( r => r.LowercaseUrls = true );
        services.AddControllers();
        services.AddApiVersioning();

        // CUSTOMISE API EXCEPTIONS BEHAVIOUR
        services.AddControllersWithViews( options => options.Filters.Add<ApiExceptionFilterAttribute>() );
        services.AddValidatorsFromAssembly( Assembly.GetExecutingAssembly() );

        // CUSTOMISE DEFAULT API BEHAVIOUR
        services.Configure<ApiBehaviorOptions>( options => options.SuppressModelStateInvalidFilter = true );

        //CORS
        services.AddCors( options =>
        {
            options.AddPolicy( "CorsPolicy", policyBuilder => policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        } );

        //AUTHORIZATION 
        services.AddAuthorizationConfigApi( configuration );

        //SERVICES
        services.AddDataProtection();
        services.AddMemoryCache();
        services.AddOptions();

        //FILTERS
        services.AddTransient<CryptographyAesFilter>();
        services.AddTransient<ClaimControlFilter>();
        services.AddTransient<SessionControlFilter>();


        //SWAGGER
        services.ConfigureSwaggerApi();

        //CONFIGURATIONS
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:DataBase" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:GrpcConfig" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:MongoConfig" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:EndpointsAuth" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:LogsPath" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:TemplatesPath" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:LoadParameters" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:HttpConfig" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:ControlExcepciones" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:LogosPath" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:EmailConfig" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:Endpoints" ) );
        services.Configure<ApiConfig>( configuration.GetSection( "ApiConfig:PathPlantillas" ) );
    }

    public static void UseWebApi(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI( c => c.SwaggerEndpoint( "/swagger/v1/swagger.json", "WebApi v1" ) );
        }

        app.UseCors( "CorsPolicy" );
        app.UseAuthorizationMego();
        //app.UseJwtTokenMiddleware();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints( endpoints => { endpoints.MapControllers(); } );
    }
}