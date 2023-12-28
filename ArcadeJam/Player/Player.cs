using System;
using System.Runtime.Intrinsics;
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
	Sprite sprite { get; set; } = new Sprite(Assets.player);

	private PlayerMovement movement;
	private RectRender render;
	private RectVisualizer showBounds;
	private PlayerAbilities abilities;


	public Player() {
		movement = new(Vel, Bounds, moveSpeed);
		abilities = new(Bounds, moveSpeed);
		render = new(sprite, Bounds);
		showBounds = new(Bounds);

	}

	public override void Update(GameTime gameTime) {
		
		movement.Update(gameTime);
		abilities.Update(gameTime);
		

	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		render.Draw(gameTime, spriteBatch);
		showBounds.Draw( spriteBatch);
		

	}
}

