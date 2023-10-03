using Application;
using Infrastructure;
using WebApi;
using WebApi.Config.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddWebApiServices(builder.Configuration);
// GRPC CLIENTS CONFIG MONGO AND SYBASE
builder.Services.AddGrpcClientMongoServices(builder.Configuration);
builder.Services.AddGrpcClientSybaseServices(builder.Configuration);


var app = builder.Build();

app.UseWebApi(app.Environment);

app.Run();