using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Components;

public class Sprite : IComponent {
    public Texture2D texture;

    public Sprite(Texture2D texture) {
        this.texture = texture;
    }
    public Sprite(string name, Game game){
        texture = game.Content.Load<Texture2D>(name);
    }
}
public class Vector2Comp : IComponent {
    public Vector2 val;
    public Vector2Comp(Vector2 value) {
        this.val = value;
    }    
}
public class IntComp : IComponent {
    public int val;
    public IntComp(int val){
        this.val = val;
    }
}

public class StringComp : IComponent {
    public String val;
    public StringComp(String val){
        this.val = val;
    }
}

public class DoubleComp : IComponent {
    public double val;
    public DoubleComp(double val){
        this.val = val;
    }
}
