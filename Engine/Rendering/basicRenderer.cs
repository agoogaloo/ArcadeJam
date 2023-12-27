using System;
using System.Collections.Generic;
using ArcadeJam;
using Engine.Core.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Components;
public class PointRender {
    private Sprite sprite;
    private Vector2Data position;


    public PointRender(Sprite sprite, Vector2Data position) {
        this.sprite = sprite;
        this.position = position;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        int x = (int)Math.Round(position.val.X), y = (int)Math.Round(position.val.Y);
        spriteBatch.Draw(sprite.texture, new Vector2(x, y), Color.White);
    }


}
public class RectRender {
    private Sprite sprite;
    private RectData bounds;


    public RectRender(Sprite sprite, RectData bounds) {
        this.sprite = sprite;
        this.bounds = bounds;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        Point centre = bounds.val.Center;
        centre -= sprite.texture.Bounds.Center;

        spriteBatch.Draw(sprite.texture, centre.ToVector2(), Color.White);
    }


}

public class RectVisualizer {

    private RectData bounds;


    public RectVisualizer(RectData bounds) {

        this.bounds = bounds;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        
        RenderUtils.DrawRectangle(spriteBatch, bounds.val, Color.Red);

    }
}