using Application.Common.Interfaces;
using Application.Persistence;
using Infrastructure.GrpcClients.Mongo;
using Infrastructure.GrpcClients.Sybase;
using Infrastructure.GrpcClients.Sybase.Parametros;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Infrastructure
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        //INTERFACES DE SERVICIOS
        services.AddSingleton<ILogs, LogsService>();
        services.AddSingleton<IMongoDat, LogsMongoDat>();

        services.AddTransient<ISessionControl, SessionControl.SessionControl>();
        services.AddSingleton<ISessionDat, SessionDat>();
        services.AddSingleton<IKeysDat, KeysDat>();

        services.AddSingleton<IParametroDat, ParametrosDat>();
        services.AddSingleton<IOpisDat, OpisDat>();
    }
}