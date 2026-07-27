using System.Collections;
using System.Numerics;
using Raylib_cs;

abstract class Pole : PhysicsObject;

class WoodenPole : Pole
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

class SteelPole : Pole
{
	public SteelPole(Vector2 position, float rotation)
	{
		Position = position;
		Rotation = rotation;
		
		Size = new Vector2(0.5f, 5f);
		PhysicsBody = Physics.CreateRectangle(this, Size);

		MainTexture = Graphics.GetRandomTexture("steel1", "steel2", "steel3");
	}
}

class PoleFactory
{
	public static Pole CreatePole(PoleType type, Vector2 position, float rotation)
	{
		return type switch
		{
			PoleType.Wooden => new WoodenPole(position, rotation),
			PoleType.Steel => new SteelPole(position, rotation),
			_ => null
		};
	}
}

public enum PoleType
{
	Wooden,
	Steel
}