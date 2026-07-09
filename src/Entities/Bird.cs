using System.Numerics;
using Raylib_cs;

class Bird : PhysicsObject
{
	public bool InSlingshot = false;
	public float SlingshotRotation;

	public Bird(Vector2 position)
	{
		Position = position;
		
		Size = new Vector2(2f);
		PhysicsBody = Physics.CreateCircle(this, Size);

		MainTexture = Raylib.LoadTexture("./assets/red.png");
	}

	public override void Draw()
	{
		Raylib.DrawTexturePro(
			MainTexture,
			new Rectangle(0, 0, MainTexture.Dimensions),
			new Rectangle(Position + (Size / 2f), Size),
			Size / 2,
			InSlingshot ? SlingshotRotation : Rotation,
			Color.White
		);	
	}

	public override void Render()
	{
		// Don't draw if were in the slingshot (slingshot will draw us)
		if (InSlingshot) return;
		base.Render();
	}
}