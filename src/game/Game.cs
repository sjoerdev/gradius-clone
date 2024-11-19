using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;

using Project;

namespace Project
{
    // declare enums
    public enum lifeType
    {
        gunner = 0,
        sharpSchooter,
        player,
    };
    public enum projectyleType
    {
        playerBullet = 0,
        playerMissile,
        playerLaser,
        enemyBullet,
        enemyLaser,
    };
    public enum pickUpType
    {
        speedPowerup = 0,
        missilePowerup,
        doublePowerup,
        laserPowerup,
    };

    public class Game : GameObject
    {
        public bool running = true;

        // variables used by the game
        private float m_ScoreMultiplier = 1;
        private int m_Score = 0;
        private int m_FrameCount;
        private float m_TimePast = 0;
        private int m_BackGroundPos = 0;
        private Vector2 m_PlayerVector = new Vector2(1, 1);
        private int m_PlayerAnimateStartFrame = 0;
        private List<EnemyToSpawn> m_EnemySpawner = new List<EnemyToSpawn>();
        private List<PickUpToSpawn> m_PickUpSpawner = new List<PickUpToSpawn>();
        private bool[] m_PowerUpActive = new bool[4];
        private float[] m_PowerUpMomentOfActivation = new float[4];
        private Life m_Player;

        // all gameobjects instanced by this class
        private List<Projectile> m_ProjectilesInGame = new List<Projectile>();
        private List<Life> m_LifeInGame = new List<Life>();
        private List<UISprite> m_PowerupSpritesInGame = new List<UISprite>();
        private UISprite m_HealthBarSpritesheet = new UISprite();
        private UISprite m_ResistorScoreSpritesheet = new UISprite();
        private List<PickUp> m_PickUpInGame = new List<PickUp>();
        private Bitmap m_Background;
        private Bitmap m_HudLit;
        private Bitmap m_HudUnLit;
        private List<AudioClip> m_AudioInGame = new List<AudioClip>();
        private List<CoolText> m_CoolTextInGame = new List<CoolText>();
        private CoolText m_HealthCoolText;
        
        // structs holding infomation for spawners
        struct EnemyToSpawn
        {
            public lifeType type;
            public Vector2 position;
            public float timer;
            public Vector2 offsetFromPlayer;
        };
        struct PickUpToSpawn
        {
            public pickUpType type;
            public Vector2 position;
            public float timer;
        };

        // Game Functions
        #region

        // schoots bullet
        void SchootBullet(Vector2 startPosition, Vector2 direction, projectyleType type)
        {
            // play bullet sound
            AudioClip audio = new AudioClip("Gradius Sound Effects/schoot.wav");
            Engine.PlayAudio(audio);
            m_AudioInGame.Add(audio);

            // init bullet and its componements
            Projectile bullet = new Projectile(type);
            bullet.position = startPosition;
            bullet.direction = direction;
            m_ProjectilesInGame.Add(bullet);
        }

        // checks if position is on screen or not
        bool IsPositionOnScreen(Vector2 position)
        {
            if (position.X > 0 && position.X < Engine.windowWidth && position.Y > 0 && position.Y < Engine.windowHeight)
                return true;
            else
                return false;
        }

        // animates the player
        void AnimatePlayer()
        {
            if (Engine.input.GetKeyDown(Key.S) || Engine.input.GetKeyDown(Key.Down))
            {
                m_PlayerAnimateStartFrame = m_FrameCount;
            }
            else if (Engine.input.GetKeyDown(Key.W) || Engine.input.GetKeyDown(Key.Up))
            {
                m_PlayerAnimateStartFrame = m_FrameCount;
            }

            if (Engine.input.GetKey(Key.S) || Engine.input.GetKey(Key.Down))
            {
                if (m_FrameCount == m_PlayerAnimateStartFrame)
                    m_Player.spritemap.SetSprite(new Vector2(3, 0));
                if (m_FrameCount >= m_PlayerAnimateStartFrame + 20)
                    m_Player.spritemap.SetSprite(new Vector2(4, 0));

            }
            else if (Engine.input.GetKey(Key.W) || Engine.input.GetKey(Key.Up))
            {
                if (m_FrameCount == m_PlayerAnimateStartFrame)
                    m_Player.spritemap.SetSprite(new Vector2(1, 0));
                if (m_FrameCount >= m_PlayerAnimateStartFrame + 20)
                    m_Player.spritemap.SetSprite(new Vector2(2, 0));
            }
            else
            {
                m_Player.spritemap.SetSprite(new Vector2(0, 0));
            }
        }

