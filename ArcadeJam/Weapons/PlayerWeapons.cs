using System;
using System.Data;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Weapons;

public abstract class PlayerWeapon {
    protected float timeLeft, speed;
    protected FloatData reloadTime;
    protected BoolData focused;

    protected FloatRect pos;
    protected bool shooting = false;

    public PlayerWeapon(FloatRect pos, BoolData focused, float delay = 1, float speed = 60) :
            this(pos, focused, new FloatData(delay), speed) { }

    public PlayerWeapon(FloatRect pos, BoolData focused, FloatData reloadTime, float speed = 300) {
        this.pos = pos;
        this.reloadTime = reloadTime;
        this.speed = speed;
        this.focused = focused;
    }

    public virtual void Update(GameTime gameTime) {

        if (timeLeft < 0 && shooting) {
            timeLeft = reloadTime.val;
            if (focused.val) {
                FocusShoot();
            }
            else {
                NormalShoot();
            }
            
        }
        else {
            timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        shooting = false;
    }
    public void Use() {
        shooting = true;


    }
    protected abstract void NormalShoot();
    protected abstract void FocusShoot();
    protected static void FireAtAngle(float angle, float speed, Vector2 startPos, int damage = 1) {
        angle *= (float)(Math.PI / 180); //converting to radians
        float y = -(float)(Math.Cos(angle) * speed);
        float x = (float)(Math.Sin(angle) * speed);
        NodeManager.AddNode(new PlayerBullet(new Vector2Data(x, y), startPos, damage));

    }
}


public class Level1Gun : PlayerWeapon {
    public Level1Gun(FloatRect pos, BoolData focused) : base(pos, focused, 0f, 275) {
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        if (focused.val) {
            reloadTime.val = 0.2f;
        }
        else {
            reloadTime.val = 0.3f;
        }
    }

    protected override void FocusShoot() {

        Vector2 shootPos = pos.Location;
        shootPos.X -=1;
        shootPos.Y+=5;
		FireAtAngle(0, speed, shootPos,3);
        shootPos.X = pos.Right;
		FireAtAngle(0, speed, shootPos,3);


    }

    protected override void NormalShoot() {
        Vector2 shootPos = new Vector2(pos.Centre.X, pos.Top);
        shootPos.Y+=5;
		FireAtAngle(0, speed, shootPos,3);
		FireAtAngle(-30, speed, shootPos,3);
		FireAtAngle(30, speed, shootPos,3);

    }

}

public class Level2Gun : PlayerWeapon {
    public Level2Gun(FloatRect pos, BoolData focused) : base(pos, focused, 0f, 300) {
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        if (focused.val) {
            reloadTime.val = 0.2f;
        }
        else {
            reloadTime.val = 0.3f;
        }
    }

    protected override void FocusShoot() {

        Vector2 shootPos = pos.Location;
        shootPos.X -=3.5f;
        
		FireAtAngle(0, speed, shootPos,2);
        
        shootPos.X = pos.Right+3.5f;
		FireAtAngle(0, speed, shootPos,2);
        shootPos.X = pos.Centre.X;
        shootPos.Y-=5;
		FireAtAngle(0, speed, shootPos,3);
        


    }

    protected override void NormalShoot() {
        Vector2 shootPos = pos.Location;
        shootPos.X -=1.5f;
		FireAtAngle(-2.5f, speed, shootPos, 3);
        shootPos.X = pos.Right+1.5f;
		FireAtAngle(2.5f, speed, shootPos, 3);
        shootPos.X = pos.Centre.X;
		FireAtAngle(-30, speed, shootPos, 2);
		FireAtAngle(30, speed, shootPos, 2);

    }

}

public class Level3Gun : PlayerWeapon {
    public Level3Gun(FloatRect pos, BoolData focused) : base(pos, focused, 0f, 300) {
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        if (focused.val) {
            reloadTime.val = 0.15f;
        }
        else {
            reloadTime.val = 0.3f;
        }
    }

    protected override void FocusShoot() {

        Vector2 shootPos = pos.Location;
        shootPos.X -=5.5f;
        shootPos.Y+=3;
       
		FireAtAngle(0, speed, shootPos,1);
        shootPos.X = pos.Right+5.5f;
		FireAtAngle(0, speed, shootPos,1);

        shootPos.X =pos.x-2.5f;
        shootPos.Y-=3;
       
		FireAtAngle(0, speed, shootPos,1);
        shootPos.X = pos.Right+2.5f;
		FireAtAngle(0, speed, shootPos,1);
        
        shootPos.X = pos.Centre.X;
        shootPos.Y-=5;
		NodeManager.AddNode(new PlayerSmearBullet(speed, shootPos, 2));


    }

    protected override void NormalShoot() {
        Vector2 shootPos = new Vector2(pos.Centre.X, pos.Top);
        //shootPos.Y+=5;
		NodeManager.AddNode(new PlayerSmearBullet(speed, shootPos, 3));
		FireAtAngle(15, speed, shootPos,2);
		FireAtAngle(-15, speed, shootPos,2);
		FireAtAngle(30, speed, shootPos,2);
		FireAtAngle(-30, speed, shootPos,2);
		FireAtAngle(-45, speed, shootPos,1);
		FireAtAngle(45, speed, shootPos,1);

    }

}