using Battleships.Application;
using Battleships.Exceptions;
using Battleships.Models;
using Battleships.UserInterface;
using NSubstitute.ExceptionExtensions;
using TddXt.AnyRoot.Strings;

namespace Battleships.Tests.Unit.Application;

public class GameServiceTests
{
    [Fact]
    public void Play_WhenUserTypeExit_ShouldExit()
    {
        //Given
        var userInterface = Substitute.For<IUserInterface>();
        userInterface.Read().Returns(Consts.ExitCommand);
        var boardService = Substitute.For<IBoardService>();
        var shipService = Substitute.For<IShipService>();
        var gameService = new GameService(userInterface, boardService, shipService);
        
        //When
        gameService.Play();
        
        //Then
        userInterface.Received(1).Read();
        shipService.DidNotReceiveWithAnyArgs().HandleShot(Arg.Any<string>());
        boardService.DidNotReceiveWithAnyArgs().PrintBoard();
    }

    [Fact]
    public void Play_WhenHandleAlreadyShotCell_ShouldHandleShotAndNotPrintBoard()
    {
        //Given
        var input = Any.String().ToUpper();
        var userInterface = Substitute.For<IUserInterface>();
        userInterface.Read().Returns(input, Consts.ExitCommand);
        var boardService = Substitute.For<IBoardService>();
        var shipService = Substitute.For<IShipService>();
        shipService.HandleShot(input).Returns(ShotResultType.AlreadyShot);
        var gameService = new GameService(userInterface, boardService, shipService);
        
        //When
        gameService.Play();
        
        //Then
        userInterface.Received(2).Read();
        shipService.Received(1).HandleShot(input);
        boardService.DidNotReceiveWithAnyArgs().PrintBoard();
    }

    [Theory]
    [InlineData(ShotResultType.Hit)]
    [InlineData(ShotResultType.Miss)]
    public void Play_WhenHandleCellShotFirstTime_ShouldHandleShotAndPrintBoard(ShotResultType shotResultType)
    {
        //Given
        var input = Any.String().ToUpper();
        var userInterface = Substitute.For<IUserInterface>();
        userInterface.Read().Returns(input, Consts.ExitCommand);
        var boardService = Substitute.For<IBoardService>();
        var shipService = Substitute.For<IShipService>();
        shipService.HandleShot(input).Returns(shotResultType);
        var gameService = new GameService(userInterface, boardService, shipService);
        
        //When
        gameService.Play();
        
        //Then
        userInterface.Received(2).Read();
        shipService.Received(1).HandleShot(input);
        boardService.Received(1).PrintBoard();
    }

    [Fact]
    public void Play_WhenIncorrectCoordinateExceptionAppear_ShouldWriteMessageToUser()
    {
        //Given
        var input = Any.String().ToUpper();
        var userInterface = Substitute.For<IUserInterface>();
        userInterface.Read().Returns(input, Consts.ExitCommand);
        var boardService = Substitute.For<IBoardService>();
        var shipService = Substitute.For<IShipService>();
        shipService.HandleShot(input).Throws(new InvalidCoordinateException());
        var gameService = new GameService(userInterface, boardService, shipService);
        
        //When
        gameService.Play();
        
        //Then
        userInterface.Received(2).Read();
        boardService.DidNotReceiveWithAnyArgs().PrintBoard();
        userInterface.Received(1).WriteLine("Invalid coordinate. Please enter coordinate like A1 (letter is row, digit is column). Max row: J, max column: 10");
    }
}