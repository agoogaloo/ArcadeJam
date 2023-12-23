
using System;
using Engine.Core.Components;
using Engine.Core.Data;
using Microsoft.Xna.Framework;

namespace Engine.Core.Physics;

public class VelMovement  {
    private Vector2Data vel;
    private Vector2Data pos;

    public VelMovement (Vector2Data vel, Vector2Data pos){
        this.vel = vel;
        this.pos = pos;
    }
	public  void Update(GameTime gameTime) {
		pos.val+=vel.val;
	}
}


public class Gravity {
    private Vector2Data vel;

    public Gravity(Vector2Data vel){
        this.vel = vel;
    }

    public  void Update(GameTime gameTime) {
        double time = gameTime.ElapsedGameTime.TotalSeconds;
        vel.val.Y += (float)time*5;

    }

}

