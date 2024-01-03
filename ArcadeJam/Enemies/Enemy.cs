using System;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class Enemy : Node {
    public IntData Health{get; protected set;} = new IntData(20);
    protected Sprite sprite = new(Assets.enemy);
    protected Vector2Data vel;
    protected FloatRect bounds;

    protected RectRender renderer;

    protected EnemyMovement movement;
    protected EnemyDamage damager;
    protected EnemyWeapon weapon;



    public Enemy(EnemyMovement movement) {
        renderHeight = 2;
        this.movement = movement;
        bounds = new(0, 0, 10, 10);
        vel = new(0, 0);
        movement.Init(bounds, vel);
        weapon = new Straight(bounds);
        renderer = new(sprite, bounds);
        damager = new(bounds, this, Health);

    }


    public override void Update(GameTime gameTime) {
        movement.Update(gameTime);
        weapon.Update(gameTime);
        damager.Update();
        if (Health.val <= 0) {
            Alive = false;
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(gameTime, spriteBatch);
    }


    public override void End() {
        damager.Update();
    }
}


public class BasicEnemy : Enemy {

    public BasicEnemy(EnemyMovement movement) : base(movement) {
        Health.val = 5;

    }
}

public class TrippleEnemy : Enemy {

    public TrippleEnemy(EnemyMovement movement) : base(movement) {
        Health.val = 20;
        weapon = new Tripple(bounds);

    }
}

public class SpinEnemy : Enemy {

    public SpinEnemy(EnemyMovement movement) : base(movement) {
        Health.val = 15;
        weapon = new Spiral(bounds);
        sprite = new(Assets.enemy2);
        renderer = new(sprite, bounds);

    }
	
}


