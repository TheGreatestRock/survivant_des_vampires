using Godot;

public partial class GameManager : Node 
{
	private static GameManager instance;

	private LevelManager levelManager;
	private SaveManager saveManager;

	public static GameManager Get()
	{
		return instance;
	}

	public LevelManager GetLevelManager()
	{
		return levelManager;
	}

	public SaveManager GetSaveManager()
	{
		
		return saveManager;
	}

	public override void _Ready()
	{
		base._Ready();
		
		instance = this;

		levelManager = new LevelManager();
		saveManager = new SaveManager();
	}
}
