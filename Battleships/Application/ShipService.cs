using Battleships.Exceptions;
using Battleships.Models;

namespace Battleships.Application;

public class ShipService : IShipService
{
    private readonly Cell[,] _board;
    private readonly Random _random;
    public List<Ship> Ships { get; }

    public ShipService(Cell[,] board)
    {
        _board = board;
        _random = new Random();
        Ships = new List<Ship>
        {
            new Battleship(),
            new Destroyer(),
            new Destroyer()
        };
    }

    public void GenerateShips()
    {
        Ships.ForEach(GenerateShip);
    }

    public ShotResultType HandleShot(string? coordinate)
    {
        if (!TryParseCoordinate(coordinate, out var row, out var column))
        {
            throw new InvalidCoordinateException();
        }

        if (_board[column, row].Hit) return ShotResultType.AlreadyShot;
        
        _board[column, row].Hit = true;
        return _board[column, row].ContainShip ? 
            IsFinalShot()? ShotResultType.FinalShot: ShotResultType.Hit 
            : ShotResultType.Miss;
    }

    private void GenerateShip(Ship ship)
    {
        var isHorizontal = _random.Next(2) == 0;
        var maxX = Consts.BoardSize - 1 - (isHorizontal ? ship.Size - 1 : 0);
        var maxY = Consts.BoardSize - 1 - (isHorizontal ? 0 : ship.Size - 1);
        
        int startX, startY;
        var attempt = 0;

        do
        {
            startX = _random.Next(maxX + 1);
            startY = _random.Next(maxY + 1);
            attempt++;

            if (attempt >= Consts.MaxShipPlacementAttempts)
            {
                throw new UnableToPlaceShipException();
            }
        } while (!CanPlaceShip(startX, startY, ship.Size, isHorizontal, _board));
        
        for (var i = 0; i < ship.Size; i++)
        {
            if (isHorizontal)
            {
                _board[startX + i, startY].ContainShip = true;
            }
            else
            {
                _board[startX, startY + i].ContainShip = true;
            }
        }
    }

    private static bool CanPlaceShip(int x, int y, int size, bool isHorizontal, Cell[,] board)
    {
        var minX = Math.Max(0, x);
        var minY = Math.Max(0, y);
        var maxX = Math.Min(board.GetLength(0) - 1, isHorizontal ? x + size : x);
        var maxY = Math.Min(board.GetLength(1) - 1, isHorizontal ? y : y + size);
        
        for (var i = minX; i <= maxX; i++)
        {
            for (var j = minY; j <= maxY; j++)
            {
                if (board[i, j].ContainShip)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool TryParseCoordinate(string? input, out int row, out int column)
    {
        row = -1;
        column = -1;

        if (string.IsNullOrEmpty(input) || input.Length < 2 || input.Length > 3 || !char.IsLetter(input[0]) || !char.IsDigit(input[1]))
        {
            return false;
        }

        if (input.Length == 2)
        {
            row = input[1] - '1';
        }
        else
        {
            if (!char.IsDigit(input[2]))
            {
                return false;
            }

            var numberPart = input[^2..];
            row = int.Parse(numberPart) - 1;
        }

        column = input[0] - 'A';

        return row is >= 0 and < Consts.BoardSize && column is >= 0 and < Consts.BoardSize;
    }

    private bool IsFinalShot()
    {
        return !_board.Cast<Cell>().Any(c => c.ContainShip && !c.Hit);
    }
}