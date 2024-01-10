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
    public int SpeedBonus { get; protected set; } = 500;

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

        MediaPlayer.IsRepeating = true;

    }

}
public class Level1 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[]{
        new AimedEnemy(new MoveToPoint(new Vector2(75,-100), new Vector2(75,40)), score),

        new BasicEnemy(new MoveToPoint(new Vector2(-10,-10), new Vector2(40,20)), score),
        new BasicEnemy(new MoveToPoint(new Vector2(ArcadeGame.gameWidth+10,-10), new Vector2(ArcadeGame.gameWidth-40,20)), score),

        };
        addEnemies();
    }

}

public class Level2 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[]{
        new TrippleEnemy(new MoveToPoint(new Vector2(75,-10), new Vector2(75,50)), score),
        new BasicEnemy(new MoveToPoint(new Vector2(-5,60), new Vector2(60,30),delay:1.5f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(ArcadeGame.gameWidth+5,60), new Vector2(120,30),delay:3f), score),

        };
        addEnemies();
    }

}
public class Level3 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[]{
        new SegmentEnemy(new MoveToPoint(new Vector2(-10,5),new Vector2(100,20),speed:10),score,2),
        };
        addEnemies();
    }

}
public class Level4 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[]{
        new SegmentEnemy(new MoveToPoint(new Vector2(-35,10),new Vector2(100,30),speed:30),score,1),
        new SegmentEnemy(new MoveToPoint(new Vector2(-35,50),new Vector2(120,50),speed:60,easing:2.5f,delay:2),score,3),

        };
        addEnemies();
    }

}
public class Level5 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[]{
        new Mine(new MoveToPoint(new Vector2(85,-10),new Vector2(65,55),speed:50),score),

        new BasicEnemy(new MoveToPoint(new Vector2(75,-10), new Vector2(75,60),speed:30,delay:1.5f), score),
        new AimedEnemy(new MoveToPoint(new Vector2(65,-10), new Vector2(65,50),speed:30,delay:2.5f), score),
        new AimedEnemy(new MoveToPoint(new Vector2(85,-10), new Vector2(85,50),speed:30,delay:2.5f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(55,-10), new Vector2(55,40),speed:30,delay:3.5f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(95,-10), new Vector2(95,40),speed:30,delay:3.5f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(45,-10), new Vector2(45,30),speed:30,delay:4.5f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(105,-10), new Vector2(105,30),speed:30,delay:4.5f), score),
        new Mine(new MoveToPoint(new Vector2(-10,20),new Vector2(50,50),speed:30,delay:3.75f),score),
        new Mine(new MoveToPoint(new Vector2(163,20),new Vector2(100,50),speed:30,delay:4f),score),

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
        enemies = new Node[] { new CrabBoss(score, bossBar) };
        addEnemies();
    }

}