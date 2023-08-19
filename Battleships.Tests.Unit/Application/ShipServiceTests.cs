using Battleships.Application;
using Battleships.Exceptions;
using Battleships.Models;
using TddXt.AnyRoot.Numbers;

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
        var shipService = new ShipService(new Cell[Any.Integer(),Any.Integer()]);
        
        //Then
        shipService.Ships.ShouldBeEquivalentTo(expectedShips);
    }

    [Fact]
    public void GenerateShip_WhenNoPlaceToShipsOnBoard_ShouldThrowUnableToPlaceShipException()
    {
        //Given
        var board = GetBoard(true, false);
        var shipService = new ShipService(board);
        
        //When-Then
        Should.Throw<UnableToPlaceShipException>(() => shipService.GenerateShips());
    }

    [Fact]
    public void GenerateShip_WithCorrectBoard_ShouldGenerateShips()
    {
        //Given
        var board = GetBoard(false, false);
        var shipService = new ShipService(board);
        var expectedCellsWithShips = shipService.Ships.Sum(s => s.Size);
        
        //When
        shipService.GenerateShips();
        
        //Then
        board.Cast<Cell>().Count(c=>c.ContainShip).ShouldBe(expectedCellsWithShips);
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
        var shipService = new ShipService(new Cell[Any.Integer(),Any.Integer()]);
        
        //When-Then
        Should.Throw<InvalidCoordinateException>(() => shipService.HandleShot(coordinate));
    }

    [Theory]
    [InlineData("A1")]
    [InlineData("F10")]
    public void HandleShot_WithCorrectCoordinate_ShouldPlaceShot(string coordinate)
    {
        //Given
        var board = GetBoard(true, false);
        var shipService = new ShipService(board);
        
        //When
        var result = shipService.HandleShot(coordinate);
        
        //Then
        result.ShouldBe(ShotResultType.Hit);
        board.Cast<Cell>().Count(c=>c.Hit).ShouldBe(1);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void HandleShot_IfAlreadyHit_ShouldReturnAlreadyShotResultType(bool containShip)
    {
        //Given
        var board = GetBoard(containShip, true);
        var shipService = new ShipService(board);
        
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
        var board = GetBoard(containShip, false);
        var shipService = new ShipService(board);
        
        //When
        var result = shipService.HandleShot("A1");
        
        //Then
        result.ShouldBe(expectedResultType);
    }

    [Fact]
    public void HandleShot_IfFinalShot_ShouldReturnFinalShotResultType()
    {
        //Given
        var board = GetBoard(false, false);
        board[0, 0].ContainShip = true;
        var shipService = new ShipService(board);
        
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