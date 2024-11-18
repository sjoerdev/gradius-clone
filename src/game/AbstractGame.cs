using System;
using Project;

namespace Project
{
    public class AbstractGame : GameObject
    {
        public override void GameInitialize()
        {
            Engine.title = "Gradius Clone Sjoerd Wouters";
            Engine.windowWidth = 800;
            Engine.windowHeight = 600;
            Engine.scale = new(2, 2);
            Engine.clearColor = new(0, 0, 0);
        }

        public override void GameStart()
        {
            new Menu();
        }
    }
}
