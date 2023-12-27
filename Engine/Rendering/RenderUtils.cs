using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class RenderUtils {
    

    public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color) {
        float length = (end - start).Length();
        float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
        batch.Draw(Assets.pixel, start, null, color, rotation, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
    }
    

    public static void DrawRectangle(SpriteBatch batch, Rectangle rectangle, Color color) {
        batch.Draw(Assets.pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
        batch.Draw(Assets.pixel, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
        batch.Draw(Assets.pixel, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
        batch.Draw(Assets.pixel, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);
    }

}