        // checks distance between 2 coords
        float Distance(Vector2 firstVector, Vector2 secondVector)
        {
            Vector2 deltaPos = new Vector2(secondVector.X - firstVector.X, secondVector.Y - firstVector.Y);
            float distance = (float)Math.Sqrt((Double)(deltaPos.X * deltaPos.X + deltaPos.Y * deltaPos.Y));
            return distance;
        }

        // checks for collisions between projectiles and life
        void CheckProjectileCollisions(float distanceToCollide)
        {
            for (int i = 0; i < m_ProjectilesInGame.Count; i++)
            {
                for (int j = 0; j < m_LifeInGame.Count; j++)
                {
                    var projectile = m_ProjectilesInGame[i];
                    var life = m_LifeInGame[j];

                    if (projectile.isEnemyProjectile && !life.isEnemyLife) // enemy bullets to player
                    {
                        if (Distance(projectile.position, life.position + new Vector2(life.spritemap.spritesize.X / 2, life.spritemap.spritesize.Y / 2)) <= distanceToCollide) // collision
                        {
                            // take damage
                            life.health -= projectile.damage;

                            // check health
                            if (life.health <= 0)
                            {
                                // destroy life
                                m_LifeInGame.Remove(life);

                                AudioClip audio = new AudioClip("Gradius Sound Effects/Explosion.wav");
                                Engine.PlayAudio(audio);
                                m_AudioInGame.Add(audio);

                                // save score to file
                                if (m_Score > GetScore())
                                {
                                    SaveScore();
                                }

                                // go to menu
                                Kill();
                                new Menu();
                            }

                            // destroy projectile
                            m_ProjectilesInGame.Remove(projectile);
                            AudioClip audio2 = new AudioClip("Gradius Sound Effects/Hit.wav");
                            Engine.PlayAudio(audio2);
                            m_AudioInGame.Add(audio2);

                            return;
                        }
                    }
                    else if (!projectile.isEnemyProjectile && life.isEnemyLife) // player bullets to enemies
                    {
                        if (Distance(projectile.position, life.position + new Vector2(life.spritemap.spritesize.X / 2, life.spritemap.spritesize.Y / 2)) <= distanceToCollide) // collision
                        {
                            // take damage
                            life.health -= projectile.damage;

                            // check health
                            if (life.health <= 0)
                            {
                                // destroy life
                                m_LifeInGame.Remove(life);
                                AudioClip audio = new AudioClip("Gradius Sound Effects/Explosion.wav");
                                Engine.PlayAudio(audio);
                                m_AudioInGame.Add(audio);
                                m_Score += (int)(100 * m_ScoreMultiplier);
                            }

                            // destroy projectile
                            m_ProjectilesInGame.Remove(projectile);
                            AudioClip audio2 = new AudioClip("Gradius Sound Effects/Hit.wav");
                            Engine.PlayAudio(audio2);
                            m_AudioInGame.Add(audio2);

                            return;
                        }
                    }
                }
            }
        }

        // adds an enemy to spawn to the enemy spawner
        void SpawnEnemy(float time, float verticalStartPosition, lifeType type, Vector2 offsetFromPlayer)
        {
            EnemyToSpawn test = new EnemyToSpawn();
            test.timer = time;
            test.position = new Vector2(Engine.windowWidth, verticalStartPosition);
            test.type = type;
            test.offsetFromPlayer = offsetFromPlayer;
            m_EnemySpawner.Add(test);
        }

        // adds an pickup to spawn to the pickup spawner
        void SpawnPickUp(float time, float verticalStartPosition, pickUpType type)
        {
            PickUpToSpawn test = new PickUpToSpawn();
            test.timer = time;
            test.position = new Vector2(Engine.windowHeight, verticalStartPosition);
            test.type = type;
            m_PickUpSpawner.Add(test);
        }

