using System;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam.Weapons;

public class Straight {
    private DoubleData delay ;
    private FloatRect pos;
    private double timeLeft = 0;

    public Straight(FloatRect pos,double delay = 1 ):this(pos,new DoubleData(delay)){}
    public Straight(FloatRect pos,DoubleData delay){
        this.delay = delay;
        this.pos = pos;
        timeLeft = delay.val;
    }

    public void Update(GameTime gameTime){
        timeLeft-=gameTime.ElapsedGameTime.TotalSeconds;
        if (timeLeft<0){
            timeLeft = delay.val;
            NodeManager.AddNode(new EnemyBullet(new Vector2Data(0,60),pos.Centre));
        }


    }

}
