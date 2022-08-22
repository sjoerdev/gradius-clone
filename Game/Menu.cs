using System;
using System.Collections.Generic;
using System.IO;

namespace GameEngine
{
    public class Menu : GameObject
    {
        public bool running = true;

        // variables used by this class
        private int m_BackgroundPosition = 0;
        private int m_HighScore = 0;
        private Vector2 m_HighScorePosition = new Vector2(460, 220);
        private int m_SelectedOption = 0;

        // lists of class instances
        private List<UISprite> m_UISpritesInGame = new List<UISprite>();
        private List<Audio> m_AudioInGame = new List<Audio>();
        private List<CoolText> m_CoolTextInGame = new List<CoolText>();

        // constructor (code runs when going to the menu)
        public Menu()
        {
            // start music
            GAME_ENGINE.SetVolume(0.2f);

            // read high score from disc
            m_HighScore = GetScore();

            // setup gachground sprite
            UISprite background = new UISprite();
            background.spritemap = new SpriteMap("GradiusSprites/background.png", new Vector2(GAME_ENGINE.GetScreenWidth(), GAME_ENGINE.GetScreenHeight()));
            background.position = new Vector2f(0, 0);
            m_UISpritesInGame.Add(background);

            // setup title sprite
            UISprite hudElement = new UISprite();
            hudElement.spritemap = new SpriteMap("GradiusSprites/hud_title.png", new Vector2(242, 48));
            hudElement.position = new Vector2f(GAME_ENGINE.GetScreenWidth() / 2 - hudElement.spritemap.spritesize.X, 100);
            m_UISpritesInGame.Add(hudElement);

            // setup title sprite
            UISprite devlogo = new UISprite();
            devlogo.spritemap = new SpriteMap("GradiusSprites/hud_devlogo.png", new Vector2(124, 19));
            devlogo.position = new Vector2f(GAME_ENGINE.GetScreenWidth() / 2 - devlogo.spritemap.spritesize.X, 460);
            m_UISpritesInGame.Add(devlogo);

            // setup resistor score sprite
            UISprite highScoreBackPlate = new UISprite();
            highScoreBackPlate.position = new Vector2f(m_HighScorePosition.X - 6, m_HighScorePosition.Y - 4);
            highScoreBackPlate.spritemap = new SpriteMap("GradiusSprites/resistor_backplate.png", new Vector2(41, 8));
            highScoreBackPlate.spritemap.mapLocation = new Vector2(0, 0);
            m_UISpritesInGame.Add(highScoreBackPlate);

            // init cooltext
            CoolText cooltext = new CoolText(new Vector2f(250, 500), "sjoerd wouters 2021");
            m_CoolTextInGame.Add(cooltext);
            CoolText cooltext2 = new CoolText(new Vector2f(m_HighScorePosition.X - 190, m_HighScorePosition.Y - 4), "high score:");
            m_CoolTextInGame.Add(cooltext2);
        }

