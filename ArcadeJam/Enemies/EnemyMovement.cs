
using Engine.Core.Data;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;

public abstract class EnemyMovement{
    protected Vector2Data vel;
    protected FloatRect bounds;

    private VelMovement velMovement;

    public EnemyMovement(FloatRect bounds, Vector2Data vel){
        this.bounds = bounds;
        this.vel = vel;
        this.velMovement = new(vel, bounds);
    }
    public virtual void Update(GameTime gameTime){
        velMovement.Update(gameTime);
        vel.val = Vector2.Zero;

    }

}

public class MoveToPoint : EnemyMovement {
    private Vector2 destination;
    private float speed, easing;

	public MoveToPoint(FloatRect bounds, Vector2Data vel, Vector2 destination, float speed = 3, float easing = 4) : base(bounds, vel) {
        this.destination = destination;
        this.speed = speed;
        this.easing = easing;
	}

	public override void Update(GameTime gameTime){
        Vector2 movement = destination-bounds.Centre;
        movement*=(float)(speed*gameTime.TotalGameTime.TotalSeconds/easing);
        vel.val+=movement;
        base.Update(gameTime);
	}
}