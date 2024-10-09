using Godot;

public class LevelManager
{
	public void LoadLevel(string scenePath)
	{
		if (ResourceLoader.Exists(scenePath))
		{
			GameManager.Get().GetTree().ChangeSceneToFile(scenePath);
			GD.Print($"La scène {scenePath} a été chargée.");
		}
		else
		{
			GD.PrintErr($"La scène {scenePath} n'a pas pu être trouvée.");
		}
	}
}
