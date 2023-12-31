﻿namespace Battleships.Models;

public static class Consts
{
    public const int BoardSize = 10;
    public const int MaxShipPlacementAttempts = 100;
    public const string ExitCommand = "EXIT";
    public const char DefaultCellSymbol = '.';
    public const char HitCellSymbol = 'X';
    public const char MissCellSymbol = 'O';
    public const string HitMessage = "Incredible! Hit! Sink it!";
    public const string MissMessage = "Miss! He's hiding... but you won't give up that easily, right?";
    public const string FinalHitMessage = "Congratulations! You were born a winner!";
    public const string AlreadyShotMessage = "Insanity is doing the same thing over and over and expecting different results.";
    public const string ExitMessage = "Goodbye!";
    public const string InitialMessage = "Enter coordinates (e.g., A1), or type 'EXIT' to quit.";
    public const string TryAgainMessage = "Do you want to try again? If so, type 'yes'.";
    public const string TryAgainCommand = "yes";
    public const string ReallyBadExceptionMessage =
        "Whoops! This app seems to needs a moment to catch its breath. Our best specialists will take care of it.";
}