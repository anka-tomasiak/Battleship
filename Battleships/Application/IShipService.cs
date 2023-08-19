using Battleships.Models;

namespace Battleships.Application;

public interface IShipService
{
    void GenerateShips();
    ShotResultType HandleShot(string? coordinate);
}