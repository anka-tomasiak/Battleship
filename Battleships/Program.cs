using Battleships.Application;
using Battleships.UserInterface;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddScoped<IUserInterface, ConsoleUserInterface>()
    .AddScoped<IBoardService>(provider => BoardService.Create(provider.GetRequiredService<IUserInterface>()))
    .AddScoped<IShipService, ShipService>()
    .AddTransient<IGameService, GameService>()
    .BuildServiceProvider();

var gameService = serviceProvider.GetService<IGameService>();

gameService?.Play();