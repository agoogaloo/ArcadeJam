
using System;
using Engine.Core.Data;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;

public abstract class EnemyMovement {
    protected Vector2Data vel;
    protected FloatRect bounds;

    protected VelMovement velMovement;

    public EnemyMovement() { }

    public EnemyMovement(FloatRect bounds, Vector2Data vel) {
        Init(bounds, vel);
    }
    public virtual void Init(FloatRect bounds, Vector2Data vel) {
        this.bounds = bounds;
        this.vel = vel;
        this.velMovement = new(vel, bounds);
    }

    public virtual void Update(GameTime gameTime) {
        velMovement.Update(gameTime);
        vel.val = Vector2.Zero;
        

    }

}
public  class Stationary : EnemyMovement {
    
}


public class MoveToPoint : EnemyMovement {
    private Vector2 start, destination;
    private float speed, easing;

    public MoveToPoint(Vector2 start, Vector2 destination, float speed = 60, float easing = 4) : base() {
        this.start = start;
        this.destination = destination;
        this.speed = speed;
        this.easing = easing;
    }
    public override void Init(FloatRect bounds, Vector2Data vel) {
        base.Init(bounds, vel);
        this.bounds.Centre = start;
    }

    public override void Update(GameTime gameTime) {
        Vector2 movement = destination - bounds.Centre;
        movement *= (float)(speed * gameTime.ElapsedGameTime.TotalSeconds / easing);
        vel.val = movement;
        base.Update(gameTime);
    }
    public MoveToPoint Copy() {
        return new MoveToPoint(start, destination, speed, easing);
    }
}