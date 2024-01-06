using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ArcadeJam.Enemies;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;

public class Level {
    protected Node[] enemies;
    public bool Cleared { get; protected set; } = false;

    public virtual void Start() {
        enemies = new Enemy[] {
        new BasicEnemy(new MoveToPoint(new Vector2(75,0), new Vector2(75,50))) };
        addEnemies();
    }
    protected void addEnemies() {
        foreach (Node i in enemies) {
            NodeManager.AddNode(i);
        }
    }
    public void Update(GameTime gameTime) {
        Cleared = true;
        foreach (Node i in enemies) {
            if (i.Alive) {
                Cleared = false;
            }
        }

    }
}
public class Level1 : Level {
    public override void Start() {
        enemies = new Enemy[]{
        new TrippleEnemy(new MoveToPoint(new Vector2(60,-20), new Vector2(60,40))),

        new BasicEnemy(new MoveToPoint(new Vector2(75,0), new Vector2(75,50))),

        new TrippleEnemy(new MoveToPoint(new Vector2(90,-25), new Vector2(90,40))),
        };
        addEnemies();
    }

}

public class Level2 : Level {
    public override void Start() {
        enemies = new Enemy[]{

         new BasicEnemy(new MoveToPoint(new Vector2(30,-20), new Vector2(50,50))),

         new BasicEnemy(new MoveToPoint(new Vector2(60,-40), new Vector2(65,50))),

        new TrippleEnemy(new MoveToPoint(new Vector2(75,0), new Vector2(75,50))),
         new BasicEnemy(new MoveToPoint(new Vector2(90,-40), new Vector2(85,50))),

         new BasicEnemy(new MoveToPoint(new Vector2(120,-20), new Vector2(100,50))),

        };
        addEnemies();
    }

}
public class Level3 : Level {
    public override void Start() {
        enemies = new Enemy[]{
        new BasicEnemy(new MoveToPoint(new Vector2(25,-20), new Vector2(60,50))),
        new SpinEnemy(new MoveToPoint(new Vector2(75,0), new Vector2(180,50),speed:40f)),
        new BasicEnemy(new MoveToPoint(new Vector2(130,-20), new Vector2(90,50))),
        };
        addEnemies();
    }

}
public class ShipBossStage : Level {
    public override void Start() {
        enemies = new Enemy[]{new ShipBoss()};
        addEnemies();
    }

}
public class CrabBossStage : Level {
    public override void Start() {
       enemies = new Node[]{new CrabBoss()};
        addEnemies();
    }

}