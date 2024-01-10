using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ArcadeJam.Enemies;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace ArcadeJam.Enemies;

public class Level {
    protected Node[] enemies;
    public bool Cleared { get; set; } = false;
    public int SpeedBonus { get; protected set; } = 1500;

    public virtual void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[] {
        new BasicEnemy(new MoveToPoint(new Vector2(75,0), new Vector2(75,50)), score) };
        addEnemies();
    }
    protected void addEnemies() {
        foreach (Node i in enemies) {
            NodeManager.AddNode(i);
        }
    }
    public void Update(GameTime gameTime) {
        bool enemiesLeft = false;
        foreach (Node i in enemies) {
            if (i.Alive) {
                enemiesLeft = true;
                Console.WriteLine(i+" is still alive");

            }
        }
        if (!enemiesLeft) {
            EnemiesDefeated();


        }

    }
    protected virtual void EnemiesDefeated() {
        Cleared = true;
    }
}
public class Intro : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[] { new IntroChest() };
        addEnemies();
    }
    protected override void EnemiesDefeated() {
        base.EnemiesDefeated();
        
        MediaPlayer.Play(Assets.music);
        Console.WriteLine("AAAAAA");
        MediaPlayer.IsRepeating = true;

    }

}
public class Level1 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[]{
        new BombEnemy(new MoveToPoint(new Vector2(60,-20), new Vector2(60,40)), score),

        new BasicEnemy(new MoveToPoint(new Vector2(75,0), new Vector2(75,50)), score),

        new BombEnemy(new MoveToPoint(new Vector2(200,20), new Vector2(90,40)), score),
        };
        addEnemies();
    }

}

public class Level2 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[]{

         new BasicEnemy(new MoveToPoint(new Vector2(30,-20), new Vector2(50,50)), score),

         new BasicEnemy(new MoveToPoint(new Vector2(60,-40), new Vector2(65,50)), score),

        new TrippleEnemy(new MoveToPoint(new Vector2(75,0), new Vector2(75,50)), score),
         new BasicEnemy(new MoveToPoint(new Vector2(90,-40), new Vector2(85,50)), score),

         new BasicEnemy(new MoveToPoint(new Vector2(120,-20), new Vector2(100,50)), score),

        };
        addEnemies();
    }

}
public class Level3 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[]{
        new BasicEnemy(new MoveToPoint(new Vector2(25,-20), new Vector2(60,50)), score),
        new SpinEnemy(new MoveToPoint(new Vector2(75,0), new Vector2(180,50),speed:40f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(130,-20), new Vector2(90,50)), score),
        };
        addEnemies();
    }

}
public class ShipBossStage : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[] { new ShipBoss(score) };
        addEnemies();
    }

}
public class CrabBossStage : Level {
    FloatData bossBar;
    public CrabBossStage(FloatData bossBar) {
        SpeedBonus = 5000;
        this.bossBar = bossBar;
    }
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Node[] { new CrabBoss(score, bossBar)};
        addEnemies();
    }

}