        // moves the player
        void MovePlayer()
        {
            float maxSpeed = 200 * m_Player.speedMultiplier;
            Vector2 acceleration = new Vector2(4 * m_Player.speedMultiplier, 4 * m_Player.speedMultiplier);
            Vector2 drag = new Vector2(0.991f, 0.991f);

            bool w = Engine.input.GetKey(Key.W) || Engine.input.GetKey(Key.Up);
            bool a = Engine.input.GetKey(Key.A) || Engine.input.GetKey(Key.Left);
            bool s = Engine.input.GetKey(Key.S) || Engine.input.GetKey(Key.Down);
            bool d = Engine.input.GetKey(Key.D) || Engine.input.GetKey(Key.Right);

            m_PlayerVector.X *= drag.X;
            m_PlayerVector.Y *= drag.Y;

            if (s)
            {
                if (m_PlayerVector.Y < maxSpeed && m_PlayerVector.Y > -maxSpeed)
                    m_PlayerVector.Y += acceleration.Y;
            }
            if (w)
            {
                if (m_PlayerVector.Y < maxSpeed && m_PlayerVector.Y > -maxSpeed)
                    m_PlayerVector.Y -= acceleration.Y;
            }
            if (a)
            {
                if (m_PlayerVector.X < maxSpeed && m_PlayerVector.X > -maxSpeed)
                    m_PlayerVector.X -= acceleration.X;
            }
            if (d)
            {
                if (m_PlayerVector.X < maxSpeed && m_PlayerVector.X > -maxSpeed)
                    m_PlayerVector.X += acceleration.X;
            }

            if (m_Player.position.X < 0)
            {
                m_Player.position.X = 0;
                m_PlayerVector.X = 0;
            }
            if (m_Player.position.X > Engine.windowWidth - 32 * 2)
            {
                m_Player.position.X = Engine.windowWidth - 32 * 2;
                m_PlayerVector.X = 0;
            }
            if (m_Player.position.Y < 0)
            {
                m_Player.position.Y = 0;
                m_PlayerVector.Y = 0;
            }
            if (m_Player.position.Y > Engine.windowHeight - 16 * 2)
            {
                m_Player.position.Y = Engine.windowHeight - 16 * 2;
                m_PlayerVector.Y = 0;
            }

            m_Player.position += new Vector2(m_PlayerVector.X * Engine.deltaTime, m_PlayerVector.Y * Engine.deltaTime);
        }

        // checks if the player has picked up a pickup
        void CheckPickUpCollisions(float distanceToCollide)
        {
            for (int i = 0; i < m_PickUpInGame.Count; i++)
            {
                for (int j = 0; j < m_LifeInGame.Count; j++)
                {
                    var pickUp = m_PickUpInGame[i];
                    var life = m_LifeInGame[j];

                    if (Distance(pickUp.position, life.position + new Vector2(life.spritemap.spritesize.X / 2, life.spritemap.spritesize.Y / 2)) <= distanceToCollide) // collision
                    {
                        // if powerup
                        if (pickUp.pickUpType == pickUpType.speedPowerup)
                        {
                            // destroy notlife
                            m_PickUpInGame.Remove(pickUp);
                            pickUp.Destroy();
                            m_PowerUpActive[0] = true;
                            m_PowerUpMomentOfActivation[0] = m_TimePast;

                            AudioClip audio2 = new AudioClip("Gradius Sound Effects/Pickup.wav");
                            Engine.PlayAudio(audio2);
                            m_AudioInGame.Add(audio2);
                        }
                        // if powerup
                        if (pickUp.pickUpType == pickUpType.missilePowerup)
                        {
                            // destroy notlife
                            m_PickUpInGame.Remove(pickUp);
                            pickUp.Destroy();
                            m_PowerUpActive[1] = true;
                            m_PowerUpMomentOfActivation[1] = m_TimePast;
                            AudioClip audio2 = new AudioClip("Gradius Sound Effects/Pickup.wav");
                            Engine.PlayAudio(audio2);
                            m_AudioInGame.Add(audio2);
                        }
                        // if powerup
                        if (pickUp.pickUpType == pickUpType.doublePowerup)
                        {
                            // destroy notlife
                            m_PickUpInGame.Remove(pickUp);
                            pickUp.Destroy();
                            m_PowerUpActive[2] = true;
                            m_PowerUpMomentOfActivation[2] = m_TimePast;
                            AudioClip audio2 = new AudioClip("Gradius Sound Effects/Pickup.wav");
                            Engine.PlayAudio(audio2);
                            m_AudioInGame.Add(audio2);
                        }
                        // if powerup
                        if (pickUp.pickUpType == pickUpType.laserPowerup)
                        {
                            // destroy notlife
                            m_PickUpInGame.Remove(pickUp);
                            pickUp.Destroy();
                            m_PowerUpActive[3] = true;
                            m_PowerUpMomentOfActivation[3] = m_TimePast;
                            AudioClip audio2 = new AudioClip("Gradius Sound Effects/Pickup.wav");
                            Engine.PlayAudio(audio2);
                            m_AudioInGame.Add(audio2);
                        }

                        return;
                    }
                }
            }
        }

