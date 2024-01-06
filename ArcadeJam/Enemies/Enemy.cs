using System;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class Enemy : Node,IGrappleable {
    public IntData Health { get; protected set; } = new IntData(20);
    protected Sprite sprite = new(Assets.enemy);
    protected Vector2Data vel;
    protected FloatRect bounds;

    protected RectRender renderer;

    protected EnemyMovement movement;
    protected EnemyDamage damager;
    protected EnemyWeapon weapon;
    protected Collision grappleCollision;
    protected FloatRect grappleBounds;
    RectVisualizer hitBoxVisualizer;
    protected bool stunned = false, grappleable = false;



    public Enemy(EnemyMovement movement) {
        renderHeight = 2;
        this.movement = movement;
        bounds = new(0, 0, 11, 13);
        grappleBounds = new(0,0,11,13);
        vel = new(0, 0);
        movement.Init(bounds, vel);
        weapon = new Straight(bounds);
        renderer = new(sprite, bounds);
        damager = new(bounds, this, Health);
        grappleCollision = new(grappleBounds, this, "grapple");
        grappleCollision.Remove();
        hitBoxVisualizer = new(grappleBounds);
    }


    public override void Update(GameTime gameTime) {
        if (!stunned) {
            movement.Update(gameTime);
            weapon.Update(gameTime);
            updateGrappleBounds();
        }
        damager.Update();
        if (Health.val <= 0) {
            Alive = false;
        }else if (!grappleable && Health.val<ArcadeGame.player.grappleDamage.val){
            grappleCollision.Readd();
            sprite.texture = Assets.enemyStun;
            grappleable = true;
        }else if (grappleable && Health.val>ArcadeGame.player.grappleDamage.val){
            grappleable = false;
            grappleCollision.Remove();
            sprite.texture = Assets.enemy;
        }
    }
    protected void updateGrappleBounds(){
        grappleBounds.x = bounds.Centre.X-grappleBounds.width/2;
        grappleBounds.y = bounds.y;
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        
        renderer.Draw( spriteBatch);
        hitBoxVisualizer.bounds = grappleBounds;
        hitBoxVisualizer.Draw(spriteBatch);
        hitBoxVisualizer.bounds = bounds;
        hitBoxVisualizer.Draw(spriteBatch);
    }

    public void GrappleStun() {
        stunned = true;

    }
    public void GrappleHit(int damage) {
        Health.val-=damage;
        stunned = false;

    }


    public override void End() {
        damager.End();
        grappleCollision.Remove();
    }
}


public class BasicEnemy : Enemy {

    public BasicEnemy(EnemyMovement movement) : base(movement) {
        Health.val = 10;

    }
}

public class TrippleEnemy : Enemy {

    public TrippleEnemy(EnemyMovement movement) : base(movement) {
        Health.val = 30;
        weapon = new Tripple(bounds);

    }
}

public class SpinEnemy : Enemy {

    public SpinEnemy(EnemyMovement movement) : base(movement) {
        Health.val = 50;
        bounds.width =32;
        weapon = new Spiral(bounds);
        sprite = new(Assets.enemy2);
        renderer = new(sprite, bounds);

    }

}


