using System;
using System.Diagnostics;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class InputMovement {
    private Vector2Data vel;
    private FloatData moveSpeed;

    private FloatData combo;
    private Analog left = InputHandler.getAnalog("L"), right = InputHandler.getAnalog("R"),
        up = InputHandler.getAnalog("U"), down = InputHandler.getAnalog("D");
    private bool bouncing = false;
    private float friction = 12f;

    public InputMovement(Vector2Data vel, FloatData moveSpeed, FloatData combo) {
        this.vel = vel;
        this.moveSpeed = moveSpeed;
        this.combo = combo;

    }

    public void Update(GameTime gameTime) {
        Vector2 direction = new(right.Value - left.Value, down.Value - up.Value);
        if (direction.LengthSquared() > 0) {
            direction.Normalize();
        }
        if (bouncing) {
            //Console.WriteLine("vel:" + vel.val);
            vel.val.X = direction.X * moveSpeed.val;
            if (direction.Y > 0) {
                vel.val.Y += direction.Y * moveSpeed.val / 25;
            }
            else {
                vel.val.Y += direction.Y * moveSpeed.val / 10;
            }

            vel.val.Y -= friction;
            if (Math.Abs(vel.val.Y) <= moveSpeed.val) {
                bouncing = false;
            }
        }
        else {
            vel.val = direction * moveSpeed.val;
        }
        //Console.WriteLine(vel.val);


    }
    public void bounce() {
        Console.WriteLine("bouncing");
        bouncing = true;

        vel.val.Y = 130 + moveSpeed.val;
    }
}

