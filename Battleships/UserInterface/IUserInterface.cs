namespace Battleships.UserInterface;

public interface IUserInterface
{
    string? Read();
    void Write(string message);
    void WriteLine(string message);
}