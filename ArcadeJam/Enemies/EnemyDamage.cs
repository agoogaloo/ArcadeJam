using System.Collections.Generic;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class EnemyDamage {

    string[] collisionGroups = new string[] { "playerBullet" };
    Node enemy;
    List<Node> collisionList = new();
    IntData health;
    FloatRect bounds;
    Collision collision;

    Sprite sprite, damageTex;


    public EnemyDamage(FloatRect bounds, Node enemy, IntData health, Sprite sprite, Texture2D damageTex) :
    this(bounds, enemy, health, sprite, new Sprite(damageTex)) { }
    public EnemyDamage(FloatRect bounds, Node enemy, IntData health, Sprite sprite, Sprite damageTex) {
        this.health = health;
        this.enemy = enemy;
        this.sprite = sprite;
        this.damageTex = damageTex;
        collision = new(bounds, null, "enemy", collisionList);
    }

    public void Update() {
        collision.Update(collisionGroups);
        foreach (Node i in collisionList) {
            if (i is PlayerBullet b) {
                b.OnHit();
                health.val -= b.Damage;
                sprite.texture = damageTex.texture;
            }
        }

    }
    public void End() {
        collision.Remove();
    }
}

