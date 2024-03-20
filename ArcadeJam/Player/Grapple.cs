using System;
using System.Collections.Generic;
using System.Net;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;


public enum GrappleState {
    loaded, shooting, yoink, reloading, hit

}

public class Grapple {

    public GrappleState grappleState = GrappleState.loaded;

    private const float shootSpeed = 400, reloadAccel = 160;
    private const int BaseDamage = 20, damageMulti = 2, yoinkSpeed = 120;
    private float hookSpeed = 0;
    private int damage = BaseDamage;
    private Sprite hook = new(Assets.hook), chain = new(Assets.chain);
    FloatRect chainRect = new(-50, 0, 3, 0);
    FloatData combo;
    IntData grappleDamage;




    private IGrappleable target = null;

    FloatRect shipBounds, hookBounds = new FloatRect(0, 0, 9, 5);
    Collision collision;
    List<Node> collisions = new();


    RectRender hookRenderer;
    CropRender chainRenderer;



    public Grapple(FloatRect shipBounds, FloatData combo, IntData grappleDamage) {
        this.shipBounds = shipBounds;
        this.combo = combo;
        this.grappleDamage = grappleDamage;

        collision = new(hookBounds, null, "grapple", collisions);
        hookRenderer = new(hook, hookBounds);
        chainRenderer = new(chain, chainRect);
    }
    public void Update(GameTime gameTime) {
        //Console.WriteLine("grapple state:" + grappleState);
        hookBounds.x = shipBounds.Centre.X - hookBounds.width / 2;
        grappleDamage.val = (int)(BaseDamage + ((combo.val - 1) * damageMulti));
        switch (grappleState) {
            case GrappleState.loaded:
                hookBounds.y = shipBounds.y - 5;
                chainRect.height = 0;
                return;

            case GrappleState.shooting:
                shootUpdate(gameTime);
                break;

            case GrappleState.yoink:
                if (shipBounds.Top < hookBounds.Bottom || shipBounds.Top<=0) {
                    grappleState = GrappleState.hit;
                }
                break;

            case GrappleState.hit:
                grappleState = GrappleState.loaded;
                target.GrappleHit(grappleDamage.val);
                Assets.grappleHit.CreateInstance();
                Assets.grappleHit.Play();
                if (combo.val == 1) {
                    combo.val++;
                }
                else {
                    combo.val = Math.Min((int)Math.Truncate(combo.val) + 2, 7);
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
            if (i is EnemyCircleBullet c) {
                c.OnHit();
            }



        }
        //grappling enemies it hits
        collision.Update("grapple");
        foreach (Node i in collisions) {
            if (i is IGrappleable e) {
                target = e;
                grappleState = GrappleState.yoink;
            }
        }
        collision.Update("enemy");
        if (grappleState == GrappleState.yoink) {
            target.GrappleStun();
        }
        
        else if (collisions.Count > 0) {
            grappleState = GrappleState.reloading;
            hookSpeed = 75;
        }
        
        //stopping if it goes off screen
        else if (hookBounds.y < 0) {
            grappleState = GrappleState.reloading;
            hookSpeed = 75;
        }
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        //setting chain size/location
        chainRect.x = hookBounds.x + 3;
        chainRect.y = hookBounds.Bottom;
        chainRect.height = shipBounds.Top - hookBounds.Bottom;
        chainRenderer.Draw(spriteBatch);
        hookRenderer.Draw( spriteBatch);
    }

    public void Shoot() {
        if (grappleState == GrappleState.loaded) {
            grappleState = GrappleState.shooting;
            Assets.grappleShoot.Play();
        }
    }


}