        // main loop
        public override void Paint()
        {
            if (running)
            {
                // draw all ui sprites
                for (int i = 0; i < m_UISpritesInGame.Count; i++)
                {
                    if (m_UISpritesInGame[i].spritemap.spritesize.X == GAME_ENGINE.GetScreenWidth()) // background sprite exeption
                    {
                        m_UISpritesInGame[i].spritemap.Draw(new Vector2f(-m_BackgroundPosition, 0));
                    }
                    else
                    {
                        m_UISpritesInGame[i].spritemap.Draw(m_UISpritesInGame[i].position); // all others
                    }
                }

                // draw all cooltext
                for (int i = 0; i < m_CoolTextInGame.Count; i++)
                {
                    m_CoolTextInGame[i].DrawCoolText();
                }

                // draw high score color code
                GAME_ENGINE.SetColor(255, 255, 255);
                Vector2 highScoreColorCodePosition = m_HighScorePosition;
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0 && m_HighScore.ToString().Length > 0)
                    {
                        int firstdigit = Convert.ToInt32(m_HighScore.ToString().Substring(0, 1));

                        if (firstdigit == 0)
                            GAME_ENGINE.SetColor(0, 0, 0);
                        else if (firstdigit == 1)
                            GAME_ENGINE.SetColor(111, 47, 0);
                        else if (firstdigit == 2)
                            GAME_ENGINE.SetColor(255, 0, 0);
                        else if (firstdigit == 3)
                            GAME_ENGINE.SetColor(255, 120, 0);
                        else if (firstdigit == 4)
                            GAME_ENGINE.SetColor(255, 255, 0);
                        else if (firstdigit == 5)
                            GAME_ENGINE.SetColor(0, 255, 0);
                        else if (firstdigit == 6)
                            GAME_ENGINE.SetColor(0, 0, 255);
                        else if (firstdigit == 7)
                            GAME_ENGINE.SetColor(120, 0, 255);
                        else if (firstdigit == 8)
                            GAME_ENGINE.SetColor(120, 120, 120);
                        else if (firstdigit == 9)
                            GAME_ENGINE.SetColor(255, 255, 255);
                    }
                    if (i == 1 && m_HighScore.ToString().Length > 1)
                    {
                        int seconddigit = Convert.ToInt32(m_HighScore.ToString().Substring(1, 1));

                        if (seconddigit == 0)
                            GAME_ENGINE.SetColor(0, 0, 0);
                        else if (seconddigit == 1)
                            GAME_ENGINE.SetColor(111, 47, 0);
                        else if (seconddigit == 2)
                            GAME_ENGINE.SetColor(255, 0, 0);
                        else if (seconddigit == 3)
                            GAME_ENGINE.SetColor(255, 120, 0);
                        else if (seconddigit == 4)
                            GAME_ENGINE.SetColor(255, 255, 0);
                        else if (seconddigit == 5)
                            GAME_ENGINE.SetColor(0, 255, 0);
                        else if (seconddigit == 6)
                            GAME_ENGINE.SetColor(0, 0, 255);
                        else if (seconddigit == 7)
                            GAME_ENGINE.SetColor(120, 0, 255);
                        else if (seconddigit == 8)
                            GAME_ENGINE.SetColor(120, 120, 120);
                        else if (seconddigit == 9)
                            GAME_ENGINE.SetColor(255, 255, 255);
                    }
                    if (i == 2 && m_HighScore.ToString().Length > 2)
                    {
                        int thirddigit = Convert.ToInt32(m_HighScore.ToString().Substring(2, 1));

                        if (thirddigit == 0)
                            GAME_ENGINE.SetColor(0, 0, 0);
                        else if (thirddigit == 1)
                            GAME_ENGINE.SetColor(111, 47, 0);
                        else if (thirddigit == 2)
                            GAME_ENGINE.SetColor(255, 0, 0);
                        else if (thirddigit == 3)
                            GAME_ENGINE.SetColor(255, 120, 0);
                        else if (thirddigit == 4)
                            GAME_ENGINE.SetColor(255, 255, 0);
                        else if (thirddigit == 5)
                            GAME_ENGINE.SetColor(0, 255, 0);
                        else if (thirddigit == 6)
                            GAME_ENGINE.SetColor(0, 0, 255);
                        else if (thirddigit == 7)
                            GAME_ENGINE.SetColor(120, 0, 255);
                        else if (thirddigit == 8)
                            GAME_ENGINE.SetColor(120, 120, 120);
                        else if (thirddigit == 9)
                            GAME_ENGINE.SetColor(255, 255, 255);
                    }
                    if (i == 3 && m_HighScore.ToString().Length > 0)
                    {
                        int multiplier = m_HighScore.ToString().Length - 3;

                        if (multiplier == 0)
                            GAME_ENGINE.SetColor(0, 0, 0);
                        else if (multiplier == 1)
                            GAME_ENGINE.SetColor(111, 47, 0);
                        else if (multiplier == 2)
                            GAME_ENGINE.SetColor(255, 0, 0);
                        else if (multiplier == 3)
                            GAME_ENGINE.SetColor(255, 120, 0);
                        else if (multiplier == 4)
                            GAME_ENGINE.SetColor(255, 255, 0);
                        else if (multiplier == 5)
                            GAME_ENGINE.SetColor(0, 255, 0);
                        else if (multiplier == 6)
                            GAME_ENGINE.SetColor(0, 0, 255);
                        else if (multiplier == 7)
                            GAME_ENGINE.SetColor(120, 0, 255);
                        else if (multiplier == 8)
                            GAME_ENGINE.SetColor(120, 120, 120);
                        else if (multiplier == 9)
                            GAME_ENGINE.SetColor(255, 255, 255);
                    }
                    if (m_HighScore <= 0)
                    {
                        GAME_ENGINE.SetColor(0, 0, 0);
                    }

                    GAME_ENGINE.FillRectangle(highScoreColorCodePosition.X + i * 16 + i * 2, highScoreColorCodePosition.Y, 8, 3);
                }

