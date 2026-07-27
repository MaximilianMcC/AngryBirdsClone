using System.Numerics;
using System.Text;
using Raylib_cs;

class LevelEditor
{
	private static List<GameObject> spawnHistory = [];
	
	public static void Update()
	{
		SpawnStuff();
		UndoStuff();
		SerializeLevel();
	}

	public static void SpawnStuff()
	{
		// Press P to spawn a pig
		if (Raylib.IsKeyPressed(KeyboardKey.P))
		{
			spawnHistory.Add(Level.AddToLevel(new Piggy(Level.MousePosition)));
		}

		// Press H to spawn a horizontal beam
		if (Raylib.IsKeyPressed(KeyboardKey.H))
		{
			spawnHistory.Add(Level.AddToLevel(new WoodenPole(Level.MousePosition, 90f)));
		}

		// Press V to spawn a vertical beam
		if (Raylib.IsKeyPressed(KeyboardKey.V))
		{
			spawnHistory.Add(Level.AddToLevel(new WoodenPole(Level.MousePosition, 0f)));
		}
	}

	public static void UndoStuff()
	{
		// If we do ctrl+z then remove the last thing added
		if ((Raylib.IsKeyPressed(KeyboardKey.Z) || Raylib.IsKeyPressedRepeat(KeyboardKey.Z)) && Raylib.IsKeyDown(KeyboardKey.LeftControl))
		{
			// Get the last thing that we spawned
			if (spawnHistory.Count == 0) return;
			GameObject lastThingSpawned = spawnHistory.Last();

			// Un-spawn it
			lastThingSpawned.Unload();
			Level.GameObjects.Remove(lastThingSpawned);

			// Remove it from the history (gone)
			// TODO: Add a redo list or something idk
			spawnHistory.Remove(lastThingSpawned);
		}	
	}

	public static void SerializeLevel()
	{
		// Check for if we do Ctrl+S to save the level
		// TODO: Tiny file dialogue
		if (((Raylib.IsKeyPressed(KeyboardKey.S) || Raylib.IsKeyPressedRepeat(KeyboardKey.S)) && Raylib.IsKeyDown(KeyboardKey.LeftControl)) == false) return;
		string levelPath = "./assets/levels/test.lvl";

		// Get everything in the game that's not a bird
		List<GameObject> level = Level.GameObjects.Where(thing => thing is not Bird).ToList();

		// Serialise all of the things in the level
		StringBuilder serializedLevel = new StringBuilder();
		foreach (GameObject thing in level)
		{
			// Position and rotation
			serializedLevel.Append($"{thing.Position.X},{thing.Position.Y},{thing.Rotation}");

			// Type
			serializedLevel.Append($",{thing.GetType()}");

			serializedLevel.AppendLine("");
		}

		// Save it to a file
		File.WriteAllText(levelPath, serializedLevel.ToString());
		Console.WriteLine($"Written level to {levelPath}");
	}
}

abstract class LevelPrototype
{
	public abstract string Name { get; }

	public abstract void Populate();

	public static void PopulateFromFile(string fileName)
	{
		// Add the ground since every level has this
		Spawn(new Ground());

		string[] things = File.ReadAllLines(fileName);
		foreach (string thingPrototype in things)
		{
			string[] settings = thingPrototype.Split(',');
			object[] constructorArgs = new object[]
			{
				// Position
				new Vector2(
					float.Parse(settings[0]),
					float.Parse(settings[1])
				),

				// Rotation
				float.Parse(settings[2])
			};

			// Get the type
			Type type = Type.GetType(settings[3]);

			// Create a new thing and add it to the level
			GameObject gameObject = Activator.CreateInstance(type, constructorArgs) as GameObject;
			Spawn(gameObject);
		}
	}

	public static void Spawn(GameObject gameObject) => Level.GameObjects.Add(gameObject);

}