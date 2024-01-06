﻿using System;
using System.Collections.Generic;
using ArcadeJam.Enemies;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Weapons;

public class BulletBasicMovement {

    private VelMovement movement;
    RectRender renderer;
    RectVisualizer boundsVisualizer;



    public BulletBasicMovement(Vector2Data vel, FloatRect bounds, Sprite texture) {
        movement = new(vel, bounds);
        renderer = new(texture, bounds);
        boundsVisualizer = new(bounds);

    }

    public void Update(GameTime gameTime) {


        movement.Update(gameTime);

    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw( spriteBatch);
        boundsVisualizer.Draw(spriteBatch);

    }

}

public abstract class Bullet : Node {
    protected FloatRect bounds;

    protected BulletBasicMovement movement;
    protected Collision collision;
    protected List<Node> collisions = new();

    protected string[] collisionGroups = new string[] { "player" };


    public Bullet(Vector2Data vel, Vector2 startPos, Sprite sprite, FloatRect bounds, String group) {
        this.bounds = bounds;
        movement = new(vel, bounds, sprite);
        collision = new(bounds, this, group, collisions);


    }
    public override void Update(GameTime gameTime) {
        movement.Update(gameTime);

        //removing itself if it goes offscreen
        if (bounds.Top > ArcadeGame.height || bounds.Bottom < 0 || bounds.Left > ArcadeGame.width || bounds.Right < 0) {
            Alive = false;
        }
    }
    public virtual void OnHit() {
        Alive = false;
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        movement.Draw(gameTime, spriteBatch);
    }
    public override void End() {
        collision.Remove();
    }

}

public class PlayerBullet : Bullet {

    public int Damage { get; protected set; } = 1;


    public PlayerBullet(Vector2Data vel, Vector2 startPos, int damage = 1) : this(vel,
    startPos, new Sprite(Assets.PlayerBullet), damage) { }

    public PlayerBullet(Vector2Data vel, Vector2 startPos, Sprite sprite, int Damage = 1) : base(vel, startPos,
        sprite, new(startPos.X - 2.5f, startPos.Y - 2.5f, 5, 5), "playerBullet") {
        renderHeight = 3;
        this.Damage = Damage;
    }

}
public class PlayerSmearBullet : PlayerBullet {


    public PlayerSmearBullet(float speed, Vector2 startPos, int Damage = 1) : base(new Vector2Data(0, -speed),
    startPos, new Sprite(Assets.PlayerSmearBullet), Damage ) {bounds.height = 15;}
        
    public override void OnHit() {
        base.OnHit();
        Console.WriteLine("smack"+Damage);
    }

}




public class EnemyBullet : Bullet {
    private const int width = 5, height = 5;

    public EnemyBullet(Vector2Data vel, Vector2 startPos) : base(vel, startPos, new(Assets.enemyBullet),
    new(startPos.X - width / 2, startPos.Y - height / 2, width, height), "enemyBullet") {
        renderHeight = 4;
    }

}