using System;
using System.Xml.Xsl;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArcadeJam;
public class Jump {
    Vector2Data vel;
    Button button;
    public Jump(Vector2Data vel, Button jumpButton){
        this.vel = vel;
        this.button = jumpButton;
    }

    public void Update(GameTime gameTime) {
        if (button.JustPressed){
            button.consume();
            vel.val.Y-=5;
        }
			
    }
}

