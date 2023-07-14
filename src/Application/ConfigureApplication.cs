using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using FluentValidation;
using Application.Common.Behaviours;
using Application.Common.ISO20022.Models;

namespace Application;

public static class ConfigureApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //SERVICIOS
        services.AddValidatorsFromAssembly( Assembly.GetExecutingAssembly() );
        services.AddValidatorsFromAssemblyContaining<Header>();
        services.AddMediatR( cfg => cfg.RegisterServicesFromAssembly( typeof( ConfigureApplication ).Assembly ) );

        services.AddTransient( typeof( IPipelineBehavior<,> ), typeof( UnhandledExceptionBehavior<,> ) );
        services.AddTransient( typeof( IPipelineBehavior<,> ), typeof( ValidationBehaviour<,> ) );

        return services;
    }
}