using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Assets {
    public static Texture2D player, PlayerBullet, PlayerSmearBullet, hook, chain,
        enemy, enemyStun, enemy2, enemyBullet,
        shipBoss, crabBody, crabArm, clawL, clawR,
        borders, comboCounter, comboBar, comboBarBorder, lives;
    public static Texture2D pixel = null;

    public static SpriteFont font;
    public static void Load(ContentManager manager, Texture2D whitePixel) {
        pixel = whitePixel;
        //ui
        borders = manager.Load<Texture2D>("borders");
        comboCounter = manager.Load<Texture2D>("comboCounter");
        comboBar = manager.Load<Texture2D>("comboBar");
        comboBarBorder = manager.Load<Texture2D>("comboBarBorder");
        lives = manager.Load<Texture2D>("lightBullet");

        //player
        player = manager.Load<Texture2D>("player");
        PlayerBullet = manager.Load<Texture2D>("cannonBall");
        PlayerSmearBullet = manager.Load<Texture2D>("playerSmearBullet");
        enemyBullet = manager.Load<Texture2D>("lightBullet");
        hook = manager.Load<Texture2D>("hook");
        chain = manager.Load<Texture2D>("chain");
        //enemies
        enemy = manager.Load<Texture2D>("enemies/enemy");
        enemyStun = manager.Load<Texture2D>("enemyBullet");
        enemy2 = manager.Load<Texture2D>("iceWall");
        //bosses
        shipBoss = manager.Load<Texture2D>("enemies/shipBoss");
        crabBody = manager.Load<Texture2D>("enemies/crabBody");
        crabArm = manager.Load<Texture2D>("enemies/crabArm");
        clawL = manager.Load<Texture2D>("enemies/crabClawL");
        clawR = manager.Load<Texture2D>("enemies/crabClawR");



        font = manager.Load<SpriteFont>("monoFont");



    }

}
