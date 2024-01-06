﻿using System;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class PlayerAbilities {
    private PlayerWeapon[] upgrades;
    private const float yoinkAccel = 600, baseSpeed = 80, speedMulti = 20, comboDecay = 5;

    private BoolData focusing = new(false), useInput;
    private FloatRect bounds;
    private Vector2Data vel;
    private IntData score, grappleDamage;
    private FloatData speed, invincibleTime = new(1);
    private FloatData combo;

    private Button shootButton = InputHandler.getButton("A"), grappleButton = InputHandler.getButton("B");

    public int weapon;
    private Grapple grapple;


    public PlayerAbilities(FloatRect bounds, Vector2Data vel, FloatData speed, FloatData combo, FloatData invincibleTime, BoolData useInput, 
    IntData score, IntData grappleDamage) {
        this.bounds = bounds;
        this.speed = speed;
        this.combo = combo;
        this.vel = vel;
        this.score = score;
        this.grappleDamage = grappleDamage;
        this.invincibleTime = invincibleTime;
        this.useInput = useInput;
        upgrades = new PlayerWeapon[] { new Level1Gun(bounds, focusing), new Level2Gun(bounds, focusing), new Level3Gun(bounds, focusing) };
        weapon = 0;
        grapple = new(bounds, combo, grappleDamage);
    }


    public void Update(GameTime gameTime) {

        //inputs
        //shooting
        if (shootButton.Held) {
            upgrades[weapon].Use();
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
        upgrades[weapon].Update(gameTime);
        grapple.Update(gameTime);

        useInput.val = false;
        // updateing movement to match focus/grappling state
        switch (grapple.grappleState) {
            case GrappleState.loaded:
                useInput.val = true;
                speed.val = baseSpeed + (speedMulti * (combo.val - 1));
                if (focusing.val) {
                    speed.val *= 0.6f;
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
                score.val += 50 + (5 * (int)combo.val);
                break;
            case GrappleState.shooting:
            case GrappleState.reloading:
                vel.val = Vector2.Zero;
                break;
        }

        combo.val = Math.Max(1, combo.val - (float)gameTime.ElapsedGameTime.TotalSeconds / comboDecay);

    }
    public void Draw(GameTime gameTime, SpriteBatch batch) {
        grapple.Draw(gameTime, batch);
    }
}