        // initializes the player
        void InitPlayer()
        {
            m_Player = new Life(lifeType.player);
            m_Player.position = new Vector2(100, 300);
            m_LifeInGame.Add(m_Player);
        }

        // initializes all ui sprites
        void InitUISprites()
        {
            // init some loose bitmaps
            m_Background = new Bitmap("GradiusSprites/background.png");
            m_HudLit = new Bitmap("GradiusSprites/hud_lit.png");
            m_HudUnLit = new Bitmap("GradiusSprites/hud_unlit.png");

            // initialize powerup sprites
            for (int i = 0; i < 4; i++)
            {
                UISprite hudElement = new UISprite();
                hudElement.position = new Vector2(i * 67 + 6, 578);
                hudElement.spritemap = new SpriteMap("GradiusSprites/hud_unlit.png", new Vector2(32, 8));
                hudElement.spritemap.mapLocation = new Vector2(i, 0);
                m_PowerupSpritesInGame.Add(hudElement);
            }

            // setup healthbar sprite
            m_HealthBarSpritesheet = new UISprite();
            m_HealthBarSpritesheet.position = new Vector2(582, 578);
            m_HealthBarSpritesheet.spritemap = new SpriteMap("GradiusSprites/health_bar_spritesheet.png", new Vector2(106, 8));
            m_HealthBarSpritesheet.spritemap.mapLocation = new Vector2(0, 0);

            // setup resistor score sprite
            m_ResistorScoreSpritesheet = new UISprite();
            m_ResistorScoreSpritesheet.position = new Vector2(496, 578);
            m_ResistorScoreSpritesheet.spritemap = new SpriteMap("GradiusSprites/resistor_backplate.png", new Vector2(41, 8));
            m_ResistorScoreSpritesheet.spritemap.mapLocation = new Vector2(0, 0);

            // init health cooltext
            m_HealthCoolText = new CoolText(new Vector2(658, 576), "500");
            m_CoolTextInGame.Add(m_HealthCoolText);
        }

        // moves the background sprite
        void UpdateScrollingBackground()
        {
            m_BackGroundPos++;

            if (m_BackGroundPos >= m_Background.Width)
                m_BackGroundPos = 0;
            Engine.DrawBitmap(m_Background, new Vector2(-m_BackGroundPos, 0));
        }

