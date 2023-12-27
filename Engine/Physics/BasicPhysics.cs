﻿
using System;
using Engine.Core.Components;
using Engine.Core.Data;
using Microsoft.Xna.Framework;

namespace Engine.Core.Physics;

public class VelMovement  {
    private Vector2Data vel;
    private RectData bounds;

    public VelMovement (Vector2Data vel, RectData bounds){
        this.vel = vel;
        this.bounds = bounds;
    }
	public  void Update(GameTime gameTime) {
		bounds.val.Location+=vel.val.ToPoint();
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

