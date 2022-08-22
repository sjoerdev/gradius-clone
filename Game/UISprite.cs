namespace GameEngine
{
    public class UISprite : GameObject
    {
        public Vector2f position;
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
            Dispose();
        }
    }
}
