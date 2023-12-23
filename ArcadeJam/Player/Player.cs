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
	Engine.Core.Data.Vector2Data Position { get; set; } = new(new Vector2(150, 50));
	Engine.Core.Data.Vector2Data Vel { get; set; } = new(new Vector2(0, 0));
	DoubleData moveSpeed = new(1.5);
	Sprite sprite { get; set; } = new Sprite(Assets.player);

	PlayerMovement movement;
	ScreenRender render;


	public Player() {
		movement = new(Vel, Position, moveSpeed);
		render = new(sprite, Position);
	}

	public override void Update(GameTime gameTime) {
		movement.Update(gameTime);

	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		render.Draw(gameTime, spriteBatch);

	}
}

