using System;
using System.Collections.Generic;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Grapple {

    private float shootSpeed = 120;
    private const int BaseDamage = 20, damageMulti = 5;
    private int damage = BaseDamage;
    private Sprite hook = new(Assets.hook), chain = new(Assets.chain);
    IntData combo;


    BoolData shooting = new(false);

    FloatRect shipBounds, hookBounds = new FloatRect(0, 0, 9, 5);
    Collision collision;
    List<Node> collisions = new();


    RectRender renderer;



    public Grapple(FloatRect shipBounds, IntData combo) {
        this.shipBounds = shipBounds;
        this.combo = combo;

        collision = new(hookBounds, null, "grapple", collisions);
        renderer = new(hook, hookBounds);
    }
    public void Update(GameTime gameTime) {
        hookBounds.x = shipBounds.Centre.X - hookBounds.width / 2;
        if (shooting.val) {
            hookBounds.y -= (float)(shootSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (hookBounds.y < 0) {
                shooting.val = false;
            }
            collision.Update("enemyBullet");
            foreach (Node i in collisions) {
                if (i is EnemyBullet b) {
                    b.OnHit();
                }

            }
            collision.Update("enemy");
            foreach (Node i in collisions) {
                if (i is Enemy e) {
                    damage = BaseDamage + ((combo.val-1) * damageMulti);
                    e.Health.val -= damage;
                    shooting.val = false;
                    combo.val++;
                    Console.WriteLine("hook dealt " + damage);
                }

            }
        }
        else {
            hookBounds.y = shipBounds.y - 5;
        }
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(gameTime, spriteBatch);

    }

    public void Shoot() {
        shooting.val = true;
    }




}
