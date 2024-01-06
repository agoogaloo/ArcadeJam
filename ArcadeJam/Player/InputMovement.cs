﻿using System;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class InputMovement {
    private Vector2Data vel;
    private FloatData moveSpeed;

    private Analog left = InputHandler.getAnalog("L"), right = InputHandler.getAnalog("R"),
        up = InputHandler.getAnalog("U"), down = InputHandler.getAnalog("D");

    public InputMovement(Vector2Data vel, FloatData moveSpeed) {
        this.vel = vel;
        this.moveSpeed = moveSpeed;

    }

    public void Update(GameTime gameTime) {
        Vector2 direction = new(right.Value - left.Value, down.Value - up.Value);
        if (direction.LengthSquared() > 0) {
            direction.Normalize();
        }
        vel.val = direction * moveSpeed.val;
        //Console.WriteLine(vel.val);
      

    }
}

