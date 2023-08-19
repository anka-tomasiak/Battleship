namespace Battleships.Exceptions;

public class UnableToPlaceShipException : Exception
{
    public UnableToPlaceShipException() : base("Unable to place ship on board.")
    {}
}