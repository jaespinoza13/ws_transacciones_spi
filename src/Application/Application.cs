using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Behaviours;
using Application.Common.ISO20022.Models;
using Application.Mappings;
using AutoMapper;

namespace Application;

public static class Application
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssemblyContaining<Header>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application).Assembly));
        
        var mapperConfig = new MapperConfiguration(mc => {
            mc.AddProfile(new MappingProfile());
        });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}