using System;
using System.Reflection.Metadata.Ecma335;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Weapons;

public abstract class EnemyWeapon {
    protected FloatData delay;
    protected FloatRect pos;

    protected double timeLeft = 0;
    protected float speed;

    public EnemyWeapon(FloatRect pos, float delay = 1, float speed = 60) : this(pos, new FloatData(delay), speed) { }
    public EnemyWeapon(FloatRect pos, FloatData delay, float speed = 60) {
        this.delay = delay;
        this.speed = speed;
        this.pos = pos;
        timeLeft = delay.val;
    }

    public virtual void Update(GameTime gameTime) {
        timeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
        if (timeLeft < 0) {
            timeLeft = delay.val+timeLeft;
            Shoot();
        }
    }
    protected abstract void Shoot();
    protected void FireAtAngle(float angle, float speed) {
        angle *= (float)(Math.PI / 180); //converting to radians
        float y = -(float)(Math.Cos(angle) * speed);
        float x = (float)(Math.Sin(angle) * speed);
        NodeManager.AddNode(new EnemyBullet(new Vector2Data(x, y), pos.Centre));

    }

}

public class Straight : EnemyWeapon {
    public Straight(FloatRect pos, float delay = 1) : base(pos, delay) {
    }

    protected override void Shoot() {
        FireAtAngle(180, speed);
    }
}
public class Tripple : EnemyWeapon {
    private float angle;
    public Tripple(FloatRect pos, float delay = 1, float angle = 30, float speed = 60) : base(pos, delay, speed) {
        this.angle = angle;
    }

    protected override void Shoot() {
        FireAtAngle(180, speed);
        FireAtAngle(180-angle, speed);
        FireAtAngle(180+angle, speed);
    }
}

public class Spiral : EnemyWeapon {
    private float angle;
    private float spinSpeed;
    int prongs;
    public Spiral(FloatRect pos, float delay = 0.25f, int prongs = 4, float spinSpeed = 50, float bulletSpeed = 60) : base(pos, delay, bulletSpeed) {
        angle = 0;
        this.spinSpeed = spinSpeed;
        this.prongs = prongs;

    }
    public override void Update(GameTime gameTime) {
		base.Update(gameTime);
        angle+=(float)(spinSpeed*gameTime.ElapsedGameTime.TotalSeconds);
	}

    protected override void Shoot() {
        for (int i = 1; i <= prongs; i++) {
            FireAtAngle(angle+i*360/prongs, speed);
        }
    }

}

