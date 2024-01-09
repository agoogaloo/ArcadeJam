﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ArcadeJam;

public class Assets {
    public static Rectangle rippleSize = new (0,0,5,14), explosionSize = new(0,0,27,27);
    public static Texture2D player, PlayerBullet, PlayerSmearBullet, hook, chain,
        enemyStun, enemyBullet,crabArm,crabHinge, crown, crabBody, crabEnter,
        borders, comboCounter, comboBar, comboBarBorder, lives, introText, rippleL, rippleR, explosion;
    public static Texture2D pixel = null;

    public static Texture2D[] introChest, enemy, enemy2,
        shipBoss, angryCrabBody,crabBodyDamage, crabBodyGrapple, clawL,clawLOpen, clawR,clawROpen, gunCrown;

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
        rippleL = manager.Load<Texture2D>("rippleL");
        rippleR = manager.Load<Texture2D>("rippleR");
        explosion = manager.Load<Texture2D>("explosion");
        //enemies
        introChest = loadEnemy("introChest", manager);
        enemy = loadEnemy("enemy", manager);
        enemy2 = loadEnemy("iceWall", manager);
        //bosses
        shipBoss = loadEnemy("shipBoss", manager);
        
        angryCrabBody = loadEnemy("angryCrabBody", manager);
        crabArm = manager.Load<Texture2D>("enemies/crabArm");
        crabHinge = manager.Load<Texture2D>("enemies/clawHinge");
        crabBody = manager.Load<Texture2D>("enemies/crabBody");
        crabEnter = manager.Load<Texture2D>("enemies/crabEnter");
        clawL = loadEnemy("crabClawL", manager);
        clawR = loadEnemy("crabClawR", manager);
        clawLOpen = loadEnemy("crabClawLOpen", manager);
        clawROpen = loadEnemy("crabClawROpen", manager);
        gunCrown = loadEnemy("gunCrown",manager);
        crown = manager.Load<Texture2D>("enemies/crown");

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
