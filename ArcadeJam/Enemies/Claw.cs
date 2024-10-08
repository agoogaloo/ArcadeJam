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
	Sprite damageTex, armSprite = new(Assets.crabArm);
	public IntData health = new(300), phase = new(0);
	public FloatRect Bounds { get; private set; } = new(0, 0, 15, 33);
	Vector2Data vel;
	FloatRect bodyBounds;
	Sprite sprite;
	RectRender renderer;
	RectVisualizer hitBoxVisualizer;

	Vector2Data[] armLocs = { new(), new(), new(), new(), new(), new(), new(), new(), new() };
	PointRender[] armSegs;




	EnemyDamage damager;
	Collision grappleCollision;
	EnemyWeapon currentPattern;

	private float time = 0;


	public Claw(bool left, BoolData jabbing, FloatRect bodyBounds, IntData phase, Vector2Data vel) {
		this.left = left;
		this.phase = phase;
		this.jabbing = jabbing;
		currentPattern = new CirclePath(Bounds, speed: 20, size: 45, loopSpeed: 0.3f, delay: 3.75f);

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
			armSegs[i] = new(armSprite, armLocs[i], true);
		}

		renderer = new(sprite, Bounds);
		damager = new(Bounds, null, health, sprite, damageTex);
		this.vel = vel;
		grappleCollision = new(Bounds, this, "grapple");
		hitBoxVisualizer = new(Bounds);
		grappleCollision.Remove();


	}

	public override void Update(GameTime gameTime) {
		switch (phase.val) {
			case 0:
				ripple(gameTime);
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
		if (!(jabbing.val && phase.val == 3)) {
			grappleCollision.Remove();


		}
		if (jabbing.val && phase.val == 3) {

			grappleCollision.Readd();


			damageTex.texture = openTextures[1];
			sprite.texture = openTextures[0];

			Bounds.width = 21;
			Bounds.height = 15;
		}
		else if (grappled) {
			sprite.texture = openTextures[1];

		}
		else {
			damageTex.texture = closedTextures[1];
			sprite.texture = closedTextures[0];
			Bounds.width = 15;
			Bounds.height = 33;
		}
		damager.Update();
		if (health.val <= 0 && Alive) {
			Alive = false;
			NodeManager.AddNode(new ExplosionEffect(Bounds.Centre));
			NodeManager.AddNode(new ExplosionEffect(new Vector2(Bounds.Centre.X, Bounds.Top)));
			NodeManager.AddNode(new ExplosionEffect(new Vector2(Bounds.Centre.X, Bounds.Bottom)));

			Console.WriteLine("claw is gone");
		}
		if (!grappled) {
			Bounds.Location += vel.val;
		}
		vel.val = Vector2.Zero;
	}
	private void phase1(GameTime gameTime) {
		currentPattern.Update(gameTime);
		if (health.val <= 200) {
			float reloadTime = (float)Math.Max(Math.Min(currentPattern.timeLeft, 1.5f), 0.5);
			currentPattern = new SpreadAlternating(Bounds, delay: 1.5f);
			currentPattern.timeLeft = reloadTime;
			phase.val = 2;
			NodeManager.AddNode(new ExplosionEffect(Bounds.Centre, true));

		}


	}
	private void phase2(GameTime gameTime) {
		currentPattern.Update(gameTime);

	}
	private void phase3(GameTime gameTime) {

		currentPattern.Update(gameTime);
		if (jabbing.val) {
			ripple(gameTime);
			if ((Bounds.Centre.X < 3 || Bounds.Centre.Y < 10 || Bounds.Centre.X > ArcadeGame.gameWidth - 3 ||
			Bounds.Centre.Y > ArcadeGame.gameHeight - 20) || Bounds.Intersects(ArcadeGame.player.Bounds)) {
				jabbing.val = false;
				currentPattern.fire();
			}
		}
	}
	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		//drawing arms
		//calculating shoulter location
		Vector2 shoulderDiff = bodyBounds.Centre;
		if (left) {
			shoulderDiff.X -= 15;
		}
		else {

			shoulderDiff.X += 15;
		}
		shoulderDiff.Y += 25;
		shoulderDiff -= Bounds.Centre;
		//drawing arm
		for (int i = 0; i < armLocs.Length; i++) {
			armLocs[i].val = Bounds.Centre + i * shoulderDiff / (armLocs.Length);
			armLocs[i].val.Y -= Bounds.height / 2;

		}
		armSprite.texture = Assets.crabArm;
		foreach (PointRender i in armSegs) {

			i.Draw(spriteBatch);
		}
		//drawing hinges
		for (int i = 0; i < armLocs.Length; i += 3) {
			armLocs[i].val = Bounds.Centre + i * shoulderDiff / (armLocs.Length);
			armLocs[i].val.Y -= Bounds.height / 2;

		}
		armSprite.texture = Assets.crabHinge;
		for (int i = 0; i < armSegs.Length; i += 3) {
			armSegs[i].Draw(spriteBatch);
		}
		renderer.Draw(spriteBatch);
		hitBoxVisualizer.bounds = Bounds;
		hitBoxVisualizer.Draw(spriteBatch);
		hitBoxVisualizer.bounds = bodyBounds;
		hitBoxVisualizer.Draw(spriteBatch);


	}

	public void end() {
		damager.End();
		grappleCollision.Remove();



	}
	public void startPhase3() {
		phase.val = 3;
		currentPattern = new Explosion(Bounds, prongs: 10, speed: 100);
		currentPattern.sound = Assets.grappleHit;

	}

	public void GrappleStun() {
		grappled = true;
		jabbing.val = false;
		Bounds.height = 20;
	}

	public void GrappleHit(int damage) {
		grappled = false;
		health.val -= damage;
		grappleCollision.Remove();
		NodeManager.AddNode(new ExplosionEffect(Bounds.Centre, true, false));
		// Assets.bigExplosion.CreateInstance();
		Assets.bigExplosion.Play();
	}

	private void ripple(GameTime gameTime) {
		time += (float)gameTime.ElapsedGameTime.TotalSeconds;
		if (time > 0.1) {
			NodeManager.AddNode(new Ripple(new Vector2(Bounds.Left + 3, Bounds.Top - 2), true));
			NodeManager.AddNode(new Ripple(new Vector2(Bounds.Right - 3, Bounds.Top - 2), false));
			time = 0;


		}
	}

}