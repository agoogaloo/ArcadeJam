using System;
using Microsoft.Xna.Framework;


namespace Engine.Core.Data;
public class FloatRect {
    public float x, y, width, height;
    public float Left => x;

    public float Right => x + width;

    public float Top => y;

    public float Bottom => y + height;

    public Vector2 Centre {
        get { return new Vector2(x + width / 2, y + height / 2); }
        set {
            x = value.X - width / 2;
            y = value.Y - height / 2;
        }
    }
    public Vector2 Location {
        get {
            return new Vector2(x, y);
        }
        set {
            x = value.X;
            y = value.Y;
        }
    }
    public FloatRect(Rectangle val) : this(val.X, val.Y, val.Width, val.Height) { }
    public FloatRect(float x, float y, float width, float height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public bool Intersects(FloatRect value) {
        return value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;

    }
}