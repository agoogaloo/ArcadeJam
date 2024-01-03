using System;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class PlayerAbilities {

    private FloatRect bounds;
    private DoubleData speed;
    IntData combo;

    private Button shootButton = InputHandler.getButton("A"), grappleButton = InputHandler.getButton("B");
    private BasicGun gun;
    Grapple grapple;

    private const double reloadTime = 0.2;
    private double gunTimer = 0;

    public PlayerAbilities(FloatRect bounds, DoubleData speed, IntData combo) {
        this.bounds = bounds;
        this.speed = speed;
        this.combo = combo;
        gun = new(bounds);
        grapple = new(bounds, combo);
    }


    public void Update(GameTime gameTime) {
        //updating reload time
        if (gunTimer > 0) {
            gunTimer -= gameTime.ElapsedGameTime.TotalSeconds;
        }
        grapple.Update(gameTime);
        //shooting
        if (shootButton.Held && gunTimer <= 0) {
            gun.shoot();
            gunTimer = reloadTime;
        }

        //grappling
        if (grappleButton.Held && !shootButton.Held) {
            grapple.Shoot();
        }

        //focusing
        if (grappleButton.Held && shootButton.Held) {
            speed.val = 30;
        }
        //normal movement
        else {
            speed.val = 120 * combo.val / 2;
        }

    }
    public void Draw(GameTime gameTime, SpriteBatch batch) {
        grapple.Draw(gameTime, batch);
    }
}
