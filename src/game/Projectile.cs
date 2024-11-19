using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Project
{
    public class Projectile : GameObject
    {
        public SpriteMap spritemap;
        public Vector2 direction;
        public Vector2 position;
        public int damage;
        public int speed;
        public projectyleType projectyleType;
        public bool isEnemyProjectile;

        public Projectile(projectyleType type)
        {
            GameInitialize();
            GameStart();
            InitType(type);
        }
        void InitType(projectyleType type)
        {
            // init all types and componements
            if (type == projectyleType.playerBullet)
            {
                spritemap = new SpriteMap("GradiusSprites/default_bullet.png", new Vector2(11, 4));
                damage = 10;
                speed = 10;
                projectyleType = projectyleType.playerBullet;
                isEnemyProjectile = false;
            }
            if (type == projectyleType.playerMissile)
            {
                spritemap = new SpriteMap("GradiusSprites/missile.png", new Vector2(20, 5));
                damage = 20;
                speed = 3;
                projectyleType = projectyleType.playerMissile;
                isEnemyProjectile = false;
            }
            if (type == projectyleType.playerLaser)
            {
                spritemap = new SpriteMap("GradiusSprites/blue_lazer.png", new Vector2(32, 1));
                damage = 15;
                speed = 10;
                projectyleType = projectyleType.playerLaser;
                isEnemyProjectile = false;
            }
            if (type == projectyleType.enemyBullet)
            {
                spritemap = new SpriteMap("GradiusSprites/default_bullet.png", new Vector2(11, 4));
                damage = 5;
                speed = 10;
                projectyleType = projectyleType.enemyBullet;
                isEnemyProjectile = true;
            }
            if (type == projectyleType.enemyLaser)
            {
                spritemap = new SpriteMap("GradiusSprites/blue_lazer.png", new Vector2(32, 1));
                damage = 10;
                speed = 10;
                projectyleType = projectyleType.enemyLaser;
                isEnemyProjectile = true;
            }
        }
    }
}
