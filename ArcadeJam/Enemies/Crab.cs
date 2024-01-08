using System;
using System.ComponentModel;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ArcadeJam;


public enum Movements {
    enter, leftJab, rightJab, idle
}

public class CrabBoss : Node, IGrappleable {
    IntData health = new(150), phase = new(0), lClawPhase = new(), rClawPhase = new();
    FloatData time = new();
    FloatRect bounds = new(0, 0, 75, 48);
    Sprite sprite = new(Assets.crabBody[0]);
    Claw leftClaw;
    Claw rightClaw;


    CrabMovement movement;
    RectRender renderer;

    EnemyDamage damager;
    EnemyWeapon[] patterns = {};
    public CrabBoss() {
        leftClaw = new(true, bounds, lClawPhase);
        rightClaw = new(false, bounds, rClawPhase);
        renderer = new(sprite, bounds);
        movement = new(leftClaw.Bounds, rightClaw.Bounds, bounds);
        damager = new(bounds, null, health, sprite, Assets.crabBody[2]);

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
                phase1(gameTime);
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
            
            patterns = new EnemyWeapon[]{new AimedParallel(bounds, 1.6f)};
            movement.movementState = Movements.idle;
            lClawPhase.val = 1;
            rClawPhase.val = 1;
            
        }

    }
    private void phase1(GameTime gameTime) {
       
        foreach( EnemyWeapon i in patterns){
            i.Update(gameTime);
        }

    }
    private void phase2(GameTime gameTime) {
        foreach( EnemyWeapon i in patterns){
            i.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(spriteBatch);
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
        leftClaw.end();
        rightClaw.end();
        damager.End();

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

