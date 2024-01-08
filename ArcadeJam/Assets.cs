using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ArcadeJam;

public class Assets {
    public static Texture2D player, PlayerBullet, PlayerSmearBullet, hook, chain,
        enemyStun, enemyBullet,crabArm,
        borders, comboCounter, comboBar, comboBarBorder, lives, introText;
    public static Texture2D pixel = null, pain;

    public static Texture2D[] introChest, enemy, enemy2,
        shipBoss, crabBody,crabBodyDamage, crabBodyGrapple, clawL,clawLOpen, clawR,clawROpen;

    public static Song music;

    public static SpriteFont font;
    public static void Load(ContentManager manager, Texture2D whitePixel) {
        pixel = whitePixel;
        //ui
        borders = manager.Load<Texture2D>("borders");
        comboCounter = manager.Load<Texture2D>("comboCounter");
        comboBar = manager.Load<Texture2D>("comboBar");
        comboBarBorder = manager.Load<Texture2D>("comboBarBorder");
        lives = manager.Load<Texture2D>("lightBullet");
        introText = manager.Load<Texture2D>("introText");

        //player
        player = manager.Load<Texture2D>("player");
        PlayerBullet = manager.Load<Texture2D>("cannonBall");
        PlayerSmearBullet = manager.Load<Texture2D>("playerSmearBullet");
        enemyBullet = manager.Load<Texture2D>("lightBullet");
        hook = manager.Load<Texture2D>("hook");
        chain = manager.Load<Texture2D>("chain");
        //enemies
        introChest = loadEnemy("introChest", manager);
        enemy = loadEnemy("enemy", manager);
        enemy2 = loadEnemy("iceWall", manager);
        //bosses
        shipBoss = loadEnemy("shipBoss", manager);
        crabBody = loadEnemy("crabBody", manager);
        crabArm = manager.Load<Texture2D>("enemies/crabArm");
        clawL = loadEnemy("crabClawL", manager);
        clawR = loadEnemy("crabClawR", manager);
        clawLOpen = loadEnemy("crabClawLOpen", manager);
        clawROpen = loadEnemy("crabClawROpen", manager);
        pain = manager.Load<Texture2D>("enemies/crabClawLDamage");

        //sounds
        music = manager.Load<Song>("sounds/music");

        //fonts
        font = manager.Load<SpriteFont>("monoFont");

    }
    private static Texture2D[] loadEnemy(String name, ContentManager manager) {
        Console.WriteLine("loading enemy "+name);
        Texture2D[] textures = new Texture2D[3];
        textures[0] = manager.Load<Texture2D>("enemies/" + name);
        try {
            textures[1] = manager.Load<Texture2D>("enemies/" + name + "Damage");
        }
        catch {
            Console.WriteLine("couldnt load damage tex for " + name);
            textures[1] = textures[0];
        }

        try {
            textures[2] = manager.Load<Texture2D>("enemies/" + name + "Low");
        }
        catch {
            Console.WriteLine("couldnt load grapple tex for " + name);
            textures[2] = textures[0];
        }
        return textures;


    }

}
