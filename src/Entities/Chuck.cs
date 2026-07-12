using System.Numerics;
using Raylib_cs;

class Chuck : Bird
{
    public Chuck(Vector2 position) : base(position) { }

    public override string Name => "Chuck";

    public override void Init()
    {
		PhysicsBody = Physics.CreateTriangle(this, Size);
    	MainTexture = Raylib.LoadTexture("./assets/chuck.png");
    }
}