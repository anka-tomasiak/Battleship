using Battleships.Models;

namespace Battleships.Tests.Unit.Models;

public class CellTests
{
    [InlineData(true, false, Consts.DefaultCellSymbol)]
    [InlineData(false, false, Consts.DefaultCellSymbol)]
    [InlineData(true, true, Consts.HitCellSymbol)]
    [InlineData(false, true, Consts.MissCellSymbol)]
    [Theory]
    public void GetSymbol_ForGivenProperties_ShouldReturnCorrectSymbol(bool containShip, bool hit, char expectedSymbol)
    {
        //Given-When
        var cell = new Cell(containShip, hit);
        
        //Then
        cell.Symbol.ShouldBe(expectedSymbol);
    }
}