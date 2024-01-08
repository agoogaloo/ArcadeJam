using System;
using System.ComponentModel;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Input;
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
    EnemyWeapon[] patterns = { };
    public CrabBoss() {
        BoolData lJabbing = new(false), rJabbing = new(false);
        leftClaw = new(true, lJabbing, bounds, lClawPhase);
        rightClaw = new(false, rJabbing, bounds, rClawPhase);
        renderer = new(sprite, bounds);
        movement = new(leftClaw.Bounds, rightClaw.Bounds, lJabbing, rJabbing, bounds);
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
                FullBodyPhase(gameTime);
                break;
            case 2:
                missingClawPhase(gameTime);
                break;

        }
        

    }

    private void enter(GameTime gameTime) {

        if (bounds.Centre.X >= 75) {
            phase.val++;

            patterns = new EnemyWeapon[] { new AimedParallel(bounds, 1.6f) };
            movement.movementState = Movements.idle;

            lClawPhase.val = 1;
            rClawPhase.val = 1;

        }
        leftClaw.Update(gameTime);
        rightClaw.Update(gameTime);

    }
    private void FullBodyPhase(GameTime gameTime) {
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }
        leftClaw.Update(gameTime);
        rightClaw.Update(gameTime);
        if (!leftClaw.Alive) {
            leftClaw.end();
            phase.val++;
            rightClaw.startPhase3();
            patterns = new EnemyWeapon[] { new SpreadAlternating(bounds,rows:6) };
             movement.movementState = Movements.rightJab;
        }
        if (!rightClaw.Alive) {
            rightClaw.end();
            phase.val++;
            leftClaw.startPhase3();
            patterns = new EnemyWeapon[] { new SpreadAlternating(bounds,rows:6) };
            movement.movementState = Movements.leftJab;
        }

    }
    private void missingClawPhase(GameTime gameTime) {
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }
         if (leftClaw.Alive) {
            leftClaw.Update(gameTime);
           
        }
        if (rightClaw.Alive) {
            rightClaw.Update(gameTime);
        }
        
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(spriteBatch);
        if (leftClaw.Alive) {
            leftClaw.Draw(gameTime, spriteBatch);
        }
        if (rightClaw.Alive) {
            rightClaw.Draw(gameTime, spriteBatch);
        }


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
    BoolData lClawJab, rClawJab;
    public Movements movementState = Movements.enter;

    float jabAngle, jabSpeed = 200;

    float timer = 0, jabReturnDelay = 0.5f;
    public CrabMovement(FloatRect lClaw, FloatRect rClaw, BoolData lClawJab, BoolData rClawJab, FloatRect bounds) {
        this.lClaw = lClaw;
        this.rClaw = rClaw;
        this.rClawJab = rClawJab;
        this.lClawJab = lClawJab;
        this.bounds = bounds;
        bounds.x = -90;
        bounds.y = 2;
        lClaw.y = 20;
        rClaw.y = 20;
        lClaw.x = bounds.Centre.X - 35 - lClaw.width;
        rClaw.x = bounds.Centre.X + 35;


    }
    public void Update(GameTime gameTime) {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch (movementState) {
            case Movements.enter:
                bounds.x += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                lClaw.x += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                rClaw.x += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                break;

            case Movements.idle:

                lClaw.y += (float)(Math.Sin(timer) * gameTime.ElapsedGameTime.TotalSeconds * 10);
                rClaw.y += (float)(Math.Cos(timer) * gameTime.ElapsedGameTime.TotalSeconds * 10);
                lClaw.x += (float)(Math.Cos(timer) * gameTime.ElapsedGameTime.TotalSeconds * 5);
                rClaw.x += (float)(Math.Sin(timer) * gameTime.ElapsedGameTime.TotalSeconds * 5);
                break;
            case Movements.leftJab:
                Jab(gameTime, lClawJab, lClaw, new Vector2(bounds.x - 15, bounds.y + 15));
                break;
            case Movements.rightJab:
                Jab(gameTime, rClawJab, rClaw, new Vector2(bounds.Right + 15, bounds.y + 15));
                break;


        }
    }
    private void Jab(GameTime gameTime, BoolData jabbing, FloatRect bounds, Vector2 returnPoint) {
        if (jabbing.val) {
            bounds.x += (float)(Math.Sin(jabAngle) * jabSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            bounds.y -= (float)(Math.Cos(jabAngle) * jabSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            timer = 0;
        }
        else {
            if (timer < jabReturnDelay) {
                return;
            }
            Vector2 positionDiff = returnPoint - bounds.Centre;
            bounds.x += (float)(positionDiff.X / 3 * gameTime.ElapsedGameTime.TotalSeconds *
            gameTime.ElapsedGameTime.TotalSeconds * 100);
            bounds.y += (float)(positionDiff.Y / 3 * gameTime.ElapsedGameTime.TotalSeconds *
             gameTime.ElapsedGameTime.TotalSeconds * 100);
            if (positionDiff.Length() < 15) {
                jabbing.val = true;
                Vector2 angleCartesian = ArcadeGame.player.Bounds.Centre - bounds.Centre;
                jabAngle = (float)Math.Atan(angleCartesian.Y / angleCartesian.X);
                if (angleCartesian.X > 0) {
                    jabAngle += (float)Math.PI;
                }
                jabAngle -= (float)Math.PI / 2;

            }


        }
        Console.WriteLine(jabbing.val + ", " + jabAngle / (float)(Math.PI / 180));


    }
}

