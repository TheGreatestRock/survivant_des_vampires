using Godot;

public partial class SaveManager : Node
{	
	public void SaveGame()
	{
		using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);

		var saveNodes = GetTree().GetNodesInGroup("saveable");
		foreach (Node saveNode in saveNodes)
		{
			if (string.IsNullOrEmpty(saveNode.SceneFilePath))
			{
				GD.Print($"persistent node '{saveNode.Name}' is not an instanced scene, skipped");
				continue;
			}

			if (!saveNode.HasMethod("Save"))
			{
				GD.Print($"persistent node '{saveNode.Name}' is missing a Save() function, skipped");
				continue;
			}

			var saveData = saveNode.Call("Save");
			var json = new Json();
			json.Data = saveData;
			saveFile.StoreLine(json.ToString());

			GD.Print($"Saved {saveNode.Name}");
		}
	}
	
	public void LoadGame()
	{
		if (!FileAccess.FileExists("user://savegame.save"))
		{
			return;
		}

		var saveNodes = GetTree().GetNodesInGroup("saveable");
		foreach (Node saveNode in saveNodes)
		{
			saveNode.QueueFree();
		}
		using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
		
		while (saveFile.GetPosition() < saveFile.GetLength())
		{
			var jsonString = saveFile.GetLine();

			var json = new Json();
			var parseResult = json.Parse(jsonString);
			if (parseResult != Error.Ok)
			{
				GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
				continue;
			}

			var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);

			var newObjectScene = GD.Load<PackedScene>(nodeData["Filename"].ToString());
			var newObject = newObjectScene.Instantiate<Node>();
			GetNode(nodeData["Parent"].ToString()).AddChild(newObject);
			newObject.Set(Node2D.PropertyName.Position, new Vector2((float)nodeData["PosX"], (float)nodeData["PosY"]));
		}
	}
}
