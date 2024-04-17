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
    public int SpeedBonus { get; protected set; } = 700;

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
    bool startedMusic = false;
    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[] { new IntroChest(new Vector2(45, 60)),new IntroChest(new Vector2(105, 75)) };
        addEnemies();
    }
    protected override void EnemiesDefeated() {
        base.EnemiesDefeated();

        if (!startedMusic && MediaPlayer.State == MediaState.Stopped) {
            startedMusic = true;
            MediaPlayer.Play(Assets.music);
            Console.WriteLine("AAAAAA");
            MediaPlayer.IsRepeating = true;
        }

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
        new SegmentEnemy(new MoveToPoint(new Vector2(-32,5),new Vector2(120,20),speed:15),score,2),
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
        new Mine(new MoveToPoint(new Vector2(90,-10),new Vector2(60,55),speed:50),score),

        new BasicEnemy(new MoveToPoint(new Vector2(75,-10), new Vector2(75,60),speed:30,delay:1f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(50,-10), new Vector2(60,50),speed:35,delay:2f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(100,-10), new Vector2(90,50),speed:35,delay:4.5f), score),
        new AimedEnemy(new MoveToPoint(new Vector2(65,-10), new Vector2(65,25),speed:30,delay:2.75f), score),
        new AimedEnemy(new MoveToPoint(new Vector2(85,-10), new Vector2(85,25),speed:30,delay:5f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(0,-10), new Vector2(45,35),speed:35,delay:3.25f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(153,-10), new Vector2(105,35),speed:35,delay:5.5f), score),
        new Mine(new MoveToPoint(new Vector2(10,-10),new Vector2(90,60),speed:30,delay:4f),score),
        new Mine(new MoveToPoint(new Vector2(143,-10),new Vector2(60,60),speed:35,delay:4f),score),

        };
        addEnemies();
    }

}
public class Level6 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        SpeedBonus = 1000;
        enemies = new Enemy[]{
        new SegmentEnemy(new MoveToPoint(new Vector2(-35,10),new Vector2(100,15),speed:40),score,1),
        new SegmentEnemy(new MoveToPoint(new Vector2(153+35,30),new Vector2(50,25),speed:20),score,3),
        new Mine(new MoveToPoint(new Vector2(75,-5),new Vector2(75,50),speed:30,delay:1.5f),score ),
        //new Mine(new MoveToPoint(new Vector2(70,-5),new Vector2(75-45,45),speed:30,delay:2f),score),
        new Mine(new MoveToPoint(new Vector2(80,-5),new Vector2(75+45,45),speed:30,delay:2f),score),
        };
        addEnemies();
    }

}
public class Level7 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        SpeedBonus = 1000;
        enemies = new Enemy[]{
        new SegmentEnemy(new MoveToPoint(new Vector2(-35,40),new Vector2(100,25),easing:2,speed:30),score,2),
        new BombEnemy(new MoveToPoint(new Vector2(160,10),new Vector2(10,55),speed:20,delay:1.5f),score),
        };
        addEnemies();
    }

}
public class Level8 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        SpeedBonus = 1000;
        enemies = new Enemy[]{
        new SegmentEnemy(new MoveToPoint(new Vector2(-30,40),new Vector2(65,15),easing:2,speed:40),score,3),
        new BombEnemy(new MoveToPoint(new Vector2(ArcadeGame.gameWidth+10,0),
            new Vector2(90,65),speed:20,delay:4f),score),
        
        new AimedEnemy(new MoveToPoint(new Vector2(20,-10), new Vector2(55,50),speed:30,delay:2f), score),
        new TrippleEnemy(new MoveToPoint(new Vector2(60,-10), new Vector2(75,50),speed:30,delay:3f), score),
        new AimedEnemy(new MoveToPoint(new Vector2(90,-10), new Vector2(105,40),speed:30,delay:4f), score),
        
        };
        addEnemies();
    }

}
public class Level9 : Level {
    public override void Start(ScoreData score, int loops = 1) {
        SpeedBonus = 1000;
        enemies = new Enemy[]{
        
        new BasicEnemy(new MoveToPoint(new Vector2(130,-10), new Vector2(130,70),speed:30), score),
        new BasicEnemy(new MoveToPoint(new Vector2(115,-10), new Vector2(115,60),speed:30,delay:1f), score),
        new BasicEnemy(new MoveToPoint(new Vector2(100,-10), new Vector2(100,50),speed:30,delay:2f), score),

        new SegmentEnemy(new MoveToPoint(new Vector2(-30,5),new Vector2(100,20),speed:40,delay:2),score,1),
        
        new AimedEnemy(new MoveToPoint(new Vector2(28,-10), new Vector2(39,30),speed:25,delay:6f), score),
        //new AimedEnemy(new MoveToPoint(new Vector2(130,-10), new Vector2(125,60),speed:30,delay:5f), score),
        
        };
        addEnemies();
    }

}

public class ShipBossStage : Level {

    public override void Start(ScoreData score, int loops = 1) {
        enemies = new Enemy[] { new ShipBoss(score) };
        addEnemies();
        SpeedBonus = 3000;
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