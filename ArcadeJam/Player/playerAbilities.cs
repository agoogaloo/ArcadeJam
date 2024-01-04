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
	private IntData score;
	private FloatData speed, invincibleTime = new(1);
    private IntData combo;

    private Button shootButton = InputHandler.getButton("A"), grappleButton = InputHandler.getButton("B");
    private PlayerWeapon weapon;
    private Grapple grapple;


    public PlayerAbilities(FloatRect bounds, Vector2Data vel, FloatData speed, IntData combo, FloatData invincibleTime, BoolData useInput, IntData score) {
        this.bounds = bounds;
        this.speed = speed;
        this.combo = combo;
        this.vel = vel;
        this.score = score;
        this.invincibleTime = invincibleTime;
        this.useInput = useInput;
        weapon = new Level3Gun(bounds, focusing);
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
        //focusing
        if (grappleButton.Held && shootButton.Held) {
            focusing.val = true;
        }
        else { focusing.val = false; }
        weapon.Update(gameTime);
        grapple.Update(gameTime);

        useInput.val = false;
        // updateing movement to match focus/grappling state
        switch (grapple.grappleState) {
            case GrappleState.loaded:
                useInput.val = true;
                if (focusing.val) {
                    speed.val = baseSpeed * 0.75f;
                }
                else {

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
                score.val+=50+(5*combo.val);

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
