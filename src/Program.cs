using Raylib_cs;

class Program
{
	public static void Main(string[] args)
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.InitWindow(800, 500, "That sure is one angry bird");
		Raylib.SetExitKey(KeyboardKey.Null);

		Level.Init(new Level1());

		while (!Raylib.WindowShouldClose())
		{
			Level.Update();

			Raylib.BeginDrawing();
				Raylib.ClearBackground(Color.Magenta);
				Level.Render();	
			Raylib.EndDrawing();
		}

		Level.Unload();
		Raylib.CloseWindow();
	}
}