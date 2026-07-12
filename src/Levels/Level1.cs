using System.Numerics;

class Level1 : LevelPrototype
{
	public override string Name => "Level 1";

	public override void Populate()
	{
		Spawn(new Ground());

		Spawn(new Red(new Vector2(5, -8f)));
		Spawn(new Chuck(new Vector2(0, -8f)));
		Spawn(new Slingshot(new Vector2(10, -6f)));

		Spawn(new Piggy(new Vector2(31, -8f)));
		Spawn(new WoodenPole(new Vector2(30, -5f), 0f));
		Spawn(new WoodenPole(new Vector2(33, -5f), 0f));
		Spawn(new WoodenPole(new Vector2(31.5f, -8f), 90f));
	}
}