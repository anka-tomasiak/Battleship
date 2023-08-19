using Battleships.Exceptions;
using Battleships.Models;
using Battleships.UserInterface;

namespace Battleships.Application;

public class GameService
{
    private readonly IUserInterface _userInterface;
    private readonly IBoardService _boardService;
    private readonly IShipService _shipService;

    public GameService(IUserInterface userInterface, IBoardService boardService, IShipService shipService)
    {
        _userInterface = userInterface;
        _boardService = boardService;
        _shipService = shipService;
    }

    public void Play()
    {
        while (true)
        {
            _userInterface.WriteLine("Enter coordinates (e.g., A1), or type 'EXIT' to quit.");
            var input = _userInterface.Read()?.ToUpper();

            if (input == Consts.ExitCommand)
            {
                _userInterface.WriteLine("Exiting");
                break;
            }

            try
            {
                var result = _shipService.HandleShot(input);

                if (result != ShotResultType.AlreadyShot)
                {
                    _boardService.PrintBoard();
                }
            }
            catch (InvalidCoordinateException ex)
            {
                _userInterface.WriteLine(ex.Message);
            }
        }
    }
}