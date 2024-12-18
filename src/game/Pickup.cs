﻿using System.Numerics;
using Spork;

namespace GradiusClone;

public class PickUp : GameObject 
{
    public Vector2 position;
    public SpriteMap spritemap;
    public pickUpType pickUpType;

    public PickUp(pickUpType type)
    {
        GameInitialize();
        GameStart();
        InitType(type);
    }
    public void InitType(pickUpType type)
    {
        if (type == pickUpType.doublePowerup)
        {
            spritemap = new SpriteMap("GradiusSprites/Powerup.png", new Vector2(16, 16));
            pickUpType = pickUpType.doublePowerup;
        }
        if (type == pickUpType.laserPowerup)
        {
            spritemap = new SpriteMap("GradiusSprites/Powerup.png", new Vector2(16, 16));
            pickUpType = pickUpType.laserPowerup;
        }
        if (type == pickUpType.missilePowerup)
        {
            spritemap = new SpriteMap("GradiusSprites/Powerup.png", new Vector2(16, 16));
            pickUpType = pickUpType.missilePowerup;
        }
        if (type == pickUpType.speedPowerup)
        {
            spritemap = new SpriteMap("GradiusSprites/Powerup.png", new Vector2(16, 16));
            pickUpType = pickUpType.speedPowerup;
        }
    }
}
