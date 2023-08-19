using Battleships.Models;

namespace Battleships.Tests.Unit.Models;

public class CellTests
{
    [InlineData(true, false, CellSymbols.DefaultCellSymbol)]
    [InlineData(false, false, CellSymbols.DefaultCellSymbol)]
    [InlineData(true, true, CellSymbols.HitCellSymbol)]
    [InlineData(false, true, CellSymbols.MissCellSymbol)]
    [Theory]
    public void GetSymbol_ForGivenProperties_ShouldReturnCorrectSymbol(bool containShip, bool hit, char expectedSymbol)
    {
        //Given-When
        var cell = new Cell(containShip, hit);
        
        //Then
        cell.Symbol.ShouldBe(expectedSymbol);
    }
}