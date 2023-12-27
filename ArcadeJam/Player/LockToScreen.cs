using System;
using Engine.Core.Data;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LockToScreen
{
     private Vector2Data vel;
    private RectData bounds;

    public LockToScreen(Vector2Data vel, RectData bounds){
        this.vel = vel;
        this.bounds = bounds;
    }


    public void Update(GameTime gameTime){
        if (bounds.val.Left+vel.val.X<=0){
            vel.val.X = -bounds.val.Left;
        }
        if (bounds.val.Top+vel.val.Y<=0){
            vel.val.Y = -bounds.val.Top;
        }
        if (bounds.val.Right+vel.val.X>=ArcadeGame.width-1){
            vel.val.X = ArcadeGame.width-bounds.val.Right-1;
        }
        if (bounds.val.Bottom+vel.val.Y>=ArcadeGame.height-1){
            vel.val.Y = ArcadeGame.height-bounds.val.Bottom-1;
        }
        
        
    }

}
