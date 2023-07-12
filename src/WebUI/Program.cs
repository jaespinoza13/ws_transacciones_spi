using Application;
using Infrastructure;
using WebUI;
using WebUI.Middleware;
using static AccesoDatosGrpcAse.Neg.DAL;
using static AccesoDatosGrpcMongo.Neg.DALMongo;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddWebUiServices( builder.Configuration );

var grpc = builder.Configuration.GetSection( "ApiSettings:GrpcSettings" );
var urlSybase = grpc.GetValue<string>( "client_grpc_sybase" );
var urlMongo = grpc.GetValue<string>( "client_grpc_mongo" );

builder.Services.AddGrpcClient<DALClient>( o => { o.Address = new Uri( urlSybase! ); } ).ConfigureChannel( c =>
{
    c.HttpHandler = new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        KeepAlivePingDelay = TimeSpan.FromSeconds( 20 ),
        KeepAlivePingTimeout = TimeSpan.FromSeconds( 60 ),
        EnableMultipleHttp2Connections = true
    };
} );

builder.Services.AddGrpcClient<DALMongoClient>( o => { o.Address = new Uri( urlMongo! ); } ).ConfigureChannel( c =>
{
    c.HttpHandler = new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        KeepAlivePingDelay = TimeSpan.FromSeconds( 20 ),
        KeepAlivePingTimeout = TimeSpan.FromSeconds( 60 ),
        EnableMultipleHttp2Connections = true
    };
} );

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(
    c => { c.SwaggerEndpoint( "/swagger/v1/swagger.json", "WS Transacciones SPI COOPMEGO V1" ); }
);

app.UseCors( "CorsPolicy" );

app.UseAuthotizationMego();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();