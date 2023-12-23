using System;
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
}
public class RectData {
    public Rectangle val;
    public RectData(Rectangle val) {
        this.val = val;
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
