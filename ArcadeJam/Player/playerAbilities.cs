using System;
using Engine.Core.Data;
using Engine.Core.Input;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class PlayerAbilities {

    private RectData bounds;
    private DoubleData speed;

    private Button shoot = InputHandler.getButton("A"), grapple = InputHandler.getButton("B");
    private BasicGun gun;

    public PlayerAbilities(RectData bounds, DoubleData speed) {
        this.bounds = bounds;
        this.speed = speed;
        gun = new(bounds);
    }


    public void Update(GameTime gameTime) {
        if(grapple.Held){
            speed.val = 0.5;
            Console.WriteLine("hmm");
        }else{
            speed.val = 1.5;
        }
        if (shoot.Held){
            gun.shoot();
        }


    }

}
