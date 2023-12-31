using System;
using System.Numerics;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Nodes;

namespace ArcadeJam;

public class BasicGun{

    private FloatRect bounds;


    public BasicGun(FloatRect bounds){
        this.bounds = bounds;
    }
    public void shoot(){
        NodeManager.AddNode(new PlayerBullet(new Vector2Data(0,-300),bounds.Centre));
    }
}