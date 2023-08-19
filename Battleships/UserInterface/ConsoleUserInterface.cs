namespace Battleships.UserInterface;

public class ConsoleUserInterface : IUserInterface
{
    public string? Read() => Console.ReadLine();

    public void Write(string message)
    {
        Console.Write(message);
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}