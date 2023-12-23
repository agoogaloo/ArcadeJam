using System;
using System.Xml.Xsl;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArcadeJam;
public class Jump : PhysicsComponent {
    Vector2Comp vel;
    Button button;
    public Jump(Vector2Comp vel, Button jumpButton){
        this.vel = vel;
        this.button = jumpButton;
    }

    public override void Update(GameTime gameTime) {
        if (button.JustPressed){
            button.consume();
            vel.val.Y-=5;
        }
			
    }
}

