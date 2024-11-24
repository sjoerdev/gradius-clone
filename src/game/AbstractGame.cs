using System;
using Spork;

namespace Spork;

public class AbstractGame : GameObject
{
    public override void GameInitialize()
    {
        engine.title = "Gradius Clone Sjoerd Wouters";
        engine.windowWidth = 800;
        engine.windowHeight = 600;
        engine.scale = new(2, 2);
    }

    public override void GameStart()
    {
        new Menu();
    }
}
