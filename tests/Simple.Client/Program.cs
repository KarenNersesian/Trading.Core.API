using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;


var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:32777/priceHub")
    .Build();

connection.On<string>("ReceivePriceUpdate", (price) =>
{
    Console.WriteLine($"Received price update: {price}");
});


connection.Closed += async (error) =>
{
    Console.WriteLine($"Connection closed. Trying to reconnect...");
    await Task.Delay(2000);
    await connection.StartAsync();
};


try
{
    Console.WriteLine("Connecting to SignalR...");
    await connection.StartAsync();
    Console.WriteLine("Connected to SignalR!");

   
    string instrument = "BTCUSDT"; 
    await connection.InvokeAsync("SubscribeToInstrument", instrument);
    Console.WriteLine($"Subscribed to {instrument}");

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to SignalR: {ex.Message}");
}