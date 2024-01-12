using System;
using ArcadeJam.Weapons;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class BombEnemy : Enemy {
    float timer = 99, bombDelay = 3, distance = 50, xOffset = 100;
    int cycle = 3;

    public BombEnemy(EnemyMovement movement, ScoreData score) : base(movement, Assets.bombEnemy, score) {
        Health.val = 80;
        weapon = new Nothing();
        bounds.width = 22;
        bounds.height = 13;
        grappleBounds.width = bounds.width;
        grappleBounds.height = bounds.height;
        killPoints = 100;
        grapplePoints = 150;

    }


	public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        timer+=(float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer >= bombDelay) {
            Vector2 destination = bounds.Centre;
            destination.Y+=distance;
            destination.X+=-xOffset/2+(xOffset/5*cycle);
            NodeManager.AddNode(new Mine(new MoveToPoint(bounds.Centre,destination,easing:1.5f),score));
            timer = 0;
            cycle++;
            if(cycle>=5){
                cycle = 0;
            }
        }
        if(!Alive){
            Mine mine = new Mine(new MoveToPoint(bounds.Centre,bounds.Centre,easing:100),score);
            mine.Health.val = 0;
            NodeManager.AddNode(mine);
        }
    }

}

public class Mine : Enemy {
    double deathTime = 0.25f;
    bool exploded = false;

    public Mine(EnemyMovement movement, ScoreData score) : base(movement, Assets.mine, score) {
        Health.val = 23;
        weapon = new Explosion(bounds,volleys:2);
        bounds.width = 7;
        bounds.height = 7;
        grappleBounds.width = 7;
        grappleBounds.height = 7;
        killPoints = 0;
        grapplePoints = 0;
    }
    public override void Update(GameTime gameTime) {
        sprite.texture = textures[0];
        damager.Update();
        movement.Update(gameTime);
        weapon.Update(gameTime);
        updateGrappleBounds();
        if (Health.val <= 0) {

            if (!exploded) {
                weapon.fire();
                NodeManager.AddNode(new ExplosionEffect(bounds.Centre, false));

            }
            exploded = true;
            deathTime -= gameTime.ElapsedGameTime.TotalSeconds;

        }
        if (deathTime <= 0) {
            Alive = false;
        }
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
        doRipples(gameTime);

    }
}

