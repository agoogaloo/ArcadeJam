using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using ArcadeJam.Weapons;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Input;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Player : Node {

	FloatData invincibleTime = new();
	public FloatRect Bounds { get; private set; } = new(new Rectangle(75, 120, 5, 7));
	Vector2Data Vel { get; set; } = new(new Vector2(0, 0));
	FloatData moveSpeed = new(1.5f);

	public FloatData combo { get; private set; } = new FloatData(1);
	public IntData score, lives = new IntData(30), grappleDamage = new();
	Sprite Sprite { get; set; } = new Sprite(Assets.player);
	List<Node> collisions = new();



	private PlayerMovement movement;
	private RectRender render;
	private RectVisualizer showBounds;
	private PlayerAbilities abilities;
	private Collision collision;

	string[] collisionGroups = new string[] { "enemy", "enemyBullet" };



	public Player(IntData score) {
		this.score = score;
		renderHeight = 1;
		BoolData useInput = new(true);
		movement = new(Vel, Bounds, moveSpeed, combo, useInput);
		abilities = new(Bounds, Vel, moveSpeed, combo, invincibleTime, useInput, score, grappleDamage);
		render = new(Sprite, Bounds);
		showBounds = new(Bounds);
		collision = new(Bounds, this, "player", collisions);

	}

	public override void Update(GameTime gameTime) {

		movement.Update(gameTime);
		abilities.Update(gameTime);
		collision.Update(collisionGroups);
		if (invincibleTime.val >= 0f) {
			invincibleTime.val -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			//Console.WriteLine("invincible, "+invincibleTime.val);

		}
		foreach (Node i in collisions) {
			if (i is Bullet b) {
				Console.WriteLine("hit by a bullet");
				b.OnHit();
				if (invincibleTime.val < 0f) {
					combo.val = 1;
					lives.val--;
					if (abilities.weapon >= 1) {
						abilities.weapon--;
					}
					if (lives.val < 0) {
						Alive = false;
					}

					invincibleTime.val = 3;
				}
			}
		}
	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		abilities.Draw(gameTime, spriteBatch);
		if (invincibleTime.val < 0 || (int)(invincibleTime.val * 10) % 2 == 0) {
			render.Draw( spriteBatch);
		}
		showBounds.Draw(spriteBatch);


		//spriteBatch.DrawString(Assets.font, "COMBO:" + combo.val, new Vector2(1, 5), Color.Red);
	}

	public void upgradeGun() {
		if (abilities.weapon < 2) {
			abilities.weapon += 1;
		}
	}
}

