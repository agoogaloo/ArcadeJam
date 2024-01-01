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
    private IntData health = new IntData(20);
    private Sprite sprite = new(Assets.enemy);
    private Vector2Data vel = new(20f, 0);
    private FloatRect bounds = new(20, 0, 10, 10);

    private RectRender renderer;

    private EnemyMovement movement;
    EnemyDamage damager;
    Straight pattern;



    public Enemy(Vector2 start, Vector2 destination) {
        bounds.Centre = start;
        renderer = new(sprite, bounds);
        movement = new MoveToPoint(bounds, vel, destination);
        pattern = new(bounds);
        
        damager = new(bounds, this, health);

    }


    public override void Update(GameTime gameTime) {
        movement.Update(gameTime);
        pattern.Update(gameTime);
        damager.Update();
        if (health.val <= 0) {
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
