using Godot;
using System;

public partial class GameManager : Node
{
    public static Run currentRun;

    public static void StartGame()
    {
        currentRun = new Run();
        CardManager.StartBoard();
    }
}
