using System;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class PlayerAbilities {
    private const float yoinkAccel = 600, baseSpeed = 90, speedMulti = 30;

    private BoolData focusing = new(false), useInput;
    private FloatRect bounds;
    private Vector2Data vel;

    private FloatData speed, invincibleTime = new(1);
    private IntData combo;

    private Button shootButton = InputHandler.getButton("A"), grappleButton = InputHandler.getButton("B");
    private PlayerWeapon weapon;
    private Grapple grapple;


    public PlayerAbilities(FloatRect bounds, Vector2Data vel, FloatData speed, IntData combo, FloatData invincibleTime, BoolData useInput) {
        this.bounds = bounds;
        this.speed = speed;
        this.combo = combo;
        this.vel = vel;
        this.invincibleTime = invincibleTime;
        this.useInput = useInput;
        weapon = new Level1(bounds, focusing);
        grapple = new(bounds, combo);
    }


    public void Update(GameTime gameTime) {

        //inputs
        //shooting
        if (shootButton.Held) {
            weapon.Use();
        }
        //grappling
        if (grappleButton.Held && !shootButton.Held) {
            grapple.Shoot();
        }
        weapon.Update(gameTime);
        grapple.Update(gameTime);

        useInput.val = false;
        // updateing movement to match focus/grappling state
        switch (grapple.grappleState) {
            case GrappleState.loaded:
                useInput.val = true;
                if (grappleButton.Held && shootButton.Held) {
                    focusing.val = true;
                    speed.val = baseSpeed * 0.75f;
                }
                else {
                    focusing.val = false;
                    speed.val = baseSpeed + (speedMulti * (combo.val - 1));
                }
                break;
            case GrappleState.yoink:

                vel.val.X = 0;
                if (vel.val.Y > -10) {
                    vel.val.Y = -10;
                }
                vel.val.Y -= (float)(yoinkAccel * gameTime.ElapsedGameTime.TotalSeconds);
                invincibleTime.val = 0.3f;
                break;
            case GrappleState.hit:
                grapple.grappleState = GrappleState.loaded;

                break;
            case GrappleState.shooting:
            case GrappleState.reloading:
                vel.val = Vector2.Zero;
                break;
        }
    }
    public void Draw(GameTime gameTime, SpriteBatch batch) {
        grapple.Draw(gameTime, batch);
    }
}
