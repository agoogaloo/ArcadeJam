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
	FloatRect Bounds { get; set; } = new(new Rectangle(150, 150, 8, 12));
	Vector2Data Vel { get; set; } = new(new Vector2(0, 0));
	DoubleData moveSpeed = new(1.5);
	Sprite Sprite { get; set; } = new Sprite(Assets.player);
	List<Node> collisions = new();

	private PlayerMovement movement;
	private RectRender render;
	private RectVisualizer showBounds;
	private PlayerAbilities abilities;
	private Collision collision;

	string[] collisionGroups = new string[]{"enemy", "enemyBullet"};



	public Player() {
		renderHeight = 1;
		movement = new(Vel, Bounds, moveSpeed);
		abilities = new(Bounds, moveSpeed);
		render = new(Sprite, Bounds);
		showBounds = new(Bounds);
		collision = new(Bounds, this, "player", collisions);

	}

	public override void Update(GameTime gameTime) {
		
		movement.Update(gameTime);
		abilities.Update(gameTime);
		collision.Update(collisionGroups);
		Console.WriteLine("e");
		foreach( Node i in collisions){
			if (i is Bullet b){
				Console.WriteLine("hit by a bullet");
				b.OnHit();
			}else{
				Console.WriteLine("hit by somthing else");
			}
		}
		

	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		render.Draw(gameTime, spriteBatch);
		showBounds.Draw( spriteBatch);
		

	}
}

