using System.Numerics;
using Raylib_cs;

class WoodenPole : PhysicsObject
{
	public WoodenPole(Vector2 position, float rotation)
	{
		Position = position;
		Rotation = rotation;
		
		Size = new Vector2(0.5f, 5f);
		PhysicsBody = Physics.CreateRectangle(this, Size);

		MainTexture = Graphics.GetRandomTexture("pole1", "pole2", "pole3");
	}
}