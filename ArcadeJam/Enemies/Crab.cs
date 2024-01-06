using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;


public enum Movements {
    enter, leftJab, rightJab, idle
}

public class CrabBoss : Node, IGrappleable {
    IntData health = new(150), phase = new(0);
    FloatRect bounds = new(0, 0, 75, 48);
    Sprite sprite = new(Assets.crabBody);
    Claw leftClaw;
    Claw rightClaw;

    CrabMovement movement;
    RectRender renderer;

    EnemyDamage damager;
    EnemyWeapon currentPattern;
    public CrabBoss() {
        leftClaw = new(true, bounds);
        rightClaw = new(false, bounds);
        renderer = new(sprite, bounds);
        movement = new(leftClaw.Bounds, rightClaw.Bounds, bounds);
        damager = new(bounds, null, health);

    }


    public override void Update(GameTime gameTime) {
        damager.Update();
        movement.Update(gameTime);
        if (health.val <= 0) {
            Alive = false;
        }
        switch (phase.val) {
            case 0:
                enter(gameTime);
                break;
            case 1:
                //phase1(gameTime);
                break;
            case 2:
                phase2(gameTime);
                break;

        }
        leftClaw.Update(gameTime);
        rightClaw.Update(gameTime);

    }

    private void enter(GameTime gameTime) {
        if (bounds.Centre.X >= 75) {
            phase.val++;
            currentPattern = new Spiral(bounds, 0.5f, 20, 180 * 2 / 20, 50);
            movement.movementState = Movements.idle;
        }

    }
    private void phase1(GameTime gameTime) {
        currentPattern.Update(gameTime);

    }
    private void phase2(GameTime gameTime) {

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw( spriteBatch);
        leftClaw.Draw(gameTime, spriteBatch);
        rightClaw.Draw(gameTime, spriteBatch);


    }

    public void GrappleStun() {
        throw new NotImplementedException();
    }

    public void GrappleHit(int damage) {
        throw new NotImplementedException();
    }
    public override void End() {
        base.End();
        damager.End();

    }
}


public class Claw {
    bool left;
    IntData health = new(150), phase = new(0);
    public FloatRect Bounds { get; private set; } = new(0, 0, 24, 28);
    FloatRect bodyBounds;
    Sprite sprite;
    RectRender renderer;
    RectVisualizer hitBoxVisualizer;

    Vector2Data[] armLocs = { new(), new(), new() };
    PointRender[] armSegs;


    EnemyDamage damager;
    EnemyWeapon currentPattern;

    public Claw(bool left, FloatRect bodyBounds) {
        this.left = left;
        currentPattern = new SpreadAlternating(Bounds);
        this.bodyBounds = bodyBounds;
        if (left) {
            sprite = new(Assets.clawL);
        }
        else {
            sprite = new(Assets.clawR);
        }
        armSegs = new PointRender[armLocs.Length];
        for (int i = 0; i < armLocs.Length; i++) {
            Console.WriteLine(armLocs[i]);
            armSegs[i] = new(new Sprite(Assets.crabArm), armLocs[i], true);
        }

        renderer = new(sprite, Bounds);
        damager = new(Bounds, null, health);
        hitBoxVisualizer = new(Bounds);



    }
    public void Update(GameTime gameTime) {
        currentPattern.Update(gameTime);
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        Vector2 shoulderDiff = new();

        if (left) {
            shoulderDiff = bodyBounds.Location;
            shoulderDiff.X += 15;

        }
        else {
            shoulderDiff = bodyBounds.Location;
            shoulderDiff.X += bodyBounds.width - 15;
        }
        shoulderDiff.Y += Bounds.height / 2+15;
        shoulderDiff -= Bounds.Centre;
        for (int i = 0; i < armLocs.Length; i++) {
            armLocs[i].val = Bounds.Centre + i * shoulderDiff / (armLocs.Length);
            armLocs[i].val.Y -= Bounds.height / 2;

        }
        foreach (PointRender i in armSegs) {
            i.Draw( spriteBatch);
        }
        renderer.Draw( spriteBatch);
        hitBoxVisualizer.Draw(spriteBatch);


    }
}
public class CrabMovement {
    FloatRect lClaw, rClaw;
    FloatRect bounds;
    public Movements movementState = Movements.enter;

    float idleAnimTime = 0;
    public CrabMovement(FloatRect lClaw, FloatRect rClaw, FloatRect bounds) {
        this.lClaw = lClaw;
        this.rClaw = rClaw;
        this.bounds = bounds;
        bounds.x = -90;
        bounds.y = 2;
        lClaw.y = 20;
        rClaw.y = 20;
        lClaw.x = bounds.Centre.X - 35 - lClaw.width;
        rClaw.x = bounds.Centre.X + 35;


    }
    public void Update(GameTime gameTime) {

        switch (movementState) {
            case Movements.enter:
                bounds.x += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                lClaw.x += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                rClaw.x += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                break;

            case Movements.idle:
                idleAnimTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                lClaw.y += (float)(Math.Sin(idleAnimTime) * gameTime.ElapsedGameTime.TotalSeconds * 10);
                rClaw.y += (float)(Math.Cos(idleAnimTime) * gameTime.ElapsedGameTime.TotalSeconds * 10);
                lClaw.x += (float)(Math.Cos(idleAnimTime) * gameTime.ElapsedGameTime.TotalSeconds * 5);
                rClaw.x += (float)(Math.Sin(idleAnimTime) * gameTime.ElapsedGameTime.TotalSeconds * 5);
                break;

        }
    }


}

