using Battleships.Application;
using Battleships.UserInterface;

var userInterface = new ConsoleUserInterface();
var boardService = new BoardService(userInterface);
var shipService = new ShipService(boardService.Board);
var gameService = new GameService(userInterface, boardService, shipService);

gameService.Play();