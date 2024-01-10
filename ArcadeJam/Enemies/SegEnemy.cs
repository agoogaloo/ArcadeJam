using ArcadeJam.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class SegmentEnemy : Enemy {
    bool pointOnLeft;
    public SegmentEnemy(EnemyMovement movement, ScoreData scoreData, bool leftPoint = true) :
    base(movement, Assets.trippleEnemyR, scoreData) {
        this.pointOnLeft = leftPoint;
        Health.val = 100;
        weapon = new SpreadAlternating(bounds, rows: 2);
        bounds.width = 32;
        bounds.height = 17;
        grappleBounds.width = 14;
        grappleBounds.height = bounds.height;
    }
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
         if (grappleable && pointOnLeft) {
            sprite.texture = textures[3];

        }
	}
	protected override void updateGrappleBounds() {
        grappleBounds.y = bounds.y;
        if (pointOnLeft) {
            grappleBounds.x = bounds.x;
        }else{
            grappleBounds.x = bounds.Right-grappleBounds.width;
        }
    }
}