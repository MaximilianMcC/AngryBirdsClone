using System.Numerics;
using Raylib_cs;

class Piggy : PhysicsObject
{
	public bool movingRn = false;
	private bool previouslyMoving;
	private float initialSpawningInMovementDetectionGracePeriodInSeconds = 0.5f;

	public Piggy(Vector2 position)
	{
		Position = position;
		
		Size = new Vector2(2f);
		PhysicsBody = Physics.CreateCircle(this, Size);

		MainTexture = Raylib.LoadTexture("./assets/piggy.png");
	}

	public override void Update()
	{
		// Check for if we've been moved
		if (Level.Time <= initialSpawningInMovementDetectionGracePeriodInSeconds) return;
		movingRn = Position != PreviousPosition;

		// Check for if the pig has had a lot of velocity on it (hit)
		Console.WriteLine(PhysicsBody.LinearVelocity);

		// Check for if we were moving then stopped
		// if (previouslyMoving && movingRn == false)
		{
			// Check for if the pig 
		}

		previouslyMoving = movingRn;
	}

	public override void DrawUi()
	{
		Raylib.DrawText($"{movingRn}\n{PhysicsBody.LinearVelocity}", 10, 10, 16, Color.White);
	}
}