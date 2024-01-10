using System;
using System.ComponentModel;
using System.IO;
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
    int[] phasePoints = { 300, 300, 200, 500 };
    int maxHealth;
    Vector2 crownVel = new(50, -50);
    bool grapplable = false;
    IntData health = new(350), crownHealth = new(200), phase = new(0), lClawPhase = new(), rClawPhase = new();
    float time = 0, deathTimer = 4;
    FloatRect bounds = new(-60, 0, 43, 30), crownBounds = new(-50, 0, 20, 20),
        grappleBounds = new(-20, 0, 17, 10);
    Sprite sprite = new(Assets.crabEnter), crownSprite = new(Assets.crown);
    Claw leftClaw;
    Claw rightClaw;



    CrabMovement movement;
    RectRender renderer, crownRender;
    RectVisualizer visualizer;

    EnemyDamage damager, crownDamager;
    Collision grappleCollision, crownGrapple;
    EnemyWeapon[] patterns = { };

    ScoreData score;
    FloatData progress;
    public CrabBoss(ScoreData score, FloatData progress) {
        this.score = score;
        this.progress = progress;
        progress.val = 1;        

        BoolData lJabbing = new(false), rJabbing = new(false);
        Vector2Data lVel = new(), rVel = new();

        leftClaw = new(true, lJabbing, bounds, lClawPhase, lVel);
        rightClaw = new(false, rJabbing, bounds, rClawPhase, rVel);
        renderer = new(sprite, bounds);
        crownRender = new(crownSprite, crownBounds);
        movement = new(leftClaw.Bounds, lVel, rightClaw.Bounds, rVel, lJabbing, rJabbing, bounds);

        crownDamager = new(crownBounds, null, crownHealth, crownSprite, Assets.gunCrown[1]);
        grappleCollision = new(grappleBounds, this, "grapple");
        crownGrapple = new(crownBounds, this, "grapple");
        visualizer = new(crownBounds);
        grappleCollision.Remove();
        crownGrapple.Remove();
        maxHealth = health.val+crownHealth.val+leftClaw.health.val+rightClaw.health.val;



    }


    public override void Update(GameTime gameTime) {
        movement.Update(gameTime);
        int currentHealth = health.val+Math.Max(0,crownHealth.val)+leftClaw.health.val+rightClaw.health.val;
        //Console.WriteLine(currentHealth+"/"+maxHealth+":"+(currentHealth/(float)maxHealth));
        progress.val = (float)currentHealth/maxHealth;
        if (deathTimer <= 0) {
            Alive = false;

        }
        if (health.val <= 0) {
            if (deathTimer == 4) {
                sprite.texture = Assets.angryCrabBody[0];
                score.addScore(phasePoints[3]);
                //NodeManager.AddNode(new ExplosionEffect(crownBounds.Centre, true));
                NodeManager.AddNode(new Ripple(new Vector2(crownBounds.x - 3f, crownBounds.y - 2), true));
                NodeManager.AddNode(new Ripple(new Vector2(crownBounds.Right + 2f, crownBounds.y - 2), false));

            }
            if(time>=0.2){
                Console.WriteLine("boom");
                time = 0;
                Random rand = new Random();
                Vector2 explosionLoc =new((float)rand.NextDouble()*bounds.width,(float)rand.NextDouble()*bounds.height); 
                NodeManager.AddNode(new ExplosionEffect(bounds.Location+explosionLoc, false));

            }
            deathTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            return;

        }


        switch (phase.val) {
            case 0:
                Enter(gameTime);
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

    private void Enter(GameTime gameTime) {

        if (bounds.y >= 1) {
            phase.val++;
            patterns = new EnemyWeapon[] { new AimedParallel(bounds, 2, rows: 3, seperation: 5,speed:90) };
            movement.movementState = Movements.idle;

            lClawPhase.val = 1;
            rClawPhase.val = 1;
            sprite.texture = Assets.crabBody;

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
            movement.timer = 0;
            score.addScore(phasePoints[0]);
        }
        if (!rightClaw.Alive) {

            rightClaw.end();
            phase.val++;
            leftClaw.startPhase3();
            patterns = new EnemyWeapon[] { new SpreadAlternating(bounds, rows: 6) };
            movement.movementState = Movements.leftJab;
            movement.timer = 0;
            score.addScore(phasePoints[0]);
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
            rightClaw.end();
            leftClaw.end();
            phase.val++;
            patterns = new EnemyWeapon[] { new SpreadAlternating(bounds, rows: 50, angle: 360),
            new SpreadAlternating(bounds, delay: -1, rows: 300, angle: 360, volleys: 2,speed:40) };
            score.addScore(phasePoints[1]);


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

            crownGrapple.Readd();
            crownSprite.texture = Assets.gunCrown[2];
            if (crownHealth.val > -100) {

                patterns[1].fire();
                patterns[0].fire();
            }

            crownHealth.val = -100;
        }



    }
    private void BodyPhase(GameTime gameTime) {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        sprite.texture = Assets.angryCrabBody[0];
        if (grapplable) {
            sprite.texture = Assets.angryCrabBody[2];
        }
        damager.Update();
        //updating crown
        crownBounds.Location += crownVel * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (crownBounds.Top <= 0 || crownBounds.Bottom >= ArcadeGame.gameHeight) {
            crownVel.Y *= -1;
            patterns[0].fire();
             Assets.shootSounds[2].CreateInstance();
            Assets.shootSounds[2].Play();
            crownBounds.y = Math.Max(crownBounds.y, 0);
            crownBounds.y = Math.Min(crownBounds.y, ArcadeGame.gameHeight);
        }
        if (crownBounds.Left <= 0 || crownBounds.Right >= ArcadeGame.gameWidth) {
            crownVel.X *= -1;
            patterns[0].fire();
            Assets.shootSounds[2].CreateInstance();
            Assets.shootSounds[2].Play();
            crownBounds.x = Math.Max(crownBounds.x, 0);
            crownBounds.x = Math.Min(crownBounds.x, ArcadeGame.gameWidth);
        }
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }
        if (grapplable && time > 2) {
            grapplable = false;
            grappleCollision.Remove();

        }

        if (time >= 5) {
            patterns[1].fire();
            grapplable = true;
            grappleCollision.Readd();
            Console.WriteLine("peeeew");
            time = 0;
            Assets.bigExplosion.CreateInstance();
            Assets.bigExplosion.Play();
        }
        Console.WriteLine(time);


    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(spriteBatch);
        if (deathTimer == 4) {
            crownRender.Draw(spriteBatch);
        }
        if (leftClaw.Alive) {
            leftClaw.Draw(gameTime, spriteBatch);
        }
        if (rightClaw.Alive) {
            rightClaw.Draw(gameTime, spriteBatch);
        }
        visualizer.bounds = bounds;
        visualizer.Draw(spriteBatch);
        visualizer.bounds = grappleBounds;
        visualizer.Draw(spriteBatch);



    }

    public void GrappleStun() {

    }

    public void GrappleHit(int damage) {
        if (phase.val == 3) {
            phase.val++;
            crownGrapple.Remove();
            crownSprite.texture = Assets.gunCrown[0];
            damager = new(bounds, null, health, sprite, Assets.angryCrabBody[1]);
            patterns = new EnemyWeapon[] { new Spread(crownBounds, delay: -1, shots: 10, angle: 360, volleys: 0)
            ,new SpreadAlternating(bounds, delay: -1, rows: 300, angle: 360, volleys: 2,speed:40),
            new CirclePath(bounds),new CirclePath(bounds,angle:180-60)
             ,new CirclePath(bounds,angle:180+60)};
            grappleBounds.Centre = bounds.Centre;
            grappleBounds.y = bounds.Bottom;
            score.addScore(phasePoints[2]);
            NodeManager.AddNode(new ExplosionEffect(crownBounds.Centre));

        }
        else if (phase.val == 4) {
            grapplable = false;
            grappleCollision.Remove();
            health.val -= damage;
            time = 0;
            NodeManager.AddNode(new ExplosionEffect(grappleBounds.Centre, true, false));
            Assets.bigExplosion.CreateInstance();
            Assets.bigExplosion.Play();

        }
    }
    public override void End() {
        base.End();
        leftClaw.end();
        rightClaw.end();
        if (damager != null) {
            damager.End();
        }
        if (crownDamager != null) {
            crownDamager.End();
        }
        crownGrapple.Remove();
        grappleCollision.Remove();
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

    public float timer = 0, jabReturnDelay = 0.5f;
    public CrabMovement(FloatRect lClaw, Vector2Data lVel, FloatRect rClaw, Vector2Data rVel, BoolData lClawJab, BoolData rClawJab, FloatRect bounds) {
        this.lClaw = lClaw;
        this.rClaw = rClaw;
        this.rClawJab = rClawJab;
        this.lClawJab = lClawJab;
        this.lVel = lVel;
        this.rVel = rVel;
        this.bounds = bounds;
        bounds.Centre = new Vector2(ArcadeGame.gameWidth / 2, -100);
        lClaw.y = -30;
        rClaw.y = -30;
        lClaw.x = 25 - lClaw.Centre.X / 2;
        rClaw.x = ArcadeGame.gameWidth - 25 - rClaw.Centre.X / 2;


    }
    public void Update(GameTime gameTime) {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch (movementState) {
            case Movements.enter:
                if (lClaw.y < 30) {
                    lClaw.y += (float)gameTime.ElapsedGameTime.TotalSeconds * 120;
                    rClaw.y += (float)gameTime.ElapsedGameTime.TotalSeconds * 120;
                }
                float bodyOffset = lClaw.y - bounds.y;
                bounds.y += (float)(bodyOffset / 4 * gameTime.ElapsedGameTime.TotalSeconds *
            gameTime.ElapsedGameTime.TotalSeconds * 300);

                break;

            case Movements.idle:

                lVel.val.Y += (float)(Math.Sin(timer) * gameTime.ElapsedGameTime.TotalSeconds * 7);
                rVel.val.Y += (float)(Math.Cos(timer) * gameTime.ElapsedGameTime.TotalSeconds * 7);
                lVel.val.X += (float)(Math.Cos(timer) * gameTime.ElapsedGameTime.TotalSeconds * 3);
                rVel.val.X += (float)(Math.Sin(timer) * gameTime.ElapsedGameTime.TotalSeconds * 3);
                break;
            case Movements.leftJab:

                Jab(gameTime, lClawJab, lClaw, lVel, new Vector2(bounds.x - 5, bounds.y + 15));

                break;
            case Movements.rightJab:

                Jab(gameTime, rClawJab, rClaw, rVel, new Vector2(bounds.Right + 5, bounds.y + 15));

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
            vel.val.X += (float)(positionDiff.X / 2 * gameTime.ElapsedGameTime.TotalSeconds *
            gameTime.ElapsedGameTime.TotalSeconds * 80);
            vel.val.Y += (float)(positionDiff.Y / 2 * gameTime.ElapsedGameTime.TotalSeconds *
             gameTime.ElapsedGameTime.TotalSeconds * 80);
            if (positionDiff.Length() < 10) {
                jabbing.val = true;
                Vector2 angleCartesian = ArcadeGame.player.Bounds.Centre - bounds.Centre;
                jabAngle = (float)Math.Atan(angleCartesian.Y / angleCartesian.X);
                if (angleCartesian.X > 0) {
                    jabAngle += (float)Math.PI;
                }
                jabAngle -= (float)Math.PI / 2;
                Assets.grappleShoot.CreateInstance();
                Assets.grappleShoot.Play();

            }


        }



    }
}

