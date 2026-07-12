using System.Numerics;
using Raylib_cs;

abstract class Bird : PhysicsObject
{
	public abstract string Name { get; }

	public bool HasAlreadyBeenFired = false;
	public bool InSlingshot = false;
	public float SlingshotRotation;

	public Bird(Vector2 position)
	{
		Position = position;
		Size = new Vector2(2f);

		// Add the texture and the physics shape
		Init();
	}

	public abstract void Init();

	public override void Draw()
	{
		Raylib.DrawTexturePro(
			MainTexture,
			new Rectangle(0, 0, MainTexture.Dimensions),
			new Rectangle(CenteredPosition, Size),
			Size / 2,
			InSlingshot ? SlingshotRotation : Rotation,
			Color.White
		);	
	}

	public override void Render()
	{
		// Don't draw if we're in the slingshot (slingshot will draw us)
		if (InSlingshot) return;
		base.Render();
	}
}