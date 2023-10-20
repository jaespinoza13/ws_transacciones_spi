using Application.Common.Interfaces;
using Application.Persistence;
using Infrastructure.GrpcClients.Mongo;
using Infrastructure.GrpcClients.Sybase;
using Infrastructure.GrpcClients.Sybase.Opis;
using Infrastructure.GrpcClients.Sybase.SpiArchivo;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Infrastructure
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        //INTERFACES DE SERVICIOS
        services.AddSingleton<ILogs, LogsService>();
        services.AddSingleton<IMongoDat, LogsMongoDat>();

        services.AddTransient<ISessionControl, SessionControl.SessionControl>();
        services.AddTransient<IHttpService, HttpService>();
        
        services.AddSingleton<ISessionDat, SessionDat>();
        services.AddSingleton<IKeysDat, KeysDat>();
        services.AddSingleton<IOpisDat, OpisDat>();
        services.AddSingleton<ISpiArchivoDat, SpiArchivoDat>();
    }
}