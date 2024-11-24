using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spork;

public class Life : GameObject
{
    public Vector2 position;
    public SpriteMap spritemap;
    public int health;
    public lifeType lifeType;
    public bool isEnemyLife;
    public Vector2 startPos;
    public Vector2 offsetFromPlayer;
    public float verticalSwingPos = 0;
    public bool swingDirUp = true;
    public int shootSpeed;
    public float speedMultiplier = 1;
    public projectyleType playerProjectile;
    public int StartAnimateFrame;

    public Life(lifeType type)
    {
        GameInitialize();
        GameStart();
        InitType(type);
    }
    void InitType(lifeType type)
    {
        // init all types
        if (type == lifeType.player)
        {
            spritemap = new SpriteMap("GradiusSprites/vic_viper.png", new Vector2(32, 16));
            health = 500;
            lifeType = lifeType.player;
            playerProjectile = projectyleType.playerBullet;
            isEnemyLife = false;
        }
        if (type == lifeType.gunner)
        {
            spritemap = new SpriteMap("GradiusSprites/rotating_gunner.png", new Vector2(16, 16));
            health = 20;
            lifeType = lifeType.gunner;

            Random random = new Random();
            int value = random.Next(90, 120); // fires at irregular time interval (also has chance not to fire)
            shootSpeed = value;

            isEnemyLife = true;
        }
        if (type == lifeType.sharpSchooter)
        {
            spritemap = new SpriteMap("GradiusSprites/rotating_plane.png", new Vector2(16, 16));
            health = 30;
            lifeType = lifeType.sharpSchooter;

            Random random = new Random();
            int value = random.Next(90, 120); // fires at irregular time interval (also has chance not to fire)
            shootSpeed = value;

            isEnemyLife = true;
        }
    }
}
