using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class CampCrab : Node {
    Sprite sprite = new(Assets.crabBody), crownSprite = new(Assets.crown),

    lClawSprite = new(Assets.clawL[0]), rClawSprite = new(Assets.clawR[0]),
    armSprite = new(Assets.crabArm);
    private RectRender renderer, crownRender, lClawRender, rClawRender;

    private IntData levelBonus;
    private FloatRect bounds = new(0, 0, 55, 29), crownBounds = new(0, 0, 55, 30),
    lClawBounds = new(15, 150, 15, 33), rClawBounds = new(115, 150, 15, 33);
    private float speed = 3.5f, returnSpeed = 90;
    public bool use = true, resetting = false, angry = false;

    public int startScore = 0;

    private RectVisualizer boundsVis, lCVis, rCVis;

    private float timer = 0;

    Vector2Data[] armLocs = { new(), new(), new(), new(), new(), new(), new(), new(), new() };
    PointRender[] armSegs;

    private Collision collision, lCollision, rCollision;
    private List<Node> collisions = new();

    private EnemyWeapon pattern = null;



    public CampCrab(IntData levelTime) {
        this.levelBonus = levelTime;

        bounds.Centre = new Vector2(ArcadeGame.gameWidth / 2, 175 + bounds.height / 2);
        crownBounds.Centre = new Vector2(ArcadeGame.gameWidth / 2, 174.5f);


        collision = new(bounds, this, "enemy", collisions);
        lCollision = new(lClawBounds, this, "enemy", collisions);
        rCollision = new(rClawBounds, this, "enemy", collisions);



        boundsVis = new(bounds);
        lCVis = new(lClawBounds);
        rCVis = new(rClawBounds);

        renderer = new(sprite, bounds, true);
        lClawRender = new(lClawSprite, lClawBounds, true);
        rClawRender = new(rClawSprite, rClawBounds, true);


        crownRender = new(crownSprite, crownBounds);

        armSegs = new PointRender[armLocs.Length];
        for (int i = 0; i < armLocs.Length; i++) {
            armSegs[i] = new(armSprite, armLocs[i], true);
        }

    }

    public override void Update(GameTime gameTime) {



        if (use && levelBonus.val <= startScore && !resetting) {

            //Console.WriteLine("the crabs are coming!");
            bounds.y -= (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);

            lClawBounds.y -= (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);
            rClawBounds.y -= (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);

            lClawBounds.y += (float)(Math.Sin(timer) * gameTime.ElapsedGameTime.TotalSeconds * 10);
            rClawBounds.y += (float)(Math.Cos(timer) * gameTime.ElapsedGameTime.TotalSeconds * 10);
            lClawBounds.x += (float)(Math.Cos(timer) * gameTime.ElapsedGameTime.TotalSeconds * 16);
            rClawBounds.x += (float)(Math.Sin(timer) * gameTime.ElapsedGameTime.TotalSeconds * 16);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds * 2f;
            if (bounds.y <= 90) {
                if (bounds.y <= 50) {
                    resetting = true;
                    pattern.delay.val/=1.5f;
                }
                if (pattern == null) {

                    crownSprite.texture = Assets.gunCrown[0];
                    pattern = new SpreadAlternating(crownBounds, rows: 40, angle: 360, delay: 2.5f);
                    pattern.timeLeft = 3;
                }
            }
            if (pattern != null) {
                pattern.Update(gameTime);
            }
        }

        else {
            resetting = false;
            if (bounds.y < 175) {
                bounds.y += (float)(returnSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                resetting = true;
            }
            if (lClawBounds.y < 150) {
                float lOffset = lClawBounds.y - bounds.y;

                lClawBounds.y -= (float)(lOffset / 5 * gameTime.ElapsedGameTime.TotalSeconds *
            gameTime.ElapsedGameTime.TotalSeconds * 300);
                resetting = true;

            }
            if (rClawBounds.y < 150) {

                float rOffset = lClawBounds.y - bounds.y;

                rClawBounds.y -= (float)(rOffset / 5 * gameTime.ElapsedGameTime.TotalSeconds *
                gameTime.ElapsedGameTime.TotalSeconds * 300);
                resetting = true;
            }
        }
        crownBounds.y = bounds.y - 2;

        collision.Update(new string[] { "playerBullet" });
        foreach (Node i in collisions) {
            if (i is PlayerBullet b) {
                b.OnHit();

            }
        }
        lCollision.Update(new string[] { "playerBullet" });
        foreach (Node i in collisions) {
            if (i is PlayerBullet b) {
                b.OnHit();

            }
        }
        rCollision.Update(new string[] { "playerBullet" });
        foreach (Node i in collisions) {
            if (i is PlayerBullet b) {
                b.OnHit();

            }
        }


    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(spriteBatch, gameTime);
        crownRender.Draw(spriteBatch, gameTime);

        drawArms(spriteBatch, lClawBounds, true);
        drawArms(spriteBatch, rClawBounds, false);
        lClawRender.Draw(spriteBatch, gameTime);
        rClawRender.Draw(spriteBatch, gameTime);
        boundsVis.Draw(spriteBatch, gameTime);
        lCVis.Draw(spriteBatch, gameTime);
        rCVis.Draw(spriteBatch, gameTime);


        //spriteBatch.Draw(sprite.texture, new Vector2(bounds.x, bounds.y), Color.White);
    }

    private void drawArms(SpriteBatch spriteBatch, FloatRect Bounds, bool left) {
        Vector2 shoulderDiff = bounds.Centre;
        if (left) {
            shoulderDiff.X -= 15;
        }
        else {

            shoulderDiff.X += 15;
        }
        shoulderDiff.Y -= 25;
        shoulderDiff -= Bounds.Centre;
        //drawing arm
        for (int i = 0; i < armLocs.Length; i++) {
            armLocs[i].val = Bounds.Centre + i * shoulderDiff / (armLocs.Length);
            armLocs[i].val.Y += Bounds.height / 2;

        }
        armSprite.texture = Assets.crabArm;
        foreach (PointRender i in armSegs) {

            i.Draw(spriteBatch);
        }
        //drawing hinges
        for (int i = 0; i < armLocs.Length; i += 3) {
            armLocs[i].val = Bounds.Centre + i * shoulderDiff / (armLocs.Length);
            armLocs[i].val.Y += Bounds.height / 2;

        }
        armSprite.texture = Assets.crabHinge;
        for (int i = 0; i < armSegs.Length; i += 3) {
            armSegs[i].Draw(spriteBatch);
        }


    }

    public void onHit() {
        resetting = true;
    }

}