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
    FloatRect chainRect = new(0, 0, 10, 0);
    IntData combo;



    BoolData shooting = new(false);

    FloatRect shipBounds, hookBounds = new FloatRect(0, 0, 9, 5);
    Collision collision;
    List<Node> collisions = new();


    RectRender hookRenderer;
    CropRender chainRenderer;



    public Grapple(FloatRect shipBounds, IntData combo) {
        this.shipBounds = shipBounds;
        this.combo = combo;

        collision = new(hookBounds, null, "grapple", collisions);
        hookRenderer = new(hook, hookBounds);
        chainRenderer = new(chain, chainRect);
    }
    public void Update(GameTime gameTime) {
        hookBounds.x = shipBounds.Centre.X - hookBounds.width / 2;
        if (shooting.val) {
            //moving the hook/chain
            hookBounds.y -= (float)(shootSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            chainRect.x = hookBounds.x;
            chainRect.y = hookBounds.Bottom;
            chainRect.height = shipBounds.Top - hookBounds.Bottom;


            collision.Update("enemyBullet");
            foreach (Node i in collisions) {
                if (i is EnemyBullet b) {
                    b.OnHit();
                }

            }
            collision.Update("enemy");
            foreach (Node i in collisions) {
                if (i is Enemy e) {
                    damage = BaseDamage + ((combo.val - 1) * damageMulti);
                    e.Health.val -= damage;
                    shooting.val = false;
                    combo.val++;
                    Console.WriteLine("hook dealt " + damage);
                }

            }
            //stopping if it goes off screen
            if (hookBounds.y < 0) {
                shooting.val = false;
            }
        }
        else {
            hookBounds.y = shipBounds.y - 5;
            chainRect.height = 0;
        }
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        chainRenderer.Draw(spriteBatch);
        hookRenderer.Draw(gameTime, spriteBatch);

    }

    public void Shoot() {
        shooting.val = true;
    }
}

