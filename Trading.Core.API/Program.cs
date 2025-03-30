using Implementation;
using Implementation.SocketProviders.Binance;
using Microsoft.Extensions.Options;
using Implementation.Sandbox;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTradingCoreApiServices();
builder.Services.AddTradingCoreApiDIExtensionsSandbox();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<PriceHub>("/priceHub");
var binanceClient = app.Services.GetRequiredService<BinanceWebSocketClient>();
await binanceClient.ConnectAsync();

app.Run();
