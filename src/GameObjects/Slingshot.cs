using System.Numerics;
using Raylib_cs;

class Slingshot : GameObject
{
	private Color backBandColor = new Color(71, 47, 34, 255);
	private Color frontBandColor = new Color(115, 76, 54, 255);
	private float bandThickness = 0.4f;

	public bool PullingBackwards = false;
	private bool previouslyPullingBackwards = false;

	private float fireMultiplier = 20f;
	private Vector2 currentImpulse;
	private Vector2 previousImpulse;

	public Bird bird;

	public Slingshot(Vector2 position)
	{
		Position = position;

		Size = new Vector2(2f, 7f) * 0.8f;
		MainTexture = Raylib.LoadTexture("./assets/slingshot.png");
	}

	public override void PreSceneInit()
	{
		// Get the bird we're working with
		bird = Level.GameObjects.OfType<Bird>().FirstOrDefault();
		bird.InSlingshot = true;

		// Have its physics off until we shoot it
		//! I don't think this actually turns it off properly
		bird.SimulatePhysics = false;
	}

	private Vector2 CalculatePotentialImpulse()
	{
		Vector2 cradleCenter = Position + (Size * new Vector2(0.5f, 0.1f));

		// Get the direction and power in which to shoot
		// TODO: Check for zero
		Vector2 direction = Vector2.Normalize(cradleCenter - Level.MousePosition);
		float firePower = Vector2.Distance(Level.MousePosition, cradleCenter) * fireMultiplier;

		Vector2 impulse = direction * firePower;
		return impulse;
	}

	public override void Update()
	{
		PullingBackwards = Raylib.IsMouseButtonDown(MouseButton.Left);
		// if (previouslyPullingBackwards == false && PullingBackwards) Console.WriteLine("Begin pull");

		// If we've got the bird in our slingshot
		// then make it follow the cursor
		if (PullingBackwards)
		{
			// Place the bird in the slingshot
			Vector2 birdOffset = new Vector2(0.1f, 0.8f);
			bird.TeleportTo(Level.MousePosition - (bird.Size * birdOffset), false);

			// Set the slingshot rotation to make the bird rotate towards the top of the slingshot
			Vector2 direction = Position - bird.Position;
			bird.SlingshotRotation = MathF.Atan2(direction.Y, direction.X) * Raylib.RAD2DEG;
		}

		// Check for if we've released the bird
		if (previouslyPullingBackwards && PullingBackwards == false)
		{
			// Get rid of external forces
			bird.PhysicsBody.SetLinearVelocity(Vector2.Zero);
			bird.PhysicsBody.SetAngularVelocity(0f);

			// Store our impulse
			currentImpulse = CalculatePotentialImpulse();
			bird.PhysicsBody.ApplyLinearImpulseToCenter(currentImpulse, true);
		}

		previouslyPullingBackwards = PullingBackwards;
	}

	public override void Draw()
	{
		if (PullingBackwards) DrawPredictedTrajectory();

		bird.Draw();

		if (PullingBackwards)
		{
			Vector2 backPost = Position + (Size * new Vector2(0.8f, 0.1f));
			Raylib.DrawLineEx(
				Level.MousePosition,
				backPost,
				bandThickness,
				backBandColor
			);
		}

		base.Draw();

		if (PullingBackwards)
		{
			Vector2 frontPost = Position + (Size * new Vector2(0.18f, 0.12f));
			Raylib.DrawLineEx(
				Level.MousePosition,
				frontPost,
				bandThickness,
				frontBandColor
			);
		}
	}

	public void DrawPredictedTrajectory()
	{
		// TODO: Maybe use the current fps of the game 
		// TODO: Move these up the top
		const float deltaTime = 1 / 60f;
		const float secondsToSimulate = 5f;
		const float framesPerDot = 3f;

		Vector2 velocity = CalculatePotentialImpulse() / bird.PhysicsBody.Mass;
		Vector2 simulatedPosition = bird.PositionCenter;

		const int framesToSimulate = (int)(secondsToSimulate / deltaTime);
		for (int i = 0; i < framesToSimulate; i++)
		{
			// Check for if we're due to draw a dot
			if (i % framesPerDot == 0)
			{
				Raylib.DrawCircleV(simulatedPosition, 0.15f, Color.White);
			}

			// Run the simulation
			// TODO: Maybe stop the simulation if there is collision
			velocity += new Vector2(0, Level.Gravity) * deltaTime;
			simulatedPosition += velocity * deltaTime;
		}
	}
}