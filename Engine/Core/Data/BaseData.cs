using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Data;

public class Sprite {
    public Texture2D texture;

    public Sprite(Texture2D texture) {
        this.texture = texture;
    }
    public Sprite(string name, Game game) {
        texture = game.Content.Load<Texture2D>(name);
    }
}

public class Vector2Data {
    public Vector2 val;
    public Vector2Data(Vector2 value) {
        this.val = value;
    }
    public Vector2Data(float x,float y):this(new Vector2(x,y)) {   }

}
public class FloatRect {
    public float x, y, width, height;
    public float Left => x;

    public float Right => x + width;

    public float Top => y;

    public float Bottom => y + height;

    public Vector2 Centre {
        get { return new Vector2(x + width / 2, y + height / 2); }
        set {
            x = value.X-width/2;
            y = value.Y-height/2;
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
}
public class IntData {
    public int val;
    public IntData(int val) {
        this.val = val;
    }
}

public class StringData {
    public String val;
    public StringData(String val) {
        this.val = val;
    }
}

public class DoubleData {
    public double val;
    public DoubleData(double val) {
        this.val = val;
    }
}
