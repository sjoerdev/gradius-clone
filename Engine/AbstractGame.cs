using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class AbstractGame : GameObject
    {
        public override void GameInitialize()
        {
            // Set the required values
            GAME_ENGINE.SetTitle("Gradius Clone Sjoerd Wouters");
            GAME_ENGINE.SetIcon("gradius_icon.ico");

            // Set the optional values
            GAME_ENGINE.SetScreenWidth(800);
            GAME_ENGINE.SetScale(2, 2);
            GAME_ENGINE.SetScreenHeight(600);
            GAME_ENGINE.SetBackgroundColor(0, 0, 0);
        }

        public override void GameStart()
        {
            new Menu();
        }
    }
}
