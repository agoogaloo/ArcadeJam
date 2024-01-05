using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Assets {
    public static Texture2D player, PlayerBullet, PlayerSmearBullet, enemy,enemyStun, enemy2, enemyBullet, hook, chain,
        borders, comboCounter, comboBar, comboBarBorder, lives;
    public static Texture2D pixel = null;

    public static SpriteFont font;
    public static void Load(ContentManager manager, Texture2D whitePixel) {
        borders = manager.Load<Texture2D>("borders");
        comboCounter = manager.Load<Texture2D>("comboCounter");
        comboBar = manager.Load<Texture2D>("comboBar");
        comboBarBorder = manager.Load<Texture2D>("comboBarBorder");
        lives = manager.Load<Texture2D>("lightBullet");

        pixel = whitePixel;
        player = manager.Load<Texture2D>("player");
        PlayerBullet = manager.Load<Texture2D>("cannonBall");
        PlayerSmearBullet = manager.Load<Texture2D>("playerSmearBullet");
        enemy = manager.Load<Texture2D>("enemy");
        enemyStun = manager.Load<Texture2D>("enemyBullet");
        enemy2 = manager.Load<Texture2D>("iceWall");
        enemyBullet = manager.Load<Texture2D>("lightBullet");
        hook = manager.Load<Texture2D>("hook");
        chain = manager.Load<Texture2D>("chain");



        font = manager.Load<SpriteFont>("monoFont");



    }

}
