using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		//move the player
		velocity = direction * Speed;

		// animate the player
		if (direction.X != 0)
		{
			animatedSprite.Play("walk_left");
			animatedSprite.FlipH = direction.X > 0;
		}
		else {
			if (direction.Y == 0)
				animatedSprite.Stop();
			else
				animatedSprite.Play();
		}

		Velocity = velocity;
		MoveAndSlide();
		
		GD.Print(Position);
	}
	
	public Godot.Collections.Dictionary<string, Variant> Save()
	{
		return new Godot.Collections.Dictionary<string, Variant>()
		{
			{ "Filename", SceneFilePath },
			{ "Parent", GetParent().GetPath() },
			{ "PosX",  (int)Position.X },
			{ "PosY",  (int)Position.Y },
		};
	}
	
}
