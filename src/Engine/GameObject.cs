using System.Numerics;
using Raylib_cs;

abstract class GameObject
{
	public Vector2 Position;
	public float Rotation;
	public Vector2 Size = new Vector2(10);

	public Vector2 CenteredPosition => Position + (Size / 2f);

	protected Texture2D MainTexture;

	public virtual void PreSceneInit() { }

	public virtual void Update() { }
	
	/// <summary>Drawing the actual thing only.</summary>
	public virtual void Draw()
	{
		Raylib.DrawTexturePro(
			MainTexture,
			new Rectangle(0, 0, MainTexture.Dimensions),
			new Rectangle(CenteredPosition, Size),
			Size / 2,
			Rotation,
			Color.White
		);
	}

	/// <summary>Put the actual thing on the screen. Pretty much just calls <c>Draw()</c>.</summary>
	public virtual void Render() => Draw();

	public virtual void Unload()
	{
		Raylib.UnloadTexture(MainTexture);
	}
}