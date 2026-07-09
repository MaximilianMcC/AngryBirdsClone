using Box2DSharp.Dynamics;
using Raylib_cs;

static class Graphics
{
	public static Texture2D GetRandomTexture(params string[] textureNames)
	{
		string texture = textureNames[Random.Shared.Next(0, textureNames.Length)];

		return Raylib.LoadTexture($"./assets/{texture}.png");
	}
}