namespace Battleships.Models;

public record Cell(bool ContainShip, bool Hit)
{
    public bool ContainShip { get; set; } = ContainShip;
    public bool Hit { get; set; } = Hit;
    public char Symbol => Hit && ContainShip ? CellSymbols.HitCellSymbol :
        Hit && !ContainShip ? CellSymbols.MissCellSymbol : CellSymbols.DefaultCellSymbol;
}