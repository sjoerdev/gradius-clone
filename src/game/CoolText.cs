using System;
using System.Numerics;

namespace Spork;

public class CoolText : UISprite
{
    public string text;
    private string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789:";
    public CoolText(Vector2 pos, string str)
    {
        text = str;
        position = pos;
        spritemap = new SpriteMap("GradiusSprites/hud_cooltext.png", new Vector2(8, 8));
    }

    public void DrawCoolText()
    {
        base.Paint();

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsWhiteSpace(text[i]))
            {
                // nothing
            }
            else
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (text[i] == alphabet[j])
                    {
                        spritemap.mapLocation = new Vector2(j, 0);
                    }
                }

                spritemap.Draw(position + new Vector2(16 * i, 0));
            }
        }
    }

    public override void GameEnd()
    {
        // dispose
        Destroy();
    }
}
