using System;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class InputMovement : PhysicsComponent {
    private Vector2Data vel;
    private DoubleData moveSpeed;

    private Analog left = InputHandler.getAnalog("L"), right = InputHandler.getAnalog("R"),
        up = InputHandler.getAnalog("U"), down = InputHandler.getAnalog("D");

    public InputMovement(Vector2Data vel, DoubleData moveSpeed) {
        this.vel = vel;
        this.moveSpeed = moveSpeed;
    }

    public override void Update(GameTime gameTime) {
        vel.val.X+=(float)((right.Value-left.Value)*moveSpeed.val);
        vel.val.Y+=(float)((down.Value-up.Value)*moveSpeed.val);

    }
}

