using System;
using Engine.Core.Data;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class PlayerMovement {
    private Vector2Data vel;
    private FloatRect bounds;
    private FloatData moveSpeed;

    IntData combo;
    private InputMovement inputMove;
    private VelMovement VelMove;
    private LockToScreen screenLock;
    private BoolData useInput;

    public PlayerMovement(Vector2Data vel, FloatRect bounds, FloatData moveSpeed, IntData combo, BoolData useInput) {
        this.vel = vel;
        this.bounds = bounds;
        this.moveSpeed = moveSpeed;
        this.combo = combo;
        this.useInput = useInput;

        inputMove = new(vel, moveSpeed);
        VelMove = new(vel, bounds);
        screenLock = new(bounds);

    }

    public void Update(GameTime gameTime) {
        if (useInput.val) {
            inputMove.Update(gameTime);
        }
        VelMove.Update(gameTime);
        screenLock.Update(gameTime);
        //vel.val = Vector2.Zero;

    }
}
