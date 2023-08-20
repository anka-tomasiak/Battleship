using Battleships.Application;
using Battleships.Exceptions;
using Battleships.Models;

namespace Battleships.Tests.Unit.Application;

public class ShipServiceTests
{
    [Fact]
    public void Constructor_Always_ShouldCreateShipList()
    {
        //Given
        var expectedShips = new List<Ship>
        {
            new Battleship(),
            new Destroyer(),
            new Destroyer()
        };
        
        //When
        var shipService = new ShipService(Substitute.For<IBoardService>());
        
        //Then
        shipService.Ships.ShouldBeEquivalentTo(expectedShips);
    }

    [Fact]
    public void GenerateShip_WhenNoPlaceToShipsOnBoard_ShouldThrowUnableToPlaceShipException()
    {
        //Given
        var boardService = Substitute.For<IBoardService>();
        boardService.Board.Returns(GetBoard(true, false));
        var shipService = new ShipService(boardService);
        
        //When-Then
        Should.Throw<UnableToPlaceShipException>(() => shipService.GenerateShips());
    }

    [Fact]
    public void GenerateShip_WithCorrectBoard_ShouldGenerateShips()
    {
        //Given
        var boardService = Substitute.For<IBoardService>();
        boardService.Board.Returns(GetBoard(false, false));
        var shipService = new ShipService(boardService);
        var expectedCellsWithShips = shipService.Ships.Sum(s => s.Size);
        
        //When
        shipService.GenerateShips();
        
        //Then
        boardService.Board.Cast<Cell>().Count(c=>c.ContainShip).ShouldBe(expectedCellsWithShips);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("")]
    [InlineData("A11")]
    [InlineData("T2")]
    [InlineData("something_amazing")]
    [InlineData("C0")]
    [InlineData(null)]
    public void HandleShot_WithIncorrectCoordinate_ShouldThrowInvalidCoordinateException(string coordinate)
    {
        //Given
        var shipService = new ShipService(Substitute.For<IBoardService>());
        
        //When-Then
        Should.Throw<InvalidCoordinateException>(() => shipService.HandleShot(coordinate));
    }

    [Theory]
    [InlineData("A1")]
    [InlineData("F10")]
    public void HandleShot_WithCorrectCoordinate_ShouldPlaceShot(string coordinate)
    {
        //Given
        var boardService = Substitute.For<IBoardService>();
        boardService.Board.Returns(GetBoard(true, false));
        var shipService = new ShipService(boardService);
        
        //When
        var result = shipService.HandleShot(coordinate);
        
        //Then
        result.ShouldBe(ShotResultType.Hit);
        boardService.Board.Cast<Cell>().Count(c=>c.Hit).ShouldBe(1);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void HandleShot_IfAlreadyHit_ShouldReturnAlreadyShotResultType(bool containShip)
    {
        //Given
        var boardService = Substitute.For<IBoardService>();
        boardService.Board.Returns(GetBoard(containShip, true));
        var shipService = new ShipService(boardService);
        
        //When
        var result = shipService.HandleShot("A1");
        
        //Then
        result.ShouldBe(ShotResultType.AlreadyShot);
    }

    [Theory]
    [InlineData(true, ShotResultType.Hit)]
    [InlineData(false, ShotResultType.Miss)]
    public void HandleShot_IfHitFirstTime_ShouldReturnHitShotResultType(bool containShip, ShotResultType expectedResultType)
    {
        //Given
        var boardService = Substitute.For<IBoardService>();
        boardService.Board.Returns(GetBoard(containShip, false));
        var shipService = new ShipService(boardService);
        
        //When
        var result = shipService.HandleShot("A1");
        
        //Then
        result.ShouldBe(expectedResultType);
    }

    [Fact]
    public void HandleShot_IfFinalShot_ShouldReturnFinalShotResultType()
    {
        //Given
        var boardService = Substitute.For<IBoardService>();
        boardService.Board.Returns(GetBoard(false, false));
        boardService.Board[0, 0].ContainShip = true;
        var shipService = new ShipService(boardService);
        
        //When
        var result = shipService.HandleShot("A1");
        
        //Then
        result.ShouldBe(ShotResultType.FinalShot);
    }

    private Cell[,] GetBoard(bool containShips, bool hit)
    {
        var board = new Cell[Consts.BoardSize, Consts.BoardSize];
        
        for (var i = 0; i < Consts.BoardSize; i++)
        {
            for (var j = 0; j < Consts.BoardSize; j++)
            {
                board[i, j] = new Cell(containShips, hit);
            }
        }

        return board;
    }
}