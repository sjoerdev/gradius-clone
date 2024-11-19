using System.Numerics;

namespace Spork;

public class UISprite : GameObject
{
    public Vector2 position;
    public SpriteMap spritemap;

    public UISprite()
    {
        GameInitialize();
        GameStart();
    }

    public override void Paint()
    {
        base.Paint();
    }

    public override void GameEnd()
    {
        // dispose
        Destroy();
    }
}
