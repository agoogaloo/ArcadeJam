using System;
using Engine.Core.Data;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LockToScreen
{
     private Vector2Data vel;
    private FloatRect bounds;

    public LockToScreen(Vector2Data vel, FloatRect bounds){
        this.vel = vel;
        this.bounds = bounds;
    }


    public void Update(GameTime gameTime){
        if (bounds.Left+vel.val.X<=0){
            vel.val.X = -bounds.Left;
        }
        if (bounds.Top+vel.val.Y<=0){
            vel.val.Y = -bounds.Top;
        }
        if (bounds.Right+vel.val.X>=ArcadeGame.width){
            vel.val.X = ArcadeGame.width-bounds.Right;
        }
        if (bounds.Bottom+vel.val.Y>=ArcadeGame.height){
            vel.val.Y = ArcadeGame.height-bounds.Bottom;
        }
        
        
    }

}
