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
    public ExplosionEffect(Vector2 loc, bool fast = false, bool sound = true) {
        renderHeight = 3;
        sprite = new(Assets.explosion);
        if (fast) {
            render = new(sprite, new Vector2Data(loc), frame, Assets.explosionSize, 20);
            Assets.smallExplosion1.CreateInstance();
            Assets.smallExplosion1.Play();
        }
        else {
            render = new(sprite, new Vector2Data(loc), frame, Assets.explosionSize, 10);
            Assets.bigExplosion.CreateInstance();
            Assets.bigExplosion.Play();
        }


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
