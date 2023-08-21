using Battleships.Application;
using Battleships.Models;
using Battleships.UserInterface;

namespace Battleships.Tests.Unit.Application;

public class BoardServiceTests
{
    [Fact]
    public void Create_Always_ShouldInitializeEmptyBoard()
    {
        //Given
        const int expectedSize = 10;
        
        //When
        var boardService = BoardService.Create(Substitute.For<IUserInterface>());
        
        //Then
        boardService.Board.Cast<Cell>().ToArray().All(c => c.Symbol == Consts.DefaultCellSymbol).ShouldBeTrue();
        boardService.Board.GetLength(0).ShouldBe(expectedSize);
        boardService.Board.GetLength(1).ShouldBe(expectedSize);
    }
    
    [Fact]
    public void Initialize_Always_ShouldInitializeBoardWithCorrectSizeAndSymbol()
    {
        //Given
        var boardService = BoardService.Create(Substitute.For<IUserInterface>());
        boardService.Board[1, 1] = new Cell(true, true);
        
        //When
        boardService.InitializeBoard();
        
        //Then
        boardService.Board.Cast<Cell>().ToArray().All(c => c.Symbol == Consts.DefaultCellSymbol).ShouldBeTrue();
    }
    
    [Fact]
    public void PrintBoard_WithInitializedBoard_ShouldCorrectCallWriteMethod()
    {
        //Given
        var userInterface = Substitute.For<IUserInterface>();
        var boardService = BoardService.Create(userInterface);

        //When
        boardService.PrintBoard();
        
        //Then
        userInterface.Received(1).WriteLine("    1 2 3 4 5 6 7 8 9 10");
        userInterface.Received(1).WriteLine("   ---------------------");
        for (var label = 'A'; label <= 'J'; label++)
        {
            userInterface.Received(1).Write($"{label} |");
        }
        userInterface.Received(100).Write(" .");
        userInterface.Received(10).WriteLine("");
    }
}