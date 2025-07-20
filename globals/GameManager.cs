using Godot;
using System;

public enum CardType
{
    Person,
    SpecialAction,
    BasicAction
}

public enum CardSuits
{
    Clergy,
    Militia,
    Merchant,
    Opponent,
}

public partial class GameManager : Node
{
    public static bool isOnTutorial = true;
    public static Run currentRun;

    public override void _Ready()
    {
        base._Ready();
        StartGame();
    }

    public static void StartGame()
    {
        currentRun = new Run();
        CardManager.StartBoard();
    }
}
