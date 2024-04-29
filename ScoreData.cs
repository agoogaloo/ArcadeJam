using System;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class ScoreData {
    FloatData combo;
    public int val;
    public ScoreData (FloatData combo){
        this.combo = combo;
    }

    public void addScore(int amount){
        addScore(amount,Vector2.Zero);
    }
    public void addScore(int amount, Vector2 position){
        int adjustedScore = amount+(amount*((int)combo.val-1))/5; 
        if(adjustedScore>0 && position!=Vector2.Zero){
            NodeManager.AddNode(new ScoreEffect(position,adjustedScore));
        }
        //Console.WriteLine("score:"+amount+", adjusted amount:"+adjustedScore+", combo:"+combo.val);
        val+=adjustedScore;
    }


}
