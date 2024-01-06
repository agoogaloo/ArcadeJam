using System;
using ArcadeJam.Weapons;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Enemies;


public class ShipBoss : Enemy {
    public ShipBoss() : base(new MoveToPoint(new Vector2(75, -50), new Vector2(75, 50),easing:2), Assets.shipBoss) {
        Health.val = 200;
        bounds.width = 23;
        bounds.height = 34;
        weapon = new Spiral(bounds);
        
        renderer = new(sprite, bounds);
    }
}
public class ShipBossMovement : EnemyMovement {

}
