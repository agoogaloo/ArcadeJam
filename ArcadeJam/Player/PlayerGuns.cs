using System;
using System.Numerics;
using Engine.Core.Data;

namespace ArcadeJam;

public class BasicGun{

    private FloatRect bounds;

    public BasicGun(FloatRect bounds){
        this.bounds = bounds;
    }
    public void shoot(){
        NodeManager.AddNode(new Bullet(new Vector2Data(0,-3), new Vector2Data(bounds.Centre)));
    }
}