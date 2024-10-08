using System;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ArcadeJam.Weapons;

public abstract class EnemyWeapon {
	public FloatData delay;
	protected FloatRect pos;

	public double timeLeft = 0, volleyDelay = 0.05;
	protected float speed;
	protected int volleys, volliesLeft;
	public SoundEffectInstance sound = Assets.shootSounds[1];
	protected static Random rand = new();

	public EnemyWeapon(FloatRect pos, float delay = 1, float speed = 60, int volleys = 1, SoundEffectInstance sound = null) :
		this(pos, new FloatData(delay), speed, volleys, sound) { }
	public EnemyWeapon(FloatRect pos, FloatData delay, float speed = 60, int volleys = 1, SoundEffectInstance sound = null) {

		//timeLeft = delay.val;
		this.delay = delay;
		this.speed = speed;
		this.pos = pos;
		this.volleys = volleys;
		this.volliesLeft = volleys - 1;
		if (delay.val == -1) {
			delay.val = 999999999999;
			timeLeft = delay.val;
		}
		if (sound != null) {
			this.sound = sound;
		}
		Console.WriteLine("shoot sound:" + sound);
	}

	public virtual void Update(GameTime gameTime) {
		timeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
		if (timeLeft < 0) {

			if (volliesLeft <= 0) {
				volliesLeft = volleys;
				timeLeft = delay.val;
			}
			else {
				timeLeft = volleyDelay;
			}
			if (volliesLeft == volleys - 1 || volleys == 1) {
				newVolley();
			}
			Shoot();
			volliesLeft--;

		}
	}
	protected abstract void Shoot();
	protected virtual void newVolley() {
		// sound.CreateInstance();
		sound.Pitch = (float)(rand.NextDouble() - 0.5) * 0.3f;
		sound.Stop();
		sound.Play();
	}
	protected void FireAtAngle(float angle, float speed) {
		FireAtAngle(angle, speed, pos.Centre);
	}

	protected void FireAtAngle(float angle, float speed, Vector2 loc) {
		angle *= (float)(Math.PI / 180); //converting to radians
		float x = (float)(Math.Sin(angle) * speed);
		float y = -(float)(Math.Cos(angle) * speed);
		NodeManager.AddNode(new EnemyBullet(new Vector2Data(x, y), loc));

	}
	public void fire() {
		timeLeft = 0;
		volliesLeft = volleys;
	}

}
public class Nothing : EnemyWeapon {
	public Nothing() : base(null, 9999999, 0) {
	}

	protected override void Shoot() {

	}
}
public class AimedParallel : EnemyWeapon {
	int rows;
	float seperation, angle;
	public AimedParallel(FloatRect pos, float delay = 1.6f, int rows = 3, float seperation = 25, int volleys = 8, float speed = 70) : base(pos, delay, speed, volleys) {
		this.rows = rows;
		this.seperation = seperation;
	}

	protected override void Shoot() {

		float radPerpAngle = (angle + 90) * (float)(Math.PI / 180);


		Vector2 shotLoc = new Vector2((float)(Math.Sin(radPerpAngle) * seperation), -(float)(Math.Cos(radPerpAngle) * seperation));
		Vector2 seperationVector = shotLoc * 2 / (rows - 1);
		for (int i = 0; i < rows; i++) {
			FireAtAngle(angle, speed, shotLoc + pos.Centre);
			shotLoc -= seperationVector;
		}

	}
	protected override void newVolley() {
		base.newVolley();
		Vector2 angleCartesian = ArcadeGame.player.Bounds.Centre - pos.Centre;
		angle = (float)Math.Atan(angleCartesian.Y / angleCartesian.X);
		angle /= (float)(Math.PI / 180);//converting to degrees
		if (angleCartesian.X > 0) {
			angle += 180;
		}
		angle -= 90;
	}
}

public class Straight : EnemyWeapon {
	float angle;
	public Straight(FloatRect pos, float delay = 1, float speed = 60, float angle = 180, int volleys = 0) :
		base(pos, delay, speed, volleys, Assets.shootSounds[1]) {
		this.angle = angle;
	}

	protected override void Shoot() {
		FireAtAngle(angle, speed);
	}
}
public class BigShots : EnemyWeapon {

	public BigShots(FloatRect pos, float delay = 1, float speed = 60, int volleys = 0) :
	  base(pos, delay, speed, volleys, Assets.smallExplosion1) {

	}

