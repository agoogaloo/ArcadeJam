using System;
using ArcadeJam.Weapons;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;


public class ShipBoss : Enemy {

    int phase = 0;
    float deathTime = 2, timer;
    ScoreData score;

    public ShipBoss(ScoreData score) : base(new MoveToPoint(new Vector2(75, -50), new Vector2(75, 50), easing: 2),
         Assets.shipBoss, score) {
        Health.val = 60;
        bounds.width = 23;
        bounds.height = 34;
        this.score = score;
        weapon = new Spiral(bounds);

        renderer = new(sprite, bounds);
    }

    public override void Update(GameTime gameTime) {

        if(doDeathExplosion(gameTime)){
            return;
        }

        movement.Update(gameTime);
        weapon.Update(gameTime);
        updateGrappleBounds();


        if (!grappleable && Health.val < ArcadeGame.player.grappleDamage.val) {
            grappleCollision.Readd();
            grappleable = true;
        }
        else if (grappleable && Health.val > ArcadeGame.player.grappleDamage.val) {
            grappleable = false;
            grappleCollision.Remove();
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
    public void phase1() {

    }
    public void phase2() {

    }
}



public class ShipBossMovement : EnemyMovement {

}

