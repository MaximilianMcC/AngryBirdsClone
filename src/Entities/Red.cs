using System.Numerics;
using Raylib_cs;

class Red : Bird
{
    public Red(Vector2 position) : base(position) { }

    public override string Name => "Red";

    public override void Init()
    {
		PhysicsBody = Physics.CreateCircle(this, Size);
    	MainTexture = Raylib.LoadTexture("./assets/red.png");
    }
}