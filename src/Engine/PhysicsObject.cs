using System.Numerics;
using Box2DSharp.Dynamics;
using Raylib_cs;

class PhysicsObject : GameObject
{
	public Body PhysicsBody;

	public bool SimulatePhysics
	{
		get => PhysicsBody.IsAwake;
		set => PhysicsBody.IsAwake = value;
	}

	public override void Update()
	{
		// Sync the game objects position with the physics
		Position = PhysicsBody.GetPosition() - (Size / 2f);
		Rotation = PhysicsBody.GetAngle() * Raylib.RAD2DEG;
	}

	public void TeleportTo(Vector2 position, bool forcePhysicsAfterwards = true)
	{
		Position = position;
		PhysicsBody.SetTransform(position + Size / 2f, PhysicsBody.GetAngle());

		if (forcePhysicsAfterwards) SimulatePhysics = true;
	}
}