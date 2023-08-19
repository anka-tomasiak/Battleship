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
        _shipService.GenerateShips();
        
        while (true)
        {
            _userInterface.WriteLine(Consts.InitialMessage);
            var input = _userInterface.Read()?.ToUpper();

            if (input == Consts.ExitCommand)
            {
                _userInterface.WriteLine(Consts.ExitCommand);
                break;
            }

            try
            {
                var result = _shipService.HandleShot(input);

                _userInterface.WriteLine(GetMessage(result));
                if (result != ShotResultType.AlreadyShot)
                {
                    _boardService.PrintBoard();
                }

                if (result != ShotResultType.FinalShot) continue;
                
                _userInterface.WriteLine(Consts.ExitCommand);
                break;
            }
            catch (InvalidCoordinateException ex)
            {
                _userInterface.WriteLine(ex.Message);
            }
        }
    }

    private string GetMessage(ShotResultType shotResultType)
    {
        return shotResultType switch
        {
            ShotResultType.Hit => Consts.HitMessage,
            ShotResultType.Miss => Consts.MissMessage,
            ShotResultType.FinalShot => Consts.FinalHitMessage,
            _ => Consts.AlreadyShotMessage
        };
    }
}