	protected override void Shoot() {
		NodeManager.AddNode(new BigShot(new Vector2Data(0, speed), pos.Centre));
	}
}
public class WallAlternate : EnemyWeapon {
	private int rows;
	private float seperation;
	private bool switched;
	public WallAlternate(FloatRect pos, float delay = 1, int rows = 2, float seperation = 18, int volleys = 1, float speed = 60) :
		base(pos, delay, speed, volleys) {
		this.rows = rows;
		this.seperation = seperation;
	}

	protected override void Shoot() {
		Vector2 shootLoc = pos.Centre;
		shootLoc.X -= seperation * rows / 2;
		if (switched) {

			for (int i = 0; i <= rows; i++) {
				FireAtAngle(180, speed, shootLoc);
				shootLoc.X += seperation;
			}

		}
		else {
			shootLoc.X += seperation / 2;
			for (int i = 0; i < rows; i++) {
				FireAtAngle(180, speed, shootLoc);
				shootLoc.X += seperation;
			}
		}
	}
	protected override void newVolley() {
		base.newVolley();
		switched = !switched;

	}

}
public class Spread : EnemyWeapon {
	private float angle, rowAngle;
	int shots;
	public Spread(FloatRect pos, float delay = 1, int shots = 3, float angle = 30, float speed = 60, int volleys = 3) :
	 base(pos, delay, speed, volleys) {
		this.angle = angle;
		this.shots = shots;
		rowAngle = angle * 2 / shots;
	}

	protected override void Shoot() {
		for (int i = 0; i <= shots; i++) {
			FireAtAngle(180 - angle + (rowAngle * i), speed);
		}
		// FireAtAngle(180, speed);
		// FireAtAngle(180 - angle, speed);
		// FireAtAngle(180 + angle, speed);
	}
}
public class SpreadAlternating : EnemyWeapon {
	private float angle, rowAngle, roundDelay;
	private int rows;
	private bool switched;
	public SpreadAlternating(FloatRect pos, float delay = 1, float angle = 60, int rows = 5, int volleys = 3, float speed = 60) :
		base(pos, delay, speed, volleys) {
		this.angle = angle;
		this.rows = rows;
		if (rows > 100) {
			sound = Assets.smallExplosion1;
		}

		rowAngle = angle * 2 / rows;
	}

	protected override void Shoot() {
		if (switched) {
			for (int i = 0; i <= rows; i++) {
				FireAtAngle(180 - angle + (rowAngle * i), speed);
			}

		}
		else {
			for (int i = 0; i < rows; i++) {
				FireAtAngle(180 - angle + (rowAngle * i) + rowAngle / 2, speed);
			}
		}
	}
	protected override void newVolley() {
		base.newVolley();
		switched = !switched;
	}
}
public class Explosion : EnemyWeapon {
	private int prongs;
	public Explosion(FloatRect pos, float delay = -1, int prongs = 12, int volleys = 5, float speed = 60) :
	 base(pos, delay, speed, sound: Assets.smallExplosion1) {
		this.volleys = volleys;
		this.prongs = prongs;
	}

	protected override void Shoot() {
		for (int i = 0; i < prongs; i++) {
			FireAtAngle(360 / prongs * i, speed);
		}
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
		angle += (float)(spinSpeed * gameTime.ElapsedGameTime.TotalSeconds);
	}

	protected override void Shoot() {
		for (int i = 1; i <= prongs; i++) {
			FireAtAngle(angle + i * 360 / prongs, speed);
		}
	}
}

public class CirclePath : EnemyWeapon {
	bool doubled;
	float size, angle, loopSpeed;

	public CirclePath(FloatRect pos, bool doubled = false, float delay = 2, float speed = 30, float size = 55, float loopSpeed = 0.1f,
		int volleys = 6, float angle = 180) :
	base(pos, delay, speed, volleys) {
		this.doubled = doubled;
		this.size = size;
		this.angle = angle * (float)(Math.PI / 180);
		this.loopSpeed = loopSpeed;
	}


	protected override void Shoot() {
		float x = (float)(Math.Sin(angle) * speed);
		float y = -(float)(Math.Cos(angle) * speed);
		NodeManager.AddNode(new EnemyCircleBullet(new Vector2Data(x, y), pos.Centre, size, loopSpeed));
		NodeManager.AddNode(new EnemyCircleBullet(new Vector2Data(x, y), pos.Centre, size, loopSpeed, reversed: true));
	}
}




