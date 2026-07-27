using System.Numerics;
using Box2DSharp.Dynamics;
using Raylib_cs;

static class Level
{
	public static World PhysicsWorld;
	public static readonly float Gravity = 9.81f * 1.7f;
	private static bool SimulatePhysics = true;

	public static Camera2D Camera;
	public static List<GameObject> GameObjects = [];

	public static Vector2 MousePosition => Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), Camera);

	private static double startTime;
	public static double Time => Raylib.GetTime() - startTime;

	public static void Init(LevelPrototype level)
	{
		// Create the box2d world
		PhysicsWorld = new World(new Vector2(0, Gravity));
		float metersToPixels = 20;

		// Make the camera
		Camera = new Camera2D();
		Camera.Target = new Vector2(metersToPixels, -10f);
		Camera.Offset = Raylib.GetScreenCenter();
		Camera.Zoom = metersToPixels;

		// Add everything to the level
		level.Populate();

		// Run all of the setup stuff
		// TODO: Remove this
		for (int i = GameObjects.Count - 1; i >= 0 ; i--)
		{
			GameObjects[i].PreSceneInit();
		}

		startTime = Raylib.GetTime();
	}

	public static void Update()
	{
		MoveCamera();

		LevelEditor.Update();

		// Update the physics stuff
		if (SimulatePhysics) PhysicsWorld.Step(Raylib.GetFrameTime(), 8, 3);

		// Update all the game objects
		for (int i = GameObjects.Count - 1; i >= 0 ; i--)
		{
			GameObjects[i].EngineUpdate();
		}
	}

	public static void Render()
	{
		Raylib.BeginMode2D(Camera);

		// Draw the 'sky'
		// TODO: Do this properly
		Raylib.ClearBackground(Color.Blue);

		// Draw all the game objects
		foreach (GameObject gameObject in GameObjects)
		{
			gameObject.Render();
		}

		Raylib.EndMode2D();

		// Draw all the game objects' ui
		foreach (GameObject gameObject in GameObjects)
		{
			gameObject.DrawUi();
		}
		LevelEditor.Render();

		if (SimulatePhysics == false) Raylib.DrawText($"Physics sim off", 10, 10, 16, Color.White);
	}

	public static void Unload()
	{
		// Unload all the game objects
		foreach (GameObject gameObject in GameObjects)
		{
			gameObject.Unload();
		}
	}

	private static void MoveCamera()
	{
		const float panSpeed = 10f;
		float movement = panSpeed * Raylib.GetFrameTime();

		// Move around
		{
			if (Raylib.IsKeyDown(KeyboardKey.Left)) Camera.Target.X -= movement;
			if (Raylib.IsKeyDown(KeyboardKey.Right)) Camera.Target.X += movement;

			if (Raylib.IsKeyDown(KeyboardKey.Up)) Camera.Target.Y -= movement;
			if (Raylib.IsKeyDown(KeyboardKey.Down)) Camera.Target.Y += movement;
		}

		// Zoom in/out
		Camera.Zoom += Raylib.GetMouseWheelMove();

		// Reset
		if (Raylib.IsKeyPressed(KeyboardKey.R)) Camera.Target = Vector2.Zero;

		// Debug save camera position
		// if (Raylib.IsKeyPressed(KeyboardKey.S)) Console.WriteLine($"CAMERA: {Camera.Target} @ {Camera.Zoom}%");
	}

	public static GameObject AddToLevel(GameObject newGameObject)
	{
		GameObjects.Add(newGameObject);
		return newGameObject;
	}
}