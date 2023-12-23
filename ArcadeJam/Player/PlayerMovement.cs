using System;
using Engine.Core.Data;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class PlayerMovement {
    private Vector2Data vel, position;
    private DoubleData moveSpeed;

    private InputMovement inputMove;
    private VelMovement VelMove;

    public PlayerMovement(Vector2Data vel, Vector2Data position, DoubleData moveSpeed){
        this.vel = vel;
        this.position = position;
        this.moveSpeed = moveSpeed;

        inputMove = new(vel, moveSpeed);
        VelMove = new(vel, position);

    }



    public void Update(GameTime gameTime){
        inputMove.Update(gameTime);
        VelMove.Update(gameTime);
        vel.val=Vector2.Zero;



    }


}
