namespace Battleships.Exceptions;

public class UnableToPlaceShipException : Exception
{
    public UnableToPlaceShipException() : base("Unable to place ships on board.")
    {}
}