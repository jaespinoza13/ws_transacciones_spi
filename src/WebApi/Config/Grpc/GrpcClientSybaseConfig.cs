using static AccesoDatosGrpcAse.Neg.DAL;

namespace WebApi.Config.Grpc;

public static class GrpcClientSybaseConfig
{
    public static void AddGrpcClientSybaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var grpc = configuration.GetSection("ApiConfig:GrpcConfig");
        var urlSybase = grpc.GetValue<string>("client_grpc_sybase");
        var timeoutGrpcSybase = grpc.GetValue<int>("timeoutGrpcSybase");
        var delayOutGrpcSybase = grpc.GetValue<int>("delayOutGrpcSybase");
        services.AddGrpcClient<DALClient>(o => { o.Address = new Uri(urlSybase!); }).ConfigureChannel(c =>
        {
            c.HttpHandler = new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                KeepAlivePingDelay = TimeSpan.FromSeconds(delayOutGrpcSybase),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(timeoutGrpcSybase),
                EnableMultipleHttp2Connections = true
            };
        });
    }
}