using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
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
    Vector2 crownVel = new(50, 50);
    IntData health = new(150), crownHealth = new(100), phase = new(0), lClawPhase = new(), rClawPhase = new();
    FloatData time = new();
    FloatRect bounds = new(0, 0, 43, 30), crownBounds = new(0, 0, 20, 20),
        grappleBounds = new(0, 0, 10, 10);
    Sprite sprite = new(Assets.crabBody[0]), crownSprite = new(Assets.crown);
    Claw leftClaw;
    Claw rightClaw;



    CrabMovement movement;
    RectRender renderer, crownRender;
    RectVisualizer visualizer;

    EnemyDamage damager, crownDamager;
    Collision grappleCollision, crownGrapple;
    EnemyWeapon[] patterns = { };
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
        grappleCollision = new(grappleBounds, this, "grapple");
        crownGrapple = new(crownBounds, this, "grapple");
        visualizer = new(crownBounds);


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
            phase.val = 3;
            patterns = new EnemyWeapon[] { new AimedParallel(bounds, 1.6f) };
            movement.movementState = Movements.idle;

            lClawPhase.val = 1;
            rClawPhase.val = 1;

        }
        crownBounds.Centre = bounds.Centre;
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
        crownBounds.Centre = bounds.Centre;

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
        crownBounds.Centre = bounds.Centre;

    }
    private void CrownPhase(GameTime gameTime) {
        crownBounds.Centre = bounds.Centre;
        crownSprite.texture = Assets.gunCrown[0];
        crownDamager.Update();
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }
        if (crownHealth.val <= 0) {
            crownSprite.texture = Assets.gunCrown[2];

        }



    }
    private void BodyPhase(GameTime gameTime) {
        damager.Update();
        crownBounds.Location += crownVel * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (crownBounds.Top <= 0 || crownBounds.Bottom >= ArcadeGame.gameHeight) {
            crownVel.Y *= -1;
            patterns[0].fire();
            crownBounds.y = Math.Max(crownBounds.y, 0);
            crownBounds.y = Math.Min(crownBounds.y, ArcadeGame.gameHeight);
        }
        if (crownBounds.Left <= 0 || crownBounds.Right >= ArcadeGame.gameWidth) {
            crownVel.X *= -1;
            patterns[0].fire();
            crownBounds.x = Math.Max(crownBounds.x, 0);
            crownBounds.x = Math.Min(crownBounds.x, ArcadeGame.gameWidth);
        }
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(spriteBatch);
        crownRender.Draw(spriteBatch);
        if (leftClaw.Alive) {
            leftClaw.Draw(gameTime, spriteBatch);
        }
        if (rightClaw.Alive) {
            rightClaw.Draw(gameTime, spriteBatch);
        }
        visualizer.Draw(spriteBatch);



    }

    public void GrappleStun() {

    }

    public void GrappleHit(int damage) {
        if (phase.val == 3) {
            phase.val++;
            patterns = new EnemyWeapon[] { new Spread(crownBounds, delay: 99999999, shots: 20, angle: 360, volleys: 0)
            ,new SpreadAlternating(bounds, delay: 5, rows: 300, angle: 360, volleys: 3)};

        }
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
        bounds.y = 7;
        lClaw.y = 30;
        rClaw.y = 30;
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
                Jab(gameTime, lClawJab, lClaw, lVel, new Vector2(bounds.x - 15, bounds.y + 25));
                break;
            case Movements.rightJab:
                Jab(gameTime, rClawJab, rClaw, rVel, new Vector2(bounds.Right + 15, bounds.y + 25));
                break;




        }
    }
    private void Jab(GameTime gameTime, BoolData jabbing, FloatRect bounds, Vector2Data vel, Vector2 returnPoint) {
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

