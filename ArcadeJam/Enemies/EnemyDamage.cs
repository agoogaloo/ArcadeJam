using System.Collections.Generic;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Nodes;

namespace ArcadeJam.Enemies;

public class EnemyDamage {

    string[] collisionGroups = new string[] { "playerBullet" };
    Node enemy;
    List<Node> collisionList = new();
    IntData health;
    FloatRect bounds;
    Collision collision;



    public EnemyDamage(FloatRect bounds, Node enemy, IntData health) {
        this.health = health;
        this.enemy = enemy;
        collision = new(bounds, null, "enemy", collisionList);
    }

    public void Update() {
        collision.Update(collisionGroups);
        foreach (Node i in collisionList) {
            if (i is PlayerBullet b) {
                b.OnHit();
                health.val -= b.Damage;
            }
        }
        
    }
    public void End(){
        collision.Remove();
    }
}

