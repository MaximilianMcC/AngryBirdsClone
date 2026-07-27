using System.Numerics;

class TestLevel : LevelPrototype
{
	public override string Name => "Test level";

	public override void Populate()
	{
		PopulateFromFile("./assets/levels/test.lvl");
	}
}