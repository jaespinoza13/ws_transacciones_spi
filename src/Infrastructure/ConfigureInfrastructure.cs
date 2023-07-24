using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Application.Persistence;
using Infrastructure.Common.Interfaces;
using Infrastructure.ExternalAPIs;
using Infrastructure.gRPC_Clients.Mongo;
using Infrastructure.gRPC_Clients.Sybase;
using Infrastructure.gRPC_Clients.Sybase.Catalogos;
using Infrastructure.gRPC_Clients.Sybase.Consultas;
using Infrastructure.gRPC_Clients.Sybase.Opis;
using Infrastructure.gRPC_Clients.Sybase.Spi1;
using Infrastructure.Services;

namespace Infrastructure;

public static class ConfigureInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        //INTERFACES DE SERVICIOS
        services.AddSingleton<ILogs, LogsService>();
        services.AddSingleton<IMongoDat, LogsMongoDat>();

        services.AddTransient<IHttpService, HttpService>();
        services.AddTransient<IWsAlfresco, WsAlfresco>();

        services.AddTransient<IWsOtp, WsOtp>();
        services.AddSingleton<IOtpDat, OtpDat>();
        services.AddTransient<ISessionControl, SessionControl.SessionControl>();
        services.AddSingleton<ISesionDat, SesionDat>();
        services.AddSingleton<IKeysDat, KeysDat>();
        services.AddSingleton<ICatalogosDat, CatalogosDat>();


        //INTERFACES DE CASOS DE USO
        services.AddSingleton<ISpi1Dat, Spi1Dat>();
        services.AddSingleton<IOpisDat, OpisDat>();
        services.AddSingleton<IConsultaDat, ConsultasDat>();

        return services;
    }
}