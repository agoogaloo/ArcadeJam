using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Assets {
    public static Texture2D player, icicle, enemy, enemyBullet;
    public static Texture2D pixel = null;

    public static SpriteFont font;
    public static void Load(ContentManager manager, Texture2D whitePixel) {
        pixel = whitePixel;
        player = manager.Load<Texture2D>("peng");
        icicle = manager.Load<Texture2D>("icicle");
        enemy = manager.Load<Texture2D>("enemy");
        enemyBullet = manager.Load<Texture2D>("enemyBullet");

        font = manager.Load<SpriteFont>("monoFont");

        

    }

}
