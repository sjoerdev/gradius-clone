namespace Project
{
    public class SpriteMap : GameObject
    {
        public Bitmap spritemap;
        public Vector2 mapLocation;
        public Vector2 spritesize;

        public void SetSprite(Vector2 NewPosition)
        {
            mapLocation = NewPosition;
        }
        public Vector2 GetSprite()
        {
            return mapLocation;
        }
        public void Draw(Vector2f location)
        {
            Engine.DrawBitmap(spritemap, location.X, location.Y, mapLocation.X * spritesize.X, mapLocation.Y * spritesize.Y, spritesize.X, spritesize.Y  );
        }
        public SpriteMap(string spriteMapPath, Vector2 spriteSize)
        {
            GameInitialize();
            GameStart();
            InitSpriteMap(spriteMapPath, spriteSize);
        }
        public void InitSpriteMap(string spriteMapPath, Vector2 spriteSize)
        {
            spritemap = new Bitmap(spriteMapPath);
            mapLocation = new Vector2(0, 0);
            spritesize = spriteSize;
        }

        public override void GameEnd()
        {
            Destroy();
        }
    }
}