        // updates the ui sprites to display the correct infomation
        void UpdateUISprites()
        {
            Engine.SetColor(255, 255, 255);
            for (int i = 0; i < m_PowerupSpritesInGame.Count; i++)
            {
                if (m_PowerUpActive[i])
                    m_PowerupSpritesInGame[i].spritemap.spritemap = m_HudLit;
                else
                    m_PowerupSpritesInGame[i].spritemap.spritemap = m_HudUnLit;
                m_PowerupSpritesInGame[i].spritemap.Draw(m_PowerupSpritesInGame[i].position);
            }
            m_HealthBarSpritesheet.spritemap.Draw(m_HealthBarSpritesheet.position);
            m_HealthBarSpritesheet.spritemap.mapLocation = new Vector2((int)Math.Floor((float)(500 - m_Player.health) / 500 * 20), 0);
            m_ResistorScoreSpritesheet.spritemap.Draw(m_ResistorScoreSpritesheet.position);
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 && m_Score.ToString().Length > 0)
                {
                    int firstdigit = Convert.ToInt32(m_Score.ToString().Substring(0, 1));

                    if (firstdigit == 0)
                        Engine.SetColor(0, 0, 0);
                    else if (firstdigit == 1)
                        Engine.SetColor(111, 47, 0);
                    else if (firstdigit == 2)
                        Engine.SetColor(255, 0, 0);
                    else if (firstdigit == 3)
                        Engine.SetColor(255, 120, 0);
                    else if (firstdigit == 4)
                        Engine.SetColor(255, 255, 0);
                    else if (firstdigit == 5)
                        Engine.SetColor(0, 255, 0);
                    else if (firstdigit == 6)
                        Engine.SetColor(0, 0, 255);
                    else if (firstdigit == 7)
                        Engine.SetColor(120, 0, 255);
                    else if (firstdigit == 8)
                        Engine.SetColor(120, 120, 120);
                    else if (firstdigit == 9)
                        Engine.SetColor(255, 255, 255);
                }
                if (i == 1 && m_Score.ToString().Length > 1)
                {
                    int seconddigit = Convert.ToInt32(m_Score.ToString().Substring(1, 1));

                    if (seconddigit == 0)
                        Engine.SetColor(0, 0, 0);
                    else if (seconddigit == 1)
                        Engine.SetColor(111, 47, 0);
                    else if (seconddigit == 2)
                        Engine.SetColor(255, 0, 0);
                    else if (seconddigit == 3)
                        Engine.SetColor(255, 120, 0);
                    else if (seconddigit == 4)
                        Engine.SetColor(255, 255, 0);
                    else if (seconddigit == 5)
                        Engine.SetColor(0, 255, 0);
                    else if (seconddigit == 6)
                        Engine.SetColor(0, 0, 255);
                    else if (seconddigit == 7)
                        Engine.SetColor(120, 0, 255);
                    else if (seconddigit == 8)
                        Engine.SetColor(120, 120, 120);
                    else if (seconddigit == 9)
                        Engine.SetColor(255, 255, 255);
                }
                if (i == 2 && m_Score.ToString().Length > 2)
                {
                    int thirddigit = Convert.ToInt32(m_Score.ToString().Substring(2, 1));

                    if (thirddigit == 0)
                        Engine.SetColor(0, 0, 0);
                    else if (thirddigit == 1)
                        Engine.SetColor(111, 47, 0);
                    else if (thirddigit == 2)
                        Engine.SetColor(255, 0, 0);
                    else if (thirddigit == 3)
                        Engine.SetColor(255, 120, 0);
                    else if (thirddigit == 4)
                        Engine.SetColor(255, 255, 0);
                    else if (thirddigit == 5)
                        Engine.SetColor(0, 255, 0);
                    else if (thirddigit == 6)
                        Engine.SetColor(0, 0, 255);
                    else if (thirddigit == 7)
                        Engine.SetColor(120, 0, 255);
                    else if (thirddigit == 8)
                        Engine.SetColor(120, 120, 120);
                    else if (thirddigit == 9)
                        Engine.SetColor(255, 255, 255);
                }
                if (i == 3 && m_Score.ToString().Length > 0)
                {
                    int multiplier = m_Score.ToString().Length - 3;

                    if (multiplier == 0)
                        Engine.SetColor(0, 0, 0);
                    else if (multiplier == 1)
                        Engine.SetColor(111, 47, 0);
                    else if (multiplier == 2)
                        Engine.SetColor(255, 0, 0);
                    else if (multiplier == 3)
                        Engine.SetColor(255, 120, 0);
                    else if (multiplier == 4)
                        Engine.SetColor(255, 255, 0);
                    else if (multiplier == 5)
                        Engine.SetColor(0, 255, 0);
                    else if (multiplier == 6)
                        Engine.SetColor(0, 0, 255);
                    else if (multiplier == 7)
                        Engine.SetColor(120, 0, 255);
                    else if (multiplier == 8)
                        Engine.SetColor(120, 120, 120);
                    else if (multiplier == 9)
                        Engine.SetColor(255, 255, 255);
                }
                if (m_Score <= 0)
                {
                    Engine.SetColor(0, 0, 0);
                }

