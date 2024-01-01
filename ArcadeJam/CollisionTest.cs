using System;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class CollisionTest : Node {

	private FloatRect bounds;
	RectVisualizer render;
	Collision collision;

	public CollisionTest(FloatRect bounds) {
		this.bounds = bounds;
		render = new(bounds);
		collision = new(bounds, this);
	}
	public CollisionTest() : this(new FloatRect(0, 0, 50, 20)) {}



	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		render.Draw(spriteBatch);
	}

	public override void Update(GameTime gameTime) {
		collision.Update();
	}
}
