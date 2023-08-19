namespace Battleships.Exceptions;

public class InvalidCoordinateException : Exception
{
    public InvalidCoordinateException(): base("Invalid coordinate. Please enter coordinate like A1 (letter is row, digit is column). Max row: J, max column: 10")
    {}
}