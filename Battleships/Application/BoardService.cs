using Battleships.Models;
using Battleships.UserInterface;

namespace Battleships.Application;

public class BoardService : IBoardService
{
    private readonly IUserInterface _userInterface;
    private const int Size = Consts.BoardSize;

    public Cell[,] Board { get; }
    
    private BoardService(IUserInterface userInterface)
    {
        _userInterface = userInterface;
        Board = new Cell[Size, Size];
    }

    public static BoardService Create(IUserInterface userInterface)
    {
        var boardService = new BoardService(userInterface);
        boardService.InitializeBoard();
        return boardService;
    }

    public void PrintBoard()
    {
        _userInterface.WriteLine("    1 2 3 4 5 6 7 8 9 10");
        _userInterface.WriteLine("   ---------------------");

        for (var i = 0; i < Board.GetLength(0); i++)
        {
            _userInterface.Write($"{(char)('A' + i)} |");
            for (var j = 0; j < Board.GetLength(1); j++)
            {
                _userInterface.Write($" {Board[i, j].Symbol}");
            }
            _userInterface.WriteLine("");
        }
    }
    
    public void InitializeBoard()
    {
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                Board[i, j] = new Cell(false, false);
            }
        }
    }
}