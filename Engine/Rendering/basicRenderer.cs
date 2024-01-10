using System;
using System.Collections.Generic;
using ArcadeJam;
using Engine.Core.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Components;

public interface IRenderer{
    public void Draw( SpriteBatch spriteBatch,GameTime gameTime = null);
}
public class PointRender:IRenderer {
    public Sprite sprite;
    private Vector2Data position;
    private bool centred;


    public PointRender(Sprite sprite, Vector2Data position, bool centred = false) {
        this.sprite = sprite;
        this.position = position;
        this.centred = centred;
    }

    public void Draw( SpriteBatch spriteBatch,GameTime gameTime = null) {
        int x = (int)Math.Round(position.val.X), y = (int)Math.Round(position.val.Y);
        if (centred){
            x-=(int)(sprite.texture.Width/2+0.5);
            y -=(int)(sprite.texture.Width/2+0.5);
        }
        spriteBatch.Draw(sprite.texture, new Vector2(x, y), Color.White);
    }


}
public class PointAnim:IRenderer {
    public Sprite sprite;
    IntData frame;
    float timer = 0, frameTime;
    private Vector2Data position;
    private bool centred;
    Rectangle sourceRect;


    public PointAnim(Sprite sprite, Vector2Data position, IntData frame,Rectangle size, float fps = 5, bool centred = true) {
        this.sprite = sprite;
        this.position = position;
        this.centred = centred;
        this.frame = frame;
        this.sourceRect = size;
        frameTime = 1f/fps;
    }

    public void Draw( SpriteBatch spriteBatch,GameTime gameTime = null) {
        
        
        int x = (int)Math.Round(position.val.X), y = (int)Math.Round(position.val.Y);
        sourceRect.X = frame.val*sourceRect.Width;
        if (centred){
            x-=(int)(sourceRect.Width/2+0.5);
            y -=(int)(sourceRect.Width/2+0.5);
        }
        spriteBatch.Draw(sprite.texture, new Vector2(x, y),sourceRect, Color.White);
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > frameTime) {
            frame.val++;
            timer = 0;
        }
    }


}

public class RectRender:IRenderer {
    private Sprite sprite;
    private FloatRect bounds;


    public RectRender(Sprite sprite, FloatRect bounds) {
        this.sprite = sprite;
        this.bounds = bounds;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime = null) {

        int x = (int)(bounds.x + (bounds.width - sprite.texture.Width) / 2 + 0.5);
        int y = (int)(bounds.y + (bounds.height - sprite.texture.Height) / 2 + 0.5);
        // Vector2 centre = -sprite.texture.Bounds.Center.ToVector2();
        // centre += bounds.Centre;
        // //rounding centre
        // centre.X = (int)Math.Round(centre.X);
        // centre.Y = (int)Math.Round(centre.Y);


        spriteBatch.Draw(sprite.texture, new Vector2(x, y), Color.White);
    }


}

public class RectVisualizer:IRenderer {

    public FloatRect bounds;


    public RectVisualizer(FloatRect bounds) {

        this.bounds = bounds;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime = null) {
        Rectangle drawRect = new((int)(bounds.x + 0.5), (int)(bounds.y + 0.5),
            (int)Math.Round(bounds.width - 1), (int)Math.Round(bounds.height - 1));

       RenderUtils.DrawRectangle(spriteBatch, drawRect, Color.Red);

    }
}

public class CropRender:IRenderer {
    Sprite sprite;
    FloatRect drawRect;

    public CropRender(Sprite sprite, FloatRect drawRect) {
        this.sprite = sprite;
        this.drawRect = drawRect;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime = null) {
        Vector2 drawLoc = new Vector2((int)(drawRect.x + 0.5), (int)(drawRect.y + 0.5));
        Rectangle sourceRect = new(0, 0, (int)(drawRect.width + 0.5), (int)(drawRect.height + 0.5));
        spriteBatch.Draw(sprite.texture, drawLoc, sourceRect, Color.White);
    }
}