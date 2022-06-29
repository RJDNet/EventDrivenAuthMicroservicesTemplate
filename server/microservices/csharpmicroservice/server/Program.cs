using CSharpMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Add Rpc Service
builder.Services.AddSingleton<IRpcServerService, RpcServerService>();
builder.Services.AddHostedService<RpcServerBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}
else 
{
    // When in Production, use HTTPS.
    //app.UseHsts();
    //app.UseHttpsRedirection();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();
