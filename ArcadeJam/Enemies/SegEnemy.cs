using ArcadeJam.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class SegmentEnemy : Enemy {
    int grapplePoint;
    public SegmentEnemy(EnemyMovement movement, ScoreData scoreData, int grapplePoint = 1) :
    base(movement, Assets.trippleEnemyR, scoreData) {
        this.grapplePoint = grapplePoint;
        Health.val = 100;
        weapon = new SpreadAlternating(bounds, rows: 3,delay:2);
        bounds.width = 32;
        bounds.height = 17;
        grappleBounds.width = 10;
        if(grapplePoint==3){
            grappleBounds.width = 14;
        }
        grappleBounds.height = bounds.height;
        killPoints = 100;
        grapplePoints = 150;
    }
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
         if (grappleable && grapplePoint==1) {
            sprite.texture = textures[2];

        }
         if (grappleable && grapplePoint==2) {
            sprite.texture = textures[3];

        }
         if (grappleable && grapplePoint==3) {
            sprite.texture = textures[4];

        }
	}
	protected override void updateGrappleBounds() {
        grappleBounds.y = bounds.y;
        grappleBounds.x = bounds.x+(grapplePoint-1)*9;
    }
}