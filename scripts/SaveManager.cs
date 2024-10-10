using Godot;

public class SaveManager
{	
	// Now SaveGame requires a SceneTree reference to operate on nodes.
	public void SaveGame(SceneTree sceneTree)
	{
		using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);

		// Get all nodes in the "saveable" group from the passed sceneTree.
		var saveNodes = sceneTree.GetNodesInGroup("saveable");
		foreach (var saveObj in saveNodes)
		{
			if (saveObj is Node saveNode)
			{
				// Ensure the node is an instanced scene
				if (string.IsNullOrEmpty(saveNode.SceneFilePath))
				{
					GD.Print($"Persistent node '{saveNode.Name}' is not an instanced scene, skipped");
					continue;
				}

				// Check if the node has a 'Save' method
				if (!saveNode.HasMethod("Save"))
				{
					GD.Print($"Persistent node '{saveNode.Name}' is missing a Save() function, skipped");
					continue;
				}

				// Call the node's Save method and serialize the data
				var nodeData = saveNode.Call("Save");
				var jsonString = Json.Stringify(nodeData);
				saveFile.StoreLine(jsonString);

				GD.Print($"Saved {saveNode.Name}");
			}
			else
			{
				GD.Print("Object in 'saveable' group is not a Node, skipped");
			}
		}
	}
	
	// LoadGame also requires a SceneTree reference
	public void LoadGame(SceneTree sceneTree)
	{
		if (!FileAccess.FileExists("user://savegame.save"))
		{
			return;
		}

		// Clear existing saveable nodes from the scene
		var saveNodes = sceneTree.GetNodesInGroup("saveable");
		foreach (var saveObj in saveNodes)
		{
			if (saveObj is Node saveNode)
			{
				saveNode.QueueFree();
			}
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

			// Deserialize the saved data
			var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);

			// Load the scene from file
			var newObjectScene = GD.Load<PackedScene>(nodeData["Filename"].ToString());
			var newObject = newObjectScene.Instantiate<Node>();

			// Add the new object to the parent node in the scene
			Node parent = sceneTree.Root.GetNode(nodeData["Parent"].ToString());
			parent.AddChild(newObject);

			// Restore object position and other properties
			GD.Print(new Vector2((float)nodeData["PosX"], (float)nodeData["PosY"]));
			newObject.Set(Node2D.PropertyName.Position, new Vector2((float)nodeData["PosX"], (float)nodeData["PosY"]));
		}
	}
}
