using System;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class PlayerAbilities {

    private FloatRect bounds;
    private DoubleData speed;

    private Button shoot = InputHandler.getButton("A"), grapple = InputHandler.getButton("B");
    private BasicGun gun;

    private const double reloadTime = 0.2;
    private double gunTimer = 0;

    public PlayerAbilities(FloatRect bounds, DoubleData speed) {
        this.bounds = bounds;
        this.speed = speed;
        gun = new(bounds);
    }


    public void Update(GameTime gameTime) {
        if (gunTimer>0){
            gunTimer-=gameTime.ElapsedGameTime.TotalSeconds;
        }
        if(grapple.Held){
            speed.val = 30;
            Console.WriteLine("hmm");
        }else{
            speed.val = 90;
        }
        if (shoot.Held && gunTimer<=0){
            gun.shoot();
            gunTimer = reloadTime;
        }

    }

}
