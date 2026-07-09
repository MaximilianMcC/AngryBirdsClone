using System.Numerics;
using Box2DSharp.Dynamics;
using Raylib_cs;

class Ground : PhysicsObject
{
	public Ground()
	{
		Size = new Vector2(40f, 3f);
		PhysicsBody = Physics.CreateRectangle(this, Size, BodyType.StaticBody);

		MainTexture = Raylib.LoadTexture("./assets/ground.png");
	}
}