                Engine.FillRectangle(502 + i * 16 + i * 2, 582, 8, 3);
            }
            m_HealthCoolText.text = m_Player.health.ToString();
            // draw all cooltext
            for (int i = 0; i < m_CoolTextInGame.Count; i++)
            {
                m_CoolTextInGame[i].DrawCoolText();
            }
        }

        // moves and animates all projectiles on screen
        void UpdateProjectiles()
        {
            for (int i = 0; i < m_ProjectilesInGame.Count; i++)
            {
                var bullet = m_ProjectilesInGame[i];
                bullet.position += new Vector2(bullet.direction.X * bullet.speed, bullet.direction.Y * bullet.speed);
                bullet.spritemap.Draw(bullet.position);

                if (!IsPositionOnScreen(bullet.position))
                {
                    m_ProjectilesInGame.RemoveAt(i);
                }
            }
        }

        // checks if the enemy spawner should spawn an enemy, and does so if needed
        void UpdateEnemySpawner()
        {
            for (int i = 0; i < m_EnemySpawner.Count; i++)
            {
                var EnemyToSpawn = m_EnemySpawner[i];

                if (EnemyToSpawn.timer < m_FrameCount)
                {
                    var temp = new Life(EnemyToSpawn.type);
                    temp.position = EnemyToSpawn.position;
                    temp.startPos = EnemyToSpawn.position;
                    temp.offsetFromPlayer = EnemyToSpawn.offsetFromPlayer;
                    m_LifeInGame.Add(temp);
                    m_EnemySpawner.Remove(EnemyToSpawn);
                }
            }
        }

        // checks if the pickup spawner should spawn an pickup, and does so if needed
        void UpdatePickUpSpawner()
        {
            for (int i = 0; i < m_PickUpSpawner.Count; i++)
            {
                var pickUpToSpawn = m_PickUpSpawner[i];

                if (pickUpToSpawn.timer < m_FrameCount)
                {
                    PickUp powerup = new PickUp(pickUpToSpawn.type);
                    powerup.position = pickUpToSpawn.position;
                    m_PickUpInGame.Add(powerup);
                    m_PickUpSpawner.Remove(pickUpToSpawn);
                }
            }
        }

        // moves and animates all life (enemies and players)
        void UpdateLife()
        {
            for (int i = 0; i < m_LifeInGame.Count; i++)
            {
                var currentLife = m_LifeInGame[i];
                currentLife.spritemap.Draw(currentLife.position);

                // animate and move each life type
                if (currentLife.lifeType == lifeType.gunner)
                {
                    if (currentLife.swingDirUp)
                    {
                        if (currentLife.verticalSwingPos < 1)
                            currentLife.verticalSwingPos += 0.005f;
                        else
                            currentLife.swingDirUp = false;
                    }
                    else
                    {
                        if (currentLife.verticalSwingPos > -1)
                            currentLife.verticalSwingPos -= 0.005f;
                        else
                            currentLife.swingDirUp = true;
                    }

                    float swingpos = currentLife.verticalSwingPos;

                    // move
                    currentLife.position.X -= 0.2f;
                    if (currentLife.swingDirUp)
                        currentLife.position.Y += (1 - Math.Abs(swingpos));
                    else
                        currentLife.position.Y -= (1 - Math.Abs(swingpos));

                    // animate
                    if ((swingpos > 0.9f && currentLife.swingDirUp) || (swingpos < -0.9f && !currentLife.swingDirUp))
                    {
                        currentLife.StartAnimateFrame = m_FrameCount;
                        currentLife.spritemap.mapLocation.X = 4;
                    }
                    else if (currentLife.swingDirUp)
                    {
                        float linswingpos = (swingpos + 1) / 2;
                        if (linswingpos < 1.0f)
                            currentLife.spritemap.mapLocation.X = 4;
                        if (linswingpos < 0.9f)
                            currentLife.spritemap.mapLocation.X = 5;
                        if (linswingpos < 0.8f)
                            currentLife.spritemap.mapLocation.X = 6;
                        if (linswingpos < 0.7f)
                            currentLife.spritemap.mapLocation.X = 7;
                        if (linswingpos < 0.6f)
                            currentLife.spritemap.mapLocation.X = 8;
                        if (linswingpos < 0.5f)
                            currentLife.spritemap.mapLocation.X = 7;
                        if (linswingpos < 0.4f)
                            currentLife.spritemap.mapLocation.X = 6;
                        if (linswingpos < 0.3f)
                            currentLife.spritemap.mapLocation.X = 5;
                        if (linswingpos < 0.2f)
                            currentLife.spritemap.mapLocation.X = 4;
                    }
                    else if (!currentLife.swingDirUp)
                    {
                        float linswingpos = (swingpos + 1) / 2;
                        if (linswingpos < 1.0f)
                            currentLife.spritemap.mapLocation.X = 4;
                        if (linswingpos < 0.9f)
                            currentLife.spritemap.mapLocation.X = 3;
                        if (linswingpos < 0.8f)
                            currentLife.spritemap.mapLocation.X = 2;
                        if (linswingpos < 0.7f)
                            currentLife.spritemap.mapLocation.X = 1;
                        if (linswingpos < 0.6f)
                            currentLife.spritemap.mapLocation.X = 0;
                        if (linswingpos < 0.5f)
                            currentLife.spritemap.mapLocation.X = 1;
                        if (linswingpos < 0.4f)
                            currentLife.spritemap.mapLocation.X = 2;
                        if (linswingpos < 0.3f)
                            currentLife.spritemap.mapLocation.X = 3;
                        if (linswingpos < 0.2f)
                            currentLife.spritemap.mapLocation.X = 4;
                    }

                    if (m_FrameCount % currentLife.shootSpeed == 4)
                    {
                        SchootBullet(currentLife.position + new Vector2(-30, 8), new Vector2(-1, 0), projectyleType.enemyLaser);
                    }
                }
                if (currentLife.lifeType == lifeType.sharpSchooter)
                {
                    Vector2 dir = (m_Player.position + currentLife.offsetFromPlayer) - currentLife.position;
                    currentLife.position += new Vector2(-0.2f, dir.Y / 200);

                    int animationSensitivity = 20;
                    if (dir.Y < animationSensitivity && dir.Y > -animationSensitivity)
                    {
                        currentLife.spritemap.mapLocation.X = 4;
                    }
                    if (dir.Y > 0)
                    {
                        if (dir.Y > animationSensitivity * 1)
                        {
                            currentLife.spritemap.mapLocation.X = 5;
                        }
                        if (dir.Y > animationSensitivity * 2)
                        {
                            currentLife.spritemap.mapLocation.X = 6;
                        }
                        if (dir.Y > animationSensitivity * 3)
                        {
                            currentLife.spritemap.mapLocation.X = 7;
                        }
                    }
                    if (dir.Y < 0)
                    {
                        if (dir.Y < animationSensitivity * -1)
                        {
                            currentLife.spritemap.mapLocation.X = 1;
                        }
                        if (dir.Y < animationSensitivity * -2)
                        {
                            currentLife.spritemap.mapLocation.X = 2;
                        }
                        if (dir.Y < animationSensitivity * -3)
                        {
                            currentLife.spritemap.mapLocation.X = 3;
                        }
                    }
                    

                    if (m_FrameCount % currentLife.shootSpeed == 1)
                    {
                        SchootBullet(currentLife.position + new Vector2(0, 8), new Vector2(-1, 0), projectyleType.enemyBullet);
                    }
                }
                if (currentLife.lifeType == lifeType.player)
                {
                    MovePlayer();
                    AnimatePlayer();

                    
                    if (Engine.input.GetKey(Key.Space))
                    {
                        if (m_FrameCount % 30 == 0)
                        {
                            SchootBullet(m_Player.position + new Vector2(32, 8), new Vector2(1, 0), m_Player.playerProjectile);
                        }
                    }
                }

                // keep life from despawning on top or bottom
                if (currentLife.position.Y < 0)
                {
                    currentLife.position.Y = 0;
                }
                if (currentLife.position.Y > Engine.windowHeight - 16 * 2)
                {
                    currentLife.position.Y = Engine.windowHeight - 16 * 2;
                }

                // dispose enemies no longer on screen
                if (!IsPositionOnScreen(currentLife.position) && currentLife.lifeType != lifeType.player)
                {
                    m_LifeInGame.RemoveAt(i);
                }
            }
        }

        // moves and animates all pickups
        void UpdatePickUp()
        {
            for (int i = 0; i < m_PickUpInGame.Count; i++)
            {
                var currentPickUp = m_PickUpInGame[i];
                currentPickUp.spritemap.Draw(currentPickUp.position);

                // move
                currentPickUp.position.X -= 0.2f;

                // keep pickup from despawning on top or bottom
                if (currentPickUp.position.Y < 0)
                {
                    currentPickUp.position.Y = 0;
                }
                if (currentPickUp.position.Y > Engine.windowHeight - 16 * 2)
                {
                    currentPickUp.position.Y = Engine.windowHeight - 16 * 2;
                }

                // dispose objects no longer on screen
                if (!IsPositionOnScreen(currentPickUp.position))
                {
                    m_PickUpInGame.RemoveAt(i);
                }
            }
        }

        // applys the effects of avtive powerups
        void ApplyActivePowerups()
        {
            m_Player.speedMultiplier = 1;
            m_Player.playerProjectile = projectyleType.playerBullet;
            m_ScoreMultiplier = 1;
            for (int i = 0; i < m_PowerUpActive.Count<bool>(); i++)
            {
                if (m_PowerUpActive[i] == true)
                {
                    if (i == 0) // speedup
                    {
                        if (m_PowerUpMomentOfActivation[i] + 5 <= m_TimePast)
                            m_PowerUpActive[i] = false;
                        else
                            m_Player.speedMultiplier = 3;
                    }
                    if (i == 1) // missile
                    {
                        if (m_PowerUpMomentOfActivation[i] + 5 <= m_TimePast)
                            m_PowerUpActive[i] = false;
                        else
                            m_Player.playerProjectile = projectyleType.playerMissile;
                    }
                    if (i == 2) // double
                    {
                        if (m_PowerUpMomentOfActivation[i] + 5 <= m_TimePast)
                            m_PowerUpActive[i] = false;
                        else
                            m_ScoreMultiplier = 2;
                    }
                    if (i == 3) // laser
                    {
                        if (m_PowerUpMomentOfActivation[i] + 5 <= m_TimePast)
                            m_PowerUpActive[i] = false;
                        else
                            m_Player.playerProjectile = projectyleType.playerLaser;
                    }
                }
            }
        }

        // this adds random enemys and pickups to the enemy and pickup spawners
        void SpawnRandomThings()
        {
            if (m_FrameCount % 420 == 0)
            {
                Random random = new Random();
                int ypos = random.Next(100, Engine.windowHeight - 100);
                int spawntype = random.Next(1, 5);

                if (spawntype == 1)
                {
                    SpawnEnemy(0, ypos, (lifeType)random.Next(0, 2), new Vector2(0, 0));
                }
                if (spawntype == 2)
                {
                    SpawnPickUp(0, ypos, (pickUpType)random.Next(0, 4));
                }
                if (spawntype == 3)
                {
                    SpawnEnemy(m_FrameCount + 10, ypos + 50, lifeType.sharpSchooter, new Vector2(0, 50));
                    SpawnEnemy(m_FrameCount + 20, ypos, lifeType.sharpSchooter, new Vector2(0, 0));
                    SpawnEnemy(m_FrameCount + 30, ypos - 50, lifeType.sharpSchooter, new Vector2(0, -50));
                }
                if (spawntype == 4)
                {
                    SpawnEnemy(m_FrameCount + 10, ypos + 50, lifeType.gunner, new Vector2(0, 0));
                    SpawnEnemy(m_FrameCount + 20, ypos, lifeType.gunner, new Vector2(0, 0));
                    SpawnEnemy(m_FrameCount + 30, ypos - 50, lifeType.gunner, new Vector2(0, 0));
                }
            }
        }

        // this keeps the time
        void KeepTime()
        {
            m_FrameCount++;
            m_TimePast += Engine.deltaTime;
        }

        // this writes the score to disc
        public void SaveScore()
        {
            // Save File to .txt  
            string dirParameter = AppDomain.CurrentDomain.BaseDirectory + @"\file.txt";
            FileStream fParameter = new FileStream(dirParameter, FileMode.Create, FileAccess.Write);
            StreamWriter m_WriterParameter = new StreamWriter(fParameter);
            m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
            m_WriterParameter.Write(m_Score.ToString());
            m_WriterParameter.Flush();
            m_WriterParameter.Close();
        }

        // this reads the score from disc
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

        // goes back to menu when pressing escape
        public void GoBackToMenuWhenEscape()
        {
            if (Engine.input.GetKeyDown(Key.Escape))
            {
                // save score to file
                if (m_Score > GetScore())
                {
                    SaveScore();
                }

                // go to menu
                Kill();
                new Menu();
            }
        }

        #endregion

        // this is the game constructor (code that runs when starting a new game)
        public Game()
        {
            InitPlayer();
            InitUISprites();
            
            // start music
            Engine.SetVolume(0.2f);
            AudioClip audio2 = new AudioClip("Gradius Sound Effects/Music.mp3");
            Engine.PlayAudio(audio2);
            m_AudioInGame.Add(audio2);
        }

        // main loop
        public override void Paint()
        {
            // if this game is still running, run all the functions of the game
            if (running)
            {
                KeepTime();
                UpdateScrollingBackground();
                UpdateUISprites();
                UpdateProjectiles();
                UpdateEnemySpawner();
                UpdatePickUpSpawner();
                UpdateLife();
                UpdatePickUp();
                ApplyActivePowerups();
                CheckProjectileCollisions(20);
                CheckPickUpCollisions(30);
                SpawnRandomThings();
                GoBackToMenuWhenEscape();
            }
        }

        // deletes all class instances and disables all running audio files
        public void Kill()
        {
            running = false;

            m_ProjectilesInGame.Clear();
            m_LifeInGame.Clear();
            m_PowerupSpritesInGame.Clear();
            m_HealthBarSpritesheet = null;
            m_ResistorScoreSpritesheet = null;
            m_PickUpInGame.Clear();
            m_Background = null;
            m_HudLit = null;
            m_HudUnLit = null;
            m_CoolTextInGame.Clear();

            for (int i = 0; i < m_AudioInGame.Count; i++)
            {
                Engine.StopAudio(m_AudioInGame[i]);
            }

            this.Destroy();
        }
    }
}