                // move background position
                m_BackgroundPosition++;
                if (m_BackgroundPosition >= 800)
                    m_BackgroundPosition = 0;

                // apply selected option
                if (GAME_ENGINE.GetKeyDown(Key.Enter))
                {
                    // play sound
                    GAME_ENGINE.SetVolume(0.3f);
                    Audio audio = new Audio("Gradius Sound Effects/projectile.wav");
                    GAME_ENGINE.PlayAudio(audio);
                    m_AudioInGame.Add(audio);

                    if (m_SelectedOption == 0)
                    {
                        Kill();
                        Game game = new Game();
                    }
                    if (m_SelectedOption == 1)
                    {
                        string dir = AppDomain.CurrentDomain.BaseDirectory + @"\file.txt";
                        File.Delete(dir);
                        m_HighScore = GetScore();
                    }
                    if (m_SelectedOption == 2)
                    {
                        System.Diagnostics.Process.Start("https://sjoerdwouters.com/");
                    }
                    if (m_SelectedOption == 3)
                    {
                        System.Environment.Exit(1);
                    }
                }

                // select option
                int LastOption = 3;
                if (GAME_ENGINE.GetKeyDown(Key.Down) || GAME_ENGINE.GetKeyDown(Key.S))
                {
                    // play sound
                    GAME_ENGINE.SetVolume(0.3f);
                    Audio audio = new Audio("Gradius Sound Effects/Select.wav");
                    GAME_ENGINE.PlayAudio(audio);
                    m_AudioInGame.Add(audio);

                    if (m_SelectedOption < LastOption)
                    {
                        m_SelectedOption++;
                    }
                    else
                    {
                        m_SelectedOption = 0;
                    }
                }
                if (GAME_ENGINE.GetKeyDown(Key.Up) || GAME_ENGINE.GetKeyDown(Key.W))
                {
                    // play sound
                    GAME_ENGINE.SetVolume(0.3f);
                    Audio audio = new Audio("Gradius Sound Effects/Select.wav");
                    GAME_ENGINE.PlayAudio(audio);
                    m_AudioInGame.Add(audio);

                    if (m_SelectedOption > 0)
                    {
                        m_SelectedOption--;
                    }
                    else
                    {
                        m_SelectedOption = LastOption;
                    }
                }

                // draw options text
                for (int i = 0; i < LastOption + 1; i++)
                {
                    if (m_SelectedOption == i)
                    {
                        GAME_ENGINE.SetColor(255, 120, 120);
                    }
                    else
                    {
                        GAME_ENGINE.SetColor(255, 255, 255);
                    }

                    var position = new Vector2(290, 280 + i * 32);
                    string text = "";

                    if (i == 0)
                    {
                        text = "Start New Game";
                    }
                    if (i == 1)
                    {
                        text = "Reset High Score";
                    }
                    if (i == 2)
                    {
                        text = "My Website";
                    }
                    if (i == 3)
                    {
                        text = "Exit And Save";
                    }
                    
                    GAME_ENGINE.DrawString(text, position.X, position.Y, 200, 200);
                }
            }
        }

        // reads high score from disc
        public int GetScore()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + @"\file.txt"; 
            if (File.Exists(dir))
            {
                return int.Parse(System.IO.File.ReadAllText(dir));
            }
            else
            {
                return 0;
            }
        }

        // kills scene
        public void Kill()
        {
            running = false;

            // clear all class instances
            m_UISpritesInGame.Clear();
            m_CoolTextInGame.Clear();

            // stop all audio
            for (int i = 0; i < m_AudioInGame.Count; i++)
            {
                GAME_ENGINE.StopAudio(m_AudioInGame[i]);
            }

            this.Dispose();
        }
    }
}
