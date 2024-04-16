using System;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;


public class ShipBoss : Enemy {

    int phase = 1;
    float deathTime = 2, timer, switchAttackTimer = 0;
    bool leftSideShooting = true, pastEdge = false;


    EnemyWeapon[] patterns;
    FloatRect patternBounds = new(0, 0, 2, 2);

    Mine lMine, rMine;

    int[] phasePoints = { 200, 200, 300 };


    public ShipBoss(ScoreData score) : base(new ShipPhase1Movement(), Assets.shipBoss, score) {
        Health.val = 535;
        bounds.width = 34;
        bounds.height = 23;
        grappleBounds.width = 8;
        grappleBounds.height = 9;
        this.score = score;
        killPoints = phasePoints[2];
        patterns = new EnemyWeapon[]{new Straight(patternBounds, 0.2f, 30, -90-60),
        new Straight(patternBounds, 0.2f,30, -90+60)};

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
                phaseTrans(gameTime);
                break;
            case 3:
                phase3(gameTime);
                break;
            case 4:
                phase4(gameTime);
                break;

        }
        foreach (EnemyWeapon i in patterns) {
            i.Update(gameTime);
        }


        movement.Update(gameTime);


        sprite.texture = textures[0];
        if (grappleable && leftSideShooting) {
            sprite.texture = textures[2];

        }
        else if (grappleable && !leftSideShooting) {
            sprite.texture = textures[3];

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
                Vector2 loc = bounds.Centre;
                loc.Y = bounds.Top;
                score.addScore(500, loc);
                timer = 0;

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
    private void phase1(GameTime gameTime) {
        patternBounds.Centre = bounds.Centre;
        patternBounds.x = bounds.Right + 10;
        //edge of screen attack
        Console.WriteLine(patternBounds.Right);
        if (patternBounds.Right >= ArcadeGame.gameWidth - 7) {
            pastEdge = true;
            NodeManager.AddNode(new BigShot(new Vector2Data(new Vector2(0, 90)), patternBounds.Centre));
            NodeManager.AddNode(new BigShot(new Vector2Data(new Vector2(0, -90)), patternBounds.Centre));
        }
        if (Health.val <= 500) {
            phase++;
            movement = new ShipPhaseTransMovement();
            movement.Init(bounds, vel);
            
            patterns = new EnemyWeapon[] { };
            score.addScore(phasePoints[0], bounds.Centre);
        }

    }
    private void phaseTrans(GameTime gameTime) {
         patternBounds.Centre = bounds.Centre;
        patternBounds.x = bounds.Right + 10;
         
        if (Math.Abs(bounds.Centre.X - 75) < 15 && bounds.y == 15) {
            phase++;
            patterns = new EnemyWeapon[] { new Spread(patternBounds, shots: 7, delay: -1, angle: 65) };
            movement = new MoveToPoint(bounds.Centre, new Vector2(75, 20 + bounds.height / 2));
            movement.Init(bounds, vel);

        }

    }

    private void phase3(GameTime gameTime) {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        patternBounds.y = bounds.y + 17;

        if (timer >= 2) {
            leftSideShooting = !leftSideShooting;
            if (leftSideShooting) {
                patternBounds.x = bounds.x + 6.5f;
            }
            else {
                patternBounds.x = bounds.x + 19.5f;
            }
            mineshoot();
            patterns[0].fire();
            timer = 0;
        }

        patternBounds.Centre = bounds.Centre;
        if (Health.val <= 245) {
            phase++;
            timer = 1;
            Vector2 loc = bounds.Centre;
            loc.Y = bounds.Top;
            score.addScore(phasePoints[1], loc);
            patterns = new EnemyWeapon[]{new Spread(patternBounds, delay:-1,shots:5,angle:40,speed:50),
            new Spread(patternBounds, delay:-1,shots:2,speed:70),

           };
        }

        if (leftSideShooting) {
            patternBounds.x = bounds.x + 6.5f;
        }
        else {
            patternBounds.x = bounds.x + 19.5f;
        }


    }
    private void mineshoot() {
        if ((leftSideShooting && (lMine == null || !lMine.Alive)) || (!leftSideShooting && (rMine == null || !rMine.Alive))) {
            Vector2 start = new Vector2(bounds.x+16, patternBounds.y+5);
            if (leftSideShooting) {
                Vector2 destination = new Vector2(patternBounds.Centre.X - 10, patternBounds.Centre.Y + 40);
                lMine = new Mine(new MoveToPoint(start, destination), score);
                NodeManager.AddNode(lMine);
            }
            else {
                Vector2 destination = new Vector2(patternBounds.Centre.X + 12, patternBounds.Centre.Y + 40);
                rMine = new Mine(new MoveToPoint(start, destination), score);
                NodeManager.AddNode(rMine);
            }

        }
        else {
            NodeManager.AddNode(new BigShot(new Vector2Data(new Vector2(0, 80)), patternBounds.Centre));

        }
    }
    private void phase4(GameTime gameTime) {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        switchAttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        patternBounds.y = bounds.Bottom;
        grappleBounds.y = bounds.Bottom;

        if (timer >= 2) {
            timer = -20;
            patterns[0].fire();
            leftSideShooting = !leftSideShooting;
            grappleCollision.Readd();
            grappleable = true;

        }

        if (switchAttackTimer > 2.75) {
            timer = 0;
            switchAttackTimer = 0;

            patterns[1].fire();
            grappleCollision.Remove();
            grappleable = false;

        }
        if (leftSideShooting) {

            patternBounds.x = bounds.x + 6.5f;
            grappleBounds.x = bounds.x + 6.5f - grappleBounds.width / 2;

        }
        else {
            patternBounds.x = bounds.x + 19.5f;
            grappleBounds.x = bounds.x + 19.5f - grappleBounds.width / 2;
        }

    }

    public override void End() {
        base.End();
        if (ArcadeGame.player.lives.val < 5) {
            ArcadeGame.player.lives.val++;
        }
        else {
            score.addScore(2500, bounds.Centre);
        }
        Assets.lifeGet.Play();
    }
    public override void GrappleHit(int damage) {
        Health.val -= damage;
        stunned = false;
        switchAttackTimer = 4;
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
        vel.val.X += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60f);
        if (bounds.x > ArcadeGame.gameWidth + 30) {
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
public class ShipPhaseTransMovement : EnemyMovement {

    public override void Init(FloatRect bounds, Vector2Data vel) {
        base.Init(bounds, vel);
    }
    public override void Update(GameTime gameTime) {
        velMovement.Update(gameTime);
        if (Math.Abs(bounds.Centre.X - 75) < 15 && bounds.y == 15) {
            return;
        }
        vel.val.X += (float)(gameTime.ElapsedGameTime.TotalSeconds * 45f);
        if (bounds.x > ArcadeGame.width + 50) {

            bounds.y = 15;
            bounds.x = -30;

            vel.val = new(0, 0);
        }
    }
}



