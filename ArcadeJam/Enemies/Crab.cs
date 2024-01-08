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
    IntData health = new(150),crownHealth = new(100), phase = new(0), lClawPhase = new(), rClawPhase = new();
    FloatData time = new();
    FloatRect bounds = new(0, 0, 75, 48),crownBounds, grappleBounds;
    Sprite sprite = new(Assets.crabBody[0]), crownSprite = new(Assets.crown);
    Claw leftClaw;
    Claw rightClaw;



    CrabMovement movement;
    RectRender renderer, crownRender;

    EnemyDamage damager, crownDamager;
    EnemyWeapon[] patterns = {};
    public CrabBoss() {
        BoolData lJabbing = new(false), rJabbing = new(false);
        Vector2Data lVel = new(), rVel = new();

        leftClaw = new(true, lJabbing, bounds, lClawPhase, lVel);
        rightClaw = new(false, rJabbing, bounds, rClawPhase, rVel);
        renderer = new(sprite, bounds);
        crownRender = new(crownSprite, crownBounds);
        movement = new(leftClaw.Bounds, lVel, rightClaw.Bounds, rVel, lJabbing, rJabbing, bounds);
        damager = new(bounds, null, health, sprite, Assets.crabBody[1]);
        crownDamager = new(crownBounds, null, crownHealth, crownSprite, Assets.gunCrown[1]);
        

    }


    public override void Update(GameTime gameTime) {
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
                JabPhase(gameTime);
                break;
            case 3:
                CrownPhase(gameTime);
                break;
            case 4:
                BodyPhase(gameTime);
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
            patterns = new EnemyWeapon[] { new SpreadAlternating(bounds, rows: 6) };
            movement.movementState = Movements.rightJab;
        }
        if (!rightClaw.Alive) {
            rightClaw.end();
            phase.val++;
            leftClaw.startPhase3();
            patterns = new EnemyWeapon[] { new SpreadAlternating(bounds, rows: 6) };
            movement.movementState = Movements.leftJab;
        }

    }
    private void JabPhase(GameTime gameTime) {
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }
        if (leftClaw.Alive) {
            leftClaw.Update(gameTime);

        }
        else if (rightClaw.Alive) {
            rightClaw.Update(gameTime);
        }
        else {
            phase.val++;
            patterns = new EnemyWeapon[] { new SpreadAlternating(bounds, rows: 50, angle: 360) };

        }

    }
    private void CrownPhase(GameTime gameTime) {
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }

    }
    private void BodyPhase(GameTime gameTime) {
        damager.Update();
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
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
	private Vector2Data lVel;
	private Vector2Data rVel;
	public Movements movementState = Movements.enter;

    float jabAngle, jabSpeed = 200;

    float timer = 0, jabReturnDelay = 0.5f;
    public CrabMovement(FloatRect lClaw, Vector2Data lVel, FloatRect rClaw, Vector2Data rVel, BoolData lClawJab, BoolData rClawJab, FloatRect bounds) {
        this.lClaw = lClaw;
        this.rClaw = rClaw;
        this.rClawJab = rClawJab;
        this.lClawJab = lClawJab;
        this.lVel = lVel;
        this.rVel = rVel;
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
                lVel.val.X = (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                rVel.val.X = (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
                break;

            case Movements.idle:

                lVel.val.Y += (float)(Math.Sin(timer) * gameTime.ElapsedGameTime.TotalSeconds * 10);
                rVel.val.Y += (float)(Math.Cos(timer) * gameTime.ElapsedGameTime.TotalSeconds * 10);
                lVel.val.X += (float)(Math.Cos(timer) * gameTime.ElapsedGameTime.TotalSeconds * 5);
                rVel.val.X += (float)(Math.Sin(timer) * gameTime.ElapsedGameTime.TotalSeconds * 5);
                break;
            case Movements.leftJab:
                Jab(gameTime, lClawJab, lClaw,lVel, new Vector2(bounds.x - 15, bounds.y + 25));
                break;
            case Movements.rightJab:
                Jab(gameTime, rClawJab, rClaw,rVel, new Vector2(bounds.Right + 15, bounds.y + 25));
                break;


        }
    }
    private void Jab(GameTime gameTime, BoolData jabbing, FloatRect bounds, Vector2Data vel, Vector2 returnPoint ) {
        if (jabbing.val) {
            vel.val.X += (float)(Math.Sin(jabAngle) * jabSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            vel.val.Y -= (float)(Math.Cos(jabAngle) * jabSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            timer = 0;
        }
        else {
            if (timer < jabReturnDelay) {
                return;
            }
            Vector2 positionDiff = returnPoint - bounds.Centre;
            vel.val.X += (float)(positionDiff.X / 3 * gameTime.ElapsedGameTime.TotalSeconds *
            gameTime.ElapsedGameTime.TotalSeconds * 100);
            vel.val.Y += (float)(positionDiff.Y / 3 * gameTime.ElapsedGameTime.TotalSeconds *
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

