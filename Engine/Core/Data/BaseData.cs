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

public class Vector2Comp {
    public Vector2 val;
    public Vector2Comp(Vector2 value) {
        this.val = value;
    }
}
public class RectComp {
    public Rectangle val;
    public RectComp(Rectangle val) {
        this.val = val;
    }
}
public class IntComp {
    public int val;
    public IntComp(int val) {
        this.val = val;
    }
}

public class StringComp {
    public String val;
    public StringComp(String val) {
        this.val = val;
    }
}

public class DoubleComp {
    public double val;
    public DoubleComp(double val) {
        this.val = val;
    }
}
