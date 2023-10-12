using static AccesoDatosGrpcMongo.Neg.DALMongo;

namespace WebApi.Config.Grpc;

public static class GrpcClientMongoConfig
{
    public static void AddGrpcClientMongoServices(this IServiceCollection services, IConfiguration configuration)
    {
        var grpc = configuration.GetSection("ApiConfig:GrpcConfig");
        var urlMongo = grpc.GetValue<string>("client_grpc_mongo");
        var timeoutGrpcMongo = grpc.GetValue<int>("timeoutGrpcMongo");
        var delayOutGrpcMongo = grpc.GetValue<int>("delayOutGrpcMongo");
        services.AddGrpcClient<DALMongoClient>(o => { o.Address = new Uri(urlMongo!); }).ConfigureChannel(c =>
        {
            c.HttpHandler = new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                KeepAlivePingDelay = TimeSpan.FromSeconds(delayOutGrpcMongo),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(timeoutGrpcMongo),
                EnableMultipleHttp2Connections = true
            };
        });
    }
}