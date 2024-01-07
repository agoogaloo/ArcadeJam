﻿using System;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class Enemy : Node, IGrappleable {
    public IntData Health { get; protected set; } = new IntData(20);
    protected Sprite sprite;
    protected Texture2D[] textures;
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



    public Enemy(EnemyMovement movement, Texture2D[] textures) {
        renderHeight = 2;
        this.movement = movement;
        this.textures = textures;
        sprite = new(textures[0]);
        bounds = new(0, 0, 11, 13);
        grappleBounds = new(0, 0, 11, 13);
        vel = new(0, 0);
        movement.Init(bounds, vel);
        weapon = new Straight(bounds);
        renderer = new(sprite, bounds);
        damager = new(bounds, this, Health, sprite, textures[1]);
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

        if (!grappleable && Health.val < ArcadeGame.player.grappleDamage.val) {
            grappleCollision.Readd();
            grappleable = true;
        }
        else if (grappleable && Health.val > ArcadeGame.player.grappleDamage.val) {
            grappleable = false;
            grappleCollision.Remove();
        }

        sprite.texture = textures[0];
        if (grappleable) {
            sprite.texture = textures[2];

        }
        damager.Update();
        if (Health.val <= 0) {
            Alive = false;
        }
    }
    protected void updateGrappleBounds() {
        grappleBounds.x = bounds.Centre.X - grappleBounds.width / 2;
        grappleBounds.y = bounds.y;
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

        renderer.Draw(spriteBatch);
        hitBoxVisualizer.bounds = grappleBounds;
        hitBoxVisualizer.Draw(spriteBatch);
        hitBoxVisualizer.bounds = bounds;
        hitBoxVisualizer.Draw(spriteBatch);
    }

    public void GrappleStun() {
        stunned = true;

    }
    public virtual void GrappleHit(int damage) {
        Health.val -= damage;
        stunned = false;

    }


    public override void End() {
        damager.End();
        grappleCollision.Remove();
    }
}
public class IntroChest : Enemy {
    public IntroChest() : base(new Stationary(), Assets.introChest) {
        Health.val = 27;
        weapon = new Nothing();
        bounds.Centre = new Vector2(71, 65);
        bounds.width = 14;
        bounds.height = 20;
        grappleBounds.width = 14;
        grappleBounds.height = 20;

    }
    public override void Update(GameTime gameTime) {
        if (!grappleable) {
            base.Update(gameTime);
        }
    }
    public override void GrappleHit(int damage) {
        Alive = false;
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        base.Draw(gameTime, spriteBatch);
        spriteBatch.Draw(Assets.introText, new Vector2(ArcadeGame.gameWidth / 2 - Assets.introText.Width / 2, 5), Color.White);

    }
}

public class BasicEnemy : Enemy {

    public BasicEnemy(EnemyMovement movement) : base(movement, Assets.enemy) {
        Health.val = 10;

    }
}

public class TrippleEnemy : Enemy {

    public TrippleEnemy(EnemyMovement movement) : base(movement, Assets.enemy) {
        Health.val = 30;
        weapon = new Tripple(bounds);

    }
}

public class SpinEnemy : Enemy {

    public SpinEnemy(EnemyMovement movement) : base(movement, Assets.enemy2) {
        Health.val = 50;
        bounds.width = 32;
        weapon = new Spiral(bounds);
        renderer = new(sprite, bounds);

    }

}



