using System;
using Engine.Core.Data;

namespace ArcadeJam;

public class BasicGun{

    private RectData bounds;

    public BasicGun(RectData bounds){
        this.bounds = bounds;
    }
    public void shoot(){
        Console.WriteLine("peew!");

    }
}