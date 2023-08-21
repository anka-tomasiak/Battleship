using Battleships.Application;
using Battleships.Models;
using Battleships.UserInterface;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddScoped<IUserInterface, ConsoleUserInterface>()
    .AddScoped<IBoardService>(provider => BoardService.Create(provider.GetRequiredService<IUserInterface>()))
    .AddScoped<IShipService, ShipService>()
    .AddTransient<IGameService, GameService>()
    .BuildServiceProvider();

try
{
    var gameService = serviceProvider.GetRequiredService<IGameService>();
    gameService.Play();
}
catch (InvalidOperationException)
{
    var userInterface = serviceProvider.GetRequiredService<IUserInterface>();
    userInterface.WriteLine(Consts.ReallyBadExceptionMessage);
}