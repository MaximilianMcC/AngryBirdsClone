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
	private Vector2 previousBirdFirePosition;

	public Bird Bird;
	public bool Active;

	public Slingshot(Vector2 position)
	{
		Position = position;

		Size = new Vector2(2f, 7f) * 0.8f;
		MainTexture = Raylib.LoadTexture("./assets/slingshot.png");
	}

	public override void PreSceneInit()
	{
		// Get the bird we're working with
		Bird = Level.GameObjects.OfType<Bird>().FirstOrDefault();
		Bird.InSlingshot = true;

		// Have its physics off until we shoot it
		//! I don't think this actually turns it off properly
		Bird.SimulatePhysics = false;
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
			Bird.TeleportTo(Level.MousePosition - (Bird.Size * birdOffset), false);

			// Set the slingshot rotation to make the bird rotate towards the top of the slingshot
			Vector2 direction = Position - Bird.Position;
			Bird.SlingshotRotation = MathF.Atan2(direction.Y, direction.X) * Raylib.RAD2DEG;
		}

		// Check for if we've released the bird
		if (previouslyPullingBackwards && PullingBackwards == false)
		{
			// Get rid of external forces
			Bird.PhysicsBody.SetLinearVelocity(Vector2.Zero);
			Bird.PhysicsBody.SetAngularVelocity(0f);

			// Store our impulse
			currentImpulse = CalculatePotentialImpulse();
			Bird.PhysicsBody.ApplyLinearImpulseToCenter(currentImpulse, true);

			// Store our fire conditions
			previousImpulse = currentImpulse;
			previousBirdFirePosition = Bird.CenteredPosition;
		}

		previouslyPullingBackwards = PullingBackwards;
	}

	public override void Draw()
	{
		// Draw our old previous path
		DrawPredictedTrajectory(previousBirdFirePosition, previousImpulse, 5, Color.LightGray);

		// Draw our live 'new' prediction
		if (PullingBackwards)
		{
			DrawPredictedTrajectory(
				Bird.CenteredPosition,
				CalculatePotentialImpulse(),
				3,
				Color.White
			);
		}

		Bird.Draw();

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

	public void DrawPredictedTrajectory(Vector2 position, Vector2 impulse, int framesPerDot, Color color)
	{
		// Ensure we've got something to do
		if (impulse == Vector2.Zero) return;

		// TODO: Maybe use the current fps of the game 
		// TODO: Move these up the top
		const float deltaTime = 1 / 60f;
		const float secondsToSimulate = 5f;

		Vector2 velocity = impulse / Bird.PhysicsBody.Mass;
		Vector2 simulatedPosition = position;

		const int framesToSimulate = (int)(secondsToSimulate / deltaTime);
		for (int i = 0; i < framesToSimulate; i++)
		{
			// Check for if we're due to draw a dot
			if (i % framesPerDot == 0)
			{
				Raylib.DrawCircleV(simulatedPosition, 0.15f, color);
			}

			// Run the simulation
			// TODO: Maybe stop the simulation if there is collision
			velocity += new Vector2(0, Level.Gravity) * deltaTime;
			simulatedPosition += velocity * deltaTime;
		}
	}
}