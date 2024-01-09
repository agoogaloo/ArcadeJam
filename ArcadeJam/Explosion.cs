using System;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class ExplosionEffect : Node {
    IntData frame = new();
    Sprite sprite;
    PointAnim render;
    public ExplosionEffect(Vector2 loc) {
        renderHeight = 0;
        sprite = new(Assets.explosion);
       
        render = new(sprite, new Vector2Data(loc), frame, Assets.explosionSize, 10);


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
