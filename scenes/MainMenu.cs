using Godot;
using System;

public partial class MainMenu : Control
{
	GameManager gm;
	[Export] Button startGameButton;
	[Export] Button exitGameButton;

	public override void _Ready()
	{
		base._Ready();
		gm = GetNode<GameManager>("/root/GameManager");
		startGameButton.Pressed += OnStartGameButtonPressed;
		exitGameButton.Pressed += OnExitGameButtonPressed;
	}
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is InputEventKey eventAction)
		{

			if (eventAction.IsActionPressed("ui_cancel"))
			{
				Visible = !Visible;
			}
		}
	}
	void OnStartGameButtonPressed()
	{
		gm.StartGame();
	}
	void OnExitGameButtonPressed()
	{
		GetTree().Quit();
	}
}
