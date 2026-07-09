using System.Numerics;
using Raylib_cs;

class Piggy : PhysicsObject
{
	public Piggy(Vector2 position)
	{
		Position = position;
		
		Size = new Vector2(2f);
		PhysicsBody = Physics.CreateCircle(this, Size);

		MainTexture = Raylib.LoadTexture("./assets/piggy.png");
	}
}