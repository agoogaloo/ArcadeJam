
using System;
using Engine.Core.Components;
using Microsoft.Xna.Framework;

namespace Engine.Core.Physics;

public class VelMovement : PhysicsComponent {
    private Vector2Comp vel;
    private Vector2Comp pos;

    public VelMovement (Vector2Comp vel, Vector2Comp pos){
        this.vel = vel;
        this.pos = pos;
    }
	public override void Update(GameTime gameTime) {
		pos.val+=vel.val;
	}
}

public class Gravity : PhysicsComponent {
    private Vector2Comp vel;

    public Gravity(Vector2Comp vel){
        this.vel = vel;
    }

    public override void Update(GameTime gameTime) {
        double time = gameTime.ElapsedGameTime.TotalSeconds;
        vel.val.Y += (float)time*5;

    }

}

