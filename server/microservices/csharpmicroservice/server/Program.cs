using CSharpMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IRpcServerService, RpcServerService>();
builder.Services.AddHostedService<RpcServerBackgroundService>();

var app = builder.Build();

app.MapControllers();

app.Run();
