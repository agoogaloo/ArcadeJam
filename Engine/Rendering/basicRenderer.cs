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
    private FloatRect bounds;


    public RectRender(Sprite sprite, FloatRect bounds) {
        this.sprite = sprite;
        this.bounds = bounds;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

        int x = (int)(bounds.x + (bounds.width  - sprite.texture.Width)/2+0.5);
        int y = (int)(bounds.y + bounds.height / 2 - sprite.texture.Height / 2+0.5);
        Vector2 centre = -sprite.texture.Bounds.Center.ToVector2();
        centre += bounds.Centre;
        //rounding centre
        centre.X = (int)Math.Round(centre.X);
        centre.Y = (int)Math.Round(centre.Y);

       
        spriteBatch.Draw(sprite.texture, new Vector2(x,y), Color.White);
    }


}

public class RectVisualizer {

    private FloatRect bounds;


    public RectVisualizer(FloatRect bounds) {

        this.bounds = bounds;
    }

    public void Draw(SpriteBatch spriteBatch) {
        Rectangle drawRect = new((int)(bounds.x+0.5), (int)(bounds.y+0.5),
            (int)Math.Round(bounds.width-1), (int)Math.Round(bounds.height-1));
            
        //RenderUtils.DrawRectangle(spriteBatch, drawRect, Color.Red);

    }
}