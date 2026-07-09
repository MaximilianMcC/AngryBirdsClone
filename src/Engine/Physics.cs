using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Raylib_cs;

class Physics
{
	public static Body CreateRectangle(GameObject gameObject, Vector2 size, BodyType bodyType = BodyType.DynamicBody)
	{
		Body body = CreateDefaultBody(gameObject, bodyType);

		PolygonShape rectangle = new PolygonShape();
		rectangle.SetAsBox(size.X / 2, size.Y / 2);
		AddShapeToBody(body, rectangle);

		return body;
	}

	public static Body CreateCircle(GameObject gameObject, Vector2 size, BodyType bodyType = BodyType.DynamicBody)
	{
		Body body = CreateDefaultBody(gameObject, bodyType);

		CircleShape circle = new CircleShape();
		circle.Radius = size.Y / 2;
		AddShapeToBody(body, circle);

		return body;
	}

	public static Body CreateTriangle(GameObject gameObject, Vector2 size, BodyType bodyType = BodyType.DynamicBody)
	{
		Body body = CreateDefaultBody(gameObject, bodyType);

		PolygonShape triangle = new PolygonShape();
		triangle.Set(new Vector2[]
		{
			new Vector2(0, -size.Y / 2),
			new Vector2(-size.X / 2, size.Y / 2),
			new Vector2(size.X / 2, size.Y / 2)
		});
		AddShapeToBody(body, triangle);

		return body;
	}

	private static Body CreateDefaultBody(GameObject gameObject, BodyType bodyType)
	{
		// Make the body
		BodyDef bodyDefinition = new BodyDef();
		bodyDefinition.BodyType = bodyType;
		bodyDefinition.Position = gameObject.Position + (gameObject.Size / 2f);
		bodyDefinition.Angle = gameObject.Rotation * Raylib.DEG2RAD;

		// Add it to the world
		Body body = Level.PhysicsWorld.CreateBody(bodyDefinition);
		return body;
	}

	private static void AddShapeToBody(Body body, Shape shape, float density = 1)
	{
		FixtureDef fixtureDefinition = new FixtureDef();
		fixtureDefinition.Shape = shape;
		fixtureDefinition.Density = density;
		fixtureDefinition.Friction = 0.8f;

		body.CreateFixture(fixtureDefinition);
	}
}