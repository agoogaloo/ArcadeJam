using System;
using ArcadeJam;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Claw {
    public bool left, Alive = true, started = false;
    BoolData jabbing;

    Texture2D[] textures;
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

    public Claw(bool left,BoolData jabbing, FloatRect bodyBounds, IntData phase) {
        this.left = left;
        this.phase = phase;
        this.jabbing = jabbing;
        currentPattern = new WallAlternate(Bounds);
        
        this.bodyBounds = bodyBounds;
        if (left) {
            textures = Assets.clawL;
        }
        else {
            textures = Assets.clawR;
        }
        sprite = new(textures[0]);
        armSegs = new PointRender[armLocs.Length];
        for (int i = 0; i < armLocs.Length; i++) {
            Console.WriteLine(armLocs[i]);
            armSegs[i] = new(new Sprite(Assets.crabArm), armLocs[i], true);
        }

        renderer = new(sprite, Bounds);
        damager = new(Bounds, null, health, sprite, textures[2]);
        hitBoxVisualizer = new(Bounds);



    }

    public void Update(GameTime gameTime) {
        switch(phase.val){
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
       
        damager.Update();
        if(health.val<=0){
            Alive = false;
            Console.WriteLine("claw is gone");
        }
    }
    private void phase1(GameTime gameTime) {
         currentPattern.Update(gameTime);
        if(health.val<=100){
            currentPattern = new SpreadAlternating(Bounds);
            phase.val = 2;
        }


    }
    private void phase2(GameTime gameTime) {
         currentPattern.Update(gameTime);

    }
    private void phase3(GameTime gameTime){
        
        currentPattern.Update(gameTime);
        if(jabbing.val &&(Bounds.Centre.X<0||Bounds.Centre.Y<0||
            Bounds.Centre.X>ArcadeGame.gameWidth||Bounds.Centre.Y>ArcadeGame.gameHeight)){
                jabbing.val = false;
                currentPattern.fire();
            }
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
    public void startPhase3(){
        phase.val = 3;
        currentPattern = new Explosion(Bounds);
        
    }
}