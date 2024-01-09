using System;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Ripple : Node {
    IntData frame = new();
    Sprite sprite;
    PointAnim render;
    public Ripple(Vector2 loc, bool left) {
        renderHeight = 0;
        sprite = new(Assets.rippleR);
        if (left) {
            sprite.texture = Assets.rippleL;
        }
        render = new(sprite, new Vector2Data(loc), frame, Assets.rippleSize, 10);


    }
    public override void Update(GameTime gameTime) {
        if (frame.val >= 5) {
            Alive = false;
        }

    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        if (frame.val < 5) {
            render.Draw(spriteBatch, gameTime);
        }

    }


}
