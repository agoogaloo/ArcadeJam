using System;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArcadeJam.Enemies;

public class Enemy : Node, IGrappleable {
	protected static Random rand = new();
	public IntData Health { get; protected set; } = new IntData(30);
	protected Sprite sprite;
	protected Texture2D[] textures;
	protected Vector2Data vel;
	protected FloatRect bounds;

	protected RectRender renderer;

	protected EnemyMovement movement;
	protected EnemyDamage damager;
	protected EnemyWeapon weapon;
	protected Collision grappleCollision;
	protected FloatRect grappleBounds;
	RectVisualizer hitBoxVisualizer;
	protected bool stunned = false, grappleable = false, doGrapple = true;

	protected int killPoints = 50, grapplePoints = 100;
	protected ScoreData score;
	protected double rippleTimer, rippleDelay = 2;

	public Enemy(EnemyMovement movement, Texture2D[] textures, ScoreData scoreData) {
		renderHeight = 2;
		this.movement = movement;
		this.textures = textures;
		this.score = scoreData;
		sprite = new(textures[0]);
		bounds = new(0, 0, 11, 13);
		grappleBounds = new(0, 0, 11, 13);
		vel = new(0, 0);
		movement.Init(bounds, vel);
		weapon = new Straight(bounds);
		renderer = new(sprite, bounds);
		damager = new(bounds, this, Health, sprite, textures[1]);
		grappleCollision = new(grappleBounds, this, "grapple");
		grappleCollision.Remove();
		hitBoxVisualizer = new(grappleBounds);
	}


	public override void Update(GameTime gameTime) {
		if (!stunned) {
			movement.Update(gameTime);
			weapon.Update(gameTime);
			updateGrappleBounds();
		}

		if (doGrapple && !grappleable && Health.val < ArcadeGame.player.grappleDamage.val) {
			grappleCollision.Readd();
			grappleable = true;
		}
		else if (grappleable && Health.val > ArcadeGame.player.grappleDamage.val) {
			grappleable = false;
			grappleCollision.Remove();
		}

		sprite.texture = textures[0];
		if (doGrapple && grappleable) {
			sprite.texture = textures[2];

		}
		damager.Update();
		if (Health.val <= 0) {
			Alive = false;
			score.addScore(killPoints, bounds.Centre);
		}
		doRipples(gameTime);

	}
	protected virtual void doRipples(GameTime gameTime) {
		rippleTimer += gameTime.ElapsedGameTime.TotalSeconds;
		if (rippleTimer >= rippleDelay) {
			NodeManager.AddNode(new Ripple(new Vector2(bounds.x - 3f, bounds.Centre.Y - 5), true));
			NodeManager.AddNode(new Ripple(new Vector2(bounds.Right + 2f, bounds.Centre.Y - 5), false));
			rippleTimer = rand.NextDouble() * 0.5f;

		}
	}
	protected virtual void updateGrappleBounds() {
		grappleBounds.x = bounds.Centre.X - grappleBounds.width / 2;
		grappleBounds.y = bounds.y;
	}
	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

		renderer.Draw(spriteBatch);
		hitBoxVisualizer.bounds = grappleBounds;
		hitBoxVisualizer.Draw(spriteBatch);
		hitBoxVisualizer.bounds = bounds;
		hitBoxVisualizer.Draw(spriteBatch);
	}

	public void GrappleStun() {
		stunned = true;

	}
	public virtual void GrappleHit(int damage) {
		Health.val -= damage;
		stunned = false;
		if (Health.val <= 0) {
			score.addScore(grapplePoints, bounds.Centre);
			Alive = false;

		}

	}


	public override void End() {
		damager.End();
		grappleCollision.Remove();
		NodeManager.AddNode(new ExplosionEffect(bounds.Centre, true));
	}
}
public class IntroChest : Enemy {
	Texture2D intro = Assets.introText;
	GamePadState gPadState = GamePad.GetState(PlayerIndex.One);
	public IntroChest(Vector2 startLoc) : base(new Stationary(), Assets.introChest, null) {
		Health.val = 27;
		weapon = new Nothing();
		bounds.Centre = startLoc;
		bounds.width = 14;
		bounds.height = 20;
		grappleBounds.width = 14;
		grappleBounds.height = 20;
		killPoints = 0;
		grapplePoints = 0;
		updateGrappleBounds();

	}
	public override void Update(GameTime gameTime) {



		if (doGrapple && !grappleable && Health.val < ArcadeGame.player.grappleDamage.val) {
			grappleCollision.Readd();
			grappleable = true;
		}
		else if (grappleable && Health.val > ArcadeGame.player.grappleDamage.val) {
			grappleable = false;
			grappleCollision.Remove();
		}

		sprite.texture = textures[0];
		damager.Update();
		if (doGrapple && grappleable) {
			sprite.texture = textures[2];

		}




		doRipples(gameTime);
	}
	public override void GrappleHit(int damage) {
		Alive = false;
	}
	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		base.Draw(gameTime, spriteBatch);


		if (GamePad.GetState(PlayerIndex.One) != gPadState) {
			gPadState = GamePad.GetState(PlayerIndex.One);
			intro = Assets.introText;
		}
		else if (Keyboard.GetState().GetPressedKeyCount() >= 1) {
			intro = Assets.introTextKeyboard;
		}
		spriteBatch.Draw(intro, new Vector2(ArcadeGame.gameWidth / 2 - Assets.introText.Width / 2, 5), Color.White);
		//spriteBatch.Draw(Assets.startText, new Vector2(ArcadeGame.gameWidth / 2 - Assets.introText.Width / 2, 100), Color.White);

	}
	protected override void doRipples(GameTime gameTime) {

		rippleTimer += gameTime.ElapsedGameTime.TotalSeconds;
		if (rippleTimer >= rippleDelay) {
			NodeManager.AddNode(new Ripple(new Vector2(bounds.x - 3f, bounds.Centre.Y + 6), true));
			NodeManager.AddNode(new Ripple(new Vector2(bounds.Right + 2f, bounds.Centre.Y + 6), false));
			rippleTimer = 0;
		}
	}
}

public class BasicEnemy : Enemy {

	public BasicEnemy(EnemyMovement movement, ScoreData score) : base(movement, Assets.enemy, score) {

		// doGrapple = false;

	}
}

public class AimedEnemy : Enemy {

	public AimedEnemy(EnemyMovement movement, ScoreData score) : base(movement, Assets.enemy, score) {

		weapon = new AimedParallel(bounds, delay: 1.5f, rows: 1, seperation: 0, volleys: 3);
		// doGrapple = false;

	}
}
public class TrippleEnemy : Enemy {

	public TrippleEnemy(EnemyMovement movement, ScoreData score) : base(movement, Assets.enemy, score) {

		weapon = new Spread(bounds, delay: 1.5f, shots: 2);
		// doGrapple = false;

	}
}



public class SpinEnemy : Enemy {

	public SpinEnemy(EnemyMovement movement, ScoreData score) : base(movement, Assets.bombEnemy, score) {
		Health.val = 50;
		bounds.width = 32;
		weapon = new Spiral(bounds);
		renderer = new(sprite, bounds);

	}

}



