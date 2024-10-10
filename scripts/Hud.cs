using Godot;
using System;

public partial class Hud : CanvasLayer
{
	// Buttons
	private Button button1;
	private Button button2;
	private Button button3;
	private Button button4;

	// Scene and script paths
	private string levelBlueScene = "res://scenes/level_blue.tscn";
	private string levelRedScene = "res://scenes/level_red.tscn";

	// Called when the node enters the scene tree
	public override void _Ready()
	{
		// Get the button nodes (assuming their names are button1, button2, etc.)
		button1 = GetNode<Button>("Button");
		button2 = GetNode<Button>("Button2");
		button3 = GetNode<Button>("Button3");
		button4 = GetNode<Button>("Button4");

		// Connect button signals to respective methods
		button1.Pressed += _on_button_pressed;
		button2.Pressed += _on_button_2_pressed;
		button3.Pressed += _on_button_3_pressed;
		button4.Pressed += _on_button_4_pressed;
	}

	// Method to load the level_blue scene
	private void _on_button_pressed()
	{
		GameManager.Get().GetLevelManager().LoadLevel(levelBlueScene);
	}

	// Method to load the level_red scene
	private void _on_button_2_pressed()
	{
		GameManager.Get().GetLevelManager().LoadLevel(levelRedScene);
	}

	// Method to save the game
	private void _on_button_3_pressed()
	{
		GameManager.Get().GetSaveManager().SaveGame(GetTree());
	}

	// Method to load the game
	private void _on_button_4_pressed()
	{
		GameManager.Get().GetSaveManager().LoadGame(GetTree());
	}
		
}
