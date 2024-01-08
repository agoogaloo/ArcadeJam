using System;
using ArcadeJam;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Claw : Node, IGrappleable {
    public bool left, started = false, grappled = false;
    BoolData jabbing;

    Texture2D[] openTextures, closedTextures;
    Sprite damageTex;
    IntData health = new(150), phase = new(0);
    public FloatRect Bounds { get; private set; } = new(0, 0, 15, 33);
    Vector2Data vel;
    FloatRect bodyBounds;
    Sprite sprite;
    RectRender renderer;
    RectVisualizer hitBoxVisualizer;

    Vector2Data[] armLocs = { new(), new(), new() };
    PointRender[] armSegs;


    EnemyDamage damager;
    Collision grappleCollision;
    EnemyWeapon currentPattern;



    public Claw(bool left, BoolData jabbing, FloatRect bodyBounds, IntData phase, Vector2Data vel) {
        this.left = left;
        this.phase = phase;
        this.jabbing = jabbing;
        currentPattern = new WallAlternate(Bounds);

        this.bodyBounds = bodyBounds;
        if (left) {
            openTextures = Assets.clawLOpen;
            closedTextures = Assets.clawL;
        }
        else {
            openTextures = Assets.clawROpen;
            closedTextures = Assets.clawR;
        }
        sprite = new(closedTextures[0]);
        damageTex = new(closedTextures[1]);
        armSegs = new PointRender[armLocs.Length];
        for (int i = 0; i < armLocs.Length; i++) {
            armSegs[i] = new(new Sprite(Assets.crabArm), armLocs[i], true);
        }

        renderer = new(sprite, Bounds);
        damager = new(Bounds, null, health, sprite, damageTex);
        this.vel = vel;
        grappleCollision = new(Bounds, this, "grapple");
        hitBoxVisualizer = new(Bounds);



    }

    public override void Update(GameTime gameTime) {
        switch (phase.val) {
            case 0:
                break;
            case 1:
                phase1(gameTime);
                break;
            case 2:
                phase2(gameTime);
                break;
            case 3:
                phase3(gameTime);
                break;

        }
        if (jabbing.val && phase.val == 3) {
            damageTex.texture = openTextures[1];
            sprite.texture = openTextures[0];
            Bounds.width = 21;
            Bounds.height = 15;
        }
        else if (grappled){
            sprite.texture = openTextures[1];

        }
        else {
            damageTex.texture = closedTextures[1];
            sprite.texture = closedTextures[0];
            Bounds.width = 15;
            Bounds.height = 33;
        }
        damager.Update();
        if (health.val <= 0) {
            Alive = false;
            Console.WriteLine("claw is gone");
        }
        if (!grappled) {
            Bounds.Location += vel.val;
        }
        vel.val = Vector2.Zero;
    }
    private void phase1(GameTime gameTime) {
        currentPattern.Update(gameTime);
        if (health.val <= 100) {
            currentPattern = new SpreadAlternating(Bounds);
            phase.val = 2;
        }


    }
    private void phase2(GameTime gameTime) {
        currentPattern.Update(gameTime);

    }
    private void phase3(GameTime gameTime) {

        currentPattern.Update(gameTime);
        if (jabbing.val && ((Bounds.Centre.X < 3 || Bounds.Centre.Y < 10 || Bounds.Centre.X > ArcadeGame.gameWidth - 3 ||
            Bounds.Centre.Y > ArcadeGame.gameHeight - 20) || Bounds.Intersects(ArcadeGame.player.Bounds))) {
            jabbing.val = false;
            currentPattern.fire();
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        Vector2 shoulderDiff = new();

        if (left) {
            shoulderDiff = bodyBounds.Location;
            shoulderDiff.X += 15;

        }
        else {
            shoulderDiff = bodyBounds.Location;
            shoulderDiff.X += bodyBounds.width - 15;
        }
        shoulderDiff.Y += Bounds.height / 2 + 15;
        shoulderDiff -= Bounds.Centre;
        for (int i = 0; i < armLocs.Length; i++) {
            armLocs[i].val = Bounds.Centre + i * shoulderDiff / (armLocs.Length);
            armLocs[i].val.Y -= Bounds.height / 2;

        }
        foreach (PointRender i in armSegs) {
            i.Draw(spriteBatch);
        }
        renderer.Draw(spriteBatch);
        hitBoxVisualizer.Draw(spriteBatch);


    }

    public void end() {
        damager.End();
    }
    public void startPhase3() {
        phase.val = 3;
        currentPattern = new Explosion(Bounds);

    }

    public void GrappleStun() {
        grappled = true;
        jabbing.val = false;
        Bounds.height = 20;
    }

    public void GrappleHit(int damage) {
        grappled = false;
        health.val -= damage;
    }
}