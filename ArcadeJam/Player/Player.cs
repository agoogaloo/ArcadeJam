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

public class Player : Node{
	Vector2Data Position { get; set; } = new(new Vector2(150, 50));
	Vector2Data Vel { get; set; } = new(new Vector2(0, 0));
	DoubleData moveSpeed  = new(0.5);
	Sprite sprite { get; set; } = new Sprite(Assets.player);

	public Player() {		
		// Add(new ScreenRender(sprite, Position));
		// Add(new VelMovement(Vel, Position));
		// Add(new InputMovement(Vel, moveSpeed));
	}

	public override void Update(GameTime gameTime) {
		
	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		
	}
}

