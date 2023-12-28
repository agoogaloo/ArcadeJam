using System;
using Engine.Core.Data;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class PlayerMovement {
    private Vector2Data vel;
    private FloatRect bounds;
    private DoubleData moveSpeed;

    private InputMovement inputMove;
    private VelMovement VelMove;
    private LockToScreen screenLock;

    public PlayerMovement(Vector2Data vel, FloatRect bounds, DoubleData moveSpeed){
        this.vel = vel;
        this.bounds = bounds;
        this.moveSpeed = moveSpeed;

        inputMove = new(vel, moveSpeed);
        VelMove = new(vel, bounds);
        screenLock = new(vel, bounds);

    }



    public void Update(GameTime gameTime){
        inputMove.Update(gameTime);
        screenLock.Update(gameTime);
        VelMove.Update(gameTime);
        vel.val=Vector2.Zero;

    }


}
