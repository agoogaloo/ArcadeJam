using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ArcadeJam;

public class Assets {
    public static Rectangle rippleSize = new(0, 0, 5, 14), explosionSize = new(0, 0, 27, 27);
    public static Texture2D player, PlayerBullet, PlayerSmearBullet, bigShot,hook, chain,
        enemyStun, enemyBullet, crabArm, crabHinge, crown, crabBody, crabEnter,
        borders, comboCounter, comboBar, comboBarBorder, bossBar, bossBarFrame, lives, introText,
         rippleL, rippleR, explosion, cursor;
    public static Texture2D pixel = null;

    public static Texture2D[] introChest, enemy, bombEnemy, mine, trippleEnemyR,
        shipBoss, angryCrabBody, crabBodyDamage, crabBodyGrapple, clawL, clawLOpen, clawR, clawROpen, gunCrown;

    public static Song music;
    public static SoundEffect bigExplosion, smallExplosion1, smallExplosion2, playerExplosion, playerHit, grappleHit,
     grappleShoot, lifeGet;

    public static SoundEffect[] shootSounds = { null, null, null };

    public static SpriteFont font, smallNumFont;

	public static void Load(ContentManager manager, Texture2D whitePixel) {
        pixel = whitePixel;
        //ui
        borders = manager.Load<Texture2D>("borders");
        comboCounter = manager.Load<Texture2D>("comboCounter");
        comboBar = manager.Load<Texture2D>("comboBar");
        comboBarBorder = manager.Load<Texture2D>("comboBarBorder");
        bossBarFrame = manager.Load<Texture2D>("bossBarFrame");
        bossBar = manager.Load<Texture2D>("bossBar");
        lives = manager.Load<Texture2D>("lightBullet");
        introText = manager.Load<Texture2D>("introText");
        cursor = manager.Load<Texture2D>("cursor");

        //player
        player = manager.Load<Texture2D>("player");
        PlayerBullet = manager.Load<Texture2D>("cannonBall");
        PlayerSmearBullet = manager.Load<Texture2D>("playerSmearBullet");
        enemyBullet = manager.Load<Texture2D>("lightBullet");
        bigShot = manager.Load<Texture2D>("bigShot");
        hook = manager.Load<Texture2D>("hook");
        chain = manager.Load<Texture2D>("chain");
        rippleL = manager.Load<Texture2D>("rippleL");
        rippleR = manager.Load<Texture2D>("rippleR");
        explosion = manager.Load<Texture2D>("explosion");
        //enemies
        introChest = loadEnemy("introChest", manager);
        enemy = loadEnemy("enemy", manager);
        bombEnemy = loadEnemy("bombEnemy", manager);
        mine = loadEnemy("mine", manager);
        trippleEnemyR = loadEnemy("trippleEnemyR", manager);
        Array.Resize(ref trippleEnemyR, 5);
        trippleEnemyR[3] = manager.Load<Texture2D>("enemies/trippleEnemyRLow2");
        trippleEnemyR[4] = manager.Load<Texture2D>("enemies/trippleEnemyRLow3");
        //bosses
        shipBoss = loadEnemy("shipBoss", manager);
        Array.Resize(ref shipBoss, 4);
        shipBoss[3] = manager.Load<Texture2D>("enemies/shipBossLowR");

        angryCrabBody = loadEnemy("angryCrabBody", manager);
        crabArm = manager.Load<Texture2D>("enemies/crabArm");
        crabHinge = manager.Load<Texture2D>("enemies/clawHinge");
        crabBody = manager.Load<Texture2D>("enemies/crabBody");
        crabEnter = manager.Load<Texture2D>("enemies/crabEnter");
        clawL = loadEnemy("crabClawL", manager);
        clawR = loadEnemy("crabClawR", manager);
        clawLOpen = loadEnemy("crabClawLOpen", manager);
        clawROpen = loadEnemy("crabClawROpen", manager);
        gunCrown = loadEnemy("gunCrown", manager);
        crown = manager.Load<Texture2D>("enemies/crown");

        //sounds
        music = manager.Load<Song>("sounds/music");
        bigExplosion = manager.Load<SoundEffect>("sounds/explosionBig");
        smallExplosion1 = manager.Load<SoundEffect>("sounds/explosionSmall");
        smallExplosion2 = manager.Load<SoundEffect>("sounds/explosionSmall2");
        playerExplosion = manager.Load<SoundEffect>("sounds/playerExplosion");
        playerHit = manager.Load<SoundEffect>("sounds/playerHit");
        grappleShoot = manager.Load<SoundEffect>("sounds/grappleShoot");
        grappleHit = manager.Load<SoundEffect>("sounds/grappleHit");
        lifeGet = manager.Load<SoundEffect>("sounds/lifeGet");
        shootSounds[0] = manager.Load<SoundEffect>("sounds/lv1Shoot");
        shootSounds[1] = manager.Load<SoundEffect>("sounds/lv2Shoot");
        shootSounds[2] = manager.Load<SoundEffect>("sounds/lv3Shoot");
        //bigExplosion = manager.Load<SoundEffect>("sounds/explosionBig");



        //fonts
        font = manager.Load<SpriteFont>("monoFont");
        smallNumFont = manager.Load<SpriteFont>("smallNumFont");

    }
    private static Texture2D[] loadEnemy(String name, ContentManager manager) {
        Console.WriteLine("loading enemy " + name);
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
