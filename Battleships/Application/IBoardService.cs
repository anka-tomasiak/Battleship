using Battleships.Models;

namespace Battleships.Application;

public interface IBoardService
{
    void PrintBoard();
    void InitializeBoard();
    Cell[,] Board { get; }
}