using System;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Bullet : Node {
    private Sprite texture = new(Assets.icicle);
    private FloatRect bounds = new(0, 0, 6, 12);
    private Vector2Data vel;

    private VelMovement movement;
    RectRender renderer;
    RectVisualizer boundsVisualizer;

    public Bullet(Vector2Data vel, Vector2Data startPos) {
        this.vel = vel;
        bounds.Centre = startPos.val;
        movement = new(vel, bounds);
        renderer = new(texture, bounds);
        boundsVisualizer = new(bounds);

    }

	public override void Update(GameTime gameTime) {

        movement.Update(gameTime);

    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(gameTime, spriteBatch);
        boundsVisualizer.Draw(spriteBatch);

    }


}
