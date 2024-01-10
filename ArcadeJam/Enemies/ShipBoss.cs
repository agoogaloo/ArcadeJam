using System;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;


public class ShipBoss : Enemy {

    int phase = 1;
    float deathTime = 2, timer;
    ScoreData score;

    EnemyWeapon[] patterns;
    FloatRect patternBounds = new(0, 0, 2, 2);

    public ShipBoss(ScoreData score) : base(new ShipPhase1Movement(), Assets.shipBoss, score) {
        Health.val = 60;
        bounds.width = 23;
        bounds.height = 34;
        this.score = score;
        patterns = new EnemyWeapon[]{new Straight(patternBounds, 0.15f, 30, -90-60),
        new Straight(patternBounds, 0.15f,30, -90+60)};

        renderer = new(sprite, bounds);
    }

    public override void Update(GameTime gameTime) {

        if (doDeathExplosion(gameTime)) {
            return;
        }
        switch (phase) {
            case 1:
                phase1(gameTime);
                break;
            case 2:
                phase2(gameTime);
                break;
        }


        movement.Update(gameTime);
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }

        sprite.texture = textures[0];
        if (grappleable) {
            sprite.texture = textures[2];

        }
        damager.Update();



    }
    public bool doDeathExplosion(GameTime gameTime) {
        //death explosions
        if (deathTime <= 0) {
            Alive = false;

        }
        if (Health.val <= 0) {
            if (deathTime == 2) {
                sprite.texture = Assets.shipBoss[0];
                score.addScore(500);

            }
            if (timer >= 0.2) {
                Console.WriteLine("boom");
                timer = 0;
                Random rand = new Random();
                Vector2 explosionLoc = new((float)rand.NextDouble() * bounds.width, (float)rand.NextDouble() * bounds.height);
                NodeManager.AddNode(new ExplosionEffect(bounds.Location + explosionLoc, false));

            }
            deathTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            return true;

        }
        return false;
    }
    public void phase1(GameTime gameTime) {
        patternBounds.Centre = bounds.Centre;
        patternBounds.x = bounds.Right + 10;

    }
    public void phase2(GameTime gameTime) {

    }
    public override void End() {
        base.End();
        if (ArcadeGame.player.lives.val < 5) {
            ArcadeGame.player.lives.val++;
        }
        else {
            score.addScore(3000);
        }
    }
}



public class ShipPhase1Movement : EnemyMovement {
    int lane = 0;
    public override void Init(FloatRect bounds, Vector2Data vel) {
        base.Init(bounds, vel);
        bounds.x = -40;
        bounds.y = 00;
        vel.val = new(100, 0);
    }
    public override void Update(GameTime gameTime) {
        velMovement.Update(gameTime);
        vel.val.X += (float)(gameTime.ElapsedGameTime.TotalSeconds *60f);
        if (bounds.x > ArcadeGame.width + 50) {
            lane++;
            if (lane >= 4) {
                lane = 0;
            }
            bounds.y = 5 + 15 * lane;
            bounds.x = -30;
            vel.val = new(0, 0);
        }
    }


}

