using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

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
    [Signal]
    public delegate void RunStartedEventHandler();
    public bool isOnTutorial = true;
    public Run currentRun;

    public override void _Ready()
    {
        base._Ready();
    }

    public void StartGame()
    {
        Run newRun = new Run();
        currentRun = newRun;
        GetTree().Root.AddChild(newRun);
        GetNode<CardManager>("/root/CardManager").StartBoard(currentRun);
        EmitSignal(SignalName.RunStarted);
    }
}
