using System;
using System.Collections.Generic;

using ArcadeJam.Enemies;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;

public class Level {
    protected Enemy[] enemies;
    public bool Cleared { get; protected set; } = false;

    public Level() : this(new Enemy[] { new Enemy(new Vector2(180, -10), new Vector2(180, 50)) }) { }
    public Level(Enemy[] enemies) {
        this.enemies = enemies;
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
