using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Assets {
    public static Texture2D player, PlayerBullet, enemy, enemy2, enemyBullet, hook, chain, borders;
    public static Texture2D pixel = null;

    public static SpriteFont font;
    public static void Load(ContentManager manager, Texture2D whitePixel) {
        borders = manager.Load<Texture2D>("borders");

        pixel = whitePixel;
        player = manager.Load<Texture2D>("player");
        PlayerBullet = manager.Load<Texture2D>("cannonBall");
        enemy = manager.Load<Texture2D>("enemy");
        enemy2 = manager.Load<Texture2D>("iceWall");
        enemyBullet = manager.Load<Texture2D>("lightBullet");
        hook = manager.Load<Texture2D>("hook");
        chain = manager.Load<Texture2D>("chain");



        font = manager.Load<SpriteFont>("monoFont");



    }

}
