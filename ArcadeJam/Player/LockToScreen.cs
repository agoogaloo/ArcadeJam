using System;
using Engine.Core.Data;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LockToScreen
{

    private FloatRect bounds;

    public LockToScreen(FloatRect bounds){
        this.bounds = bounds;
    }


    public void Update(GameTime gameTime){
        if (bounds.Left<=0){
            bounds.x = 0;
        }
        if (bounds.Top<=0){
            bounds.y = 0;
        }
        if (bounds.Right>=ArcadeGame.width){
            bounds.x = ArcadeGame.width-bounds.width;
        }
        if (bounds.Bottom>=ArcadeGame.height){
            bounds.y = ArcadeGame.height-bounds.height;
        }
        
        
    }

}
