using Raylib_cs;

class LevelEditor
{
	private static List<GameObject> spawnHistory = [];
	
	public static void Update()
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
}