using System;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using Engine.Core;
using Microsoft.Xna.Framework;

namespace Engine.Core.Input;

public class Button {
    public bool Held { get; private set; }

    public double HoldTime { get; private set; }
    public bool JustPressed { get; private set; }
    public bool JustReleased { get; private set; }

    private bool heldThisFrame = false;

    public void hold() {
        heldThisFrame = true;
    }
    /// <summary>
    /// called after all the input bindings have tried to press the button
    /// updates/sets all the held/justHeld/released fields
    /// </summary>
    public void update(GameTime gameTime) {
        JustPressed = false;
        JustReleased = false;
        if (heldThisFrame) {
            if (!Held) {
                JustPressed = true;
            }
            Held = true;
            HoldTime+=gameTime.ElapsedGameTime.TotalSeconds;
        }
        else {
            if (Held) {
                JustReleased = true;
            }
            Held = false;
            HoldTime = 0;
        }

        heldThisFrame = false;
    }
    /// <summary>
    /// stops other functions from using the same input after it has been used. 
    /// only applies to justpressed, not held (this will be more usefull when i do buffering)
    /// </summary>
    public void consume() {
        JustPressed = false;

    }
}

public class Analog {
    public float Value { get; private set; }
    public float maxFrameVal = 0;

    public void updateVal(float val) {
        maxFrameVal = Math.Max(val, maxFrameVal);
    }
    public void update() {
        Value = maxFrameVal;
        maxFrameVal = 0;
    }
}





