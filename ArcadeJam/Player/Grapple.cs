using System;
using System.Collections.Generic;
using System.Net;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;


public enum GrappleState {
    loaded, shooting, yoink, reloading, hit

}

public class Grapple {

    public GrappleState grappleState = GrappleState.loaded;

    private const float shootSpeed = 300, reloadAccel = 150;
    private const int BaseDamage = 20, damageMulti = 5, yoinkSpeed = 120;
    private float hookSpeed = 0;
    private int damage = BaseDamage;
    private Sprite hook = new(Assets.hook), chain = new(Assets.chain);
    FloatRect chainRect = new(0, 0, 3, 0);
    IntData combo;




    private Enemy target = null;

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
        //Console.WriteLine("grapple state:" + grappleState);
        hookBounds.x = shipBounds.Centre.X - hookBounds.width / 2;
        switch (grappleState) {
            case GrappleState.loaded:
                hookBounds.y = shipBounds.y - 5;
                chainRect.height = 0;
                return;
            case GrappleState.shooting:
                shootUpdate(gameTime);
                break;
            case GrappleState.yoink:
                if (shipBounds.Top < hookBounds.Bottom) {
                    grappleState = GrappleState.hit;
                    damage = BaseDamage + ((combo.val - 1) * damageMulti);
                    target.Health.val -= damage;
                    combo.val++;
                }
                break;
            case GrappleState.reloading:
                hookSpeed += (float)(reloadAccel * gameTime.ElapsedGameTime.TotalSeconds);
                hookBounds.y += (float)(hookSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (hookBounds.Bottom >= shipBounds.Top) {
                    grappleState = GrappleState.loaded;
                }

                break;
        }
        
    }

    private void shootUpdate(GameTime gameTime) {
        //moving the hook/chain
        hookBounds.y -= (float)(shootSpeed * gameTime.ElapsedGameTime.TotalSeconds);

        //destroying bullets it hits
        collision.Update("enemyBullet");
        foreach (Node i in collisions) {
            if (i is EnemyBullet b) {
                b.OnHit();
            }

        }
        //grappling enemies it hits
        collision.Update("enemy");
        foreach (Node i in collisions) {
            if (i is Enemy e) {
                target = e;
                grappleState = GrappleState.yoink;

            }

        }
        //stopping if it goes off screen
        if (hookBounds.y < 0) {
            grappleState = GrappleState.reloading;
            hookSpeed = 0;
        }
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        //setting chain size/location
        chainRect.x = hookBounds.x+3;
        chainRect.y = hookBounds.Bottom;
        chainRect.height = shipBounds.Top - hookBounds.Bottom;
        chainRenderer.Draw(spriteBatch);
        hookRenderer.Draw(gameTime, spriteBatch);
    }

    public void Shoot() {
        if (grappleState == GrappleState.loaded) {
            grappleState = GrappleState.shooting;
        }
    }


}

