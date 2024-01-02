using System;
using System.Collections.Generic;

using ArcadeJam.Enemies;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;

public class Level {
    protected Enemy[] enemies;
    public bool Cleared { get; protected set; } = false;

    public Level() : this(new Enemy[] {
        new BasicEnemy(new MoveToPoint(new Vector2(180,0), new Vector2(180,50))) }) { }
    public Level(Enemy[] enemies) {
        this.enemies = enemies;
    }
    public void Start(){
        foreach (Enemy i in enemies) {
            NodeManager.AddNode(i);
        }
    }
    public void Update(GameTime gameTime) {
        Cleared = true;
        foreach (Enemy i in enemies) {
            if (i.Alive) {
                Cleared = false;
            }
        }

    }
}
public class Level1 : Level {
    public Level1() : base(new Enemy[]{
        new TrippleEnemy(new MoveToPoint(new Vector2(150,-20), new Vector2(160,40))),

        new BasicEnemy(new MoveToPoint(new Vector2(180,0), new Vector2(180,50))),

        new TrippleEnemy(new MoveToPoint(new Vector2(210,-25), new Vector2(200,40))),

    }) { }

}

public class Level2 : Level {
    public Level2() : base(new Enemy[]{

         new BasicEnemy(new MoveToPoint(new Vector2(150,-20), new Vector2(160,50))),
         new BasicEnemy(new MoveToPoint(new Vector2(140,-30), new Vector2(130,50))),

        new TrippleEnemy(new MoveToPoint(new Vector2(180,0), new Vector2(180,50))),
         new BasicEnemy(new MoveToPoint(new Vector2(210,0), new Vector2(200,50))),

         new BasicEnemy(new MoveToPoint(new Vector2(220,0), new Vector2(230,50))),

    }) { }

}
public class Level3 : Level {
    public Level3() : base(new Enemy[]{
        new BasicEnemy(new MoveToPoint(new Vector2(150,-20), new Vector2(160,50))),
        new SpinEnemy(new MoveToPoint(new Vector2(180,0), new Vector2(180,50))),
        new BasicEnemy(new MoveToPoint(new Vector2(220,0), new Vector2(230,50))),
    }) { }

}