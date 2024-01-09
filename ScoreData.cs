using System;
using Engine.Core.Data;

namespace ArcadeJam;

public class ScoreData {
    FloatData combo;
    public int val;
    public ScoreData (FloatData combo){
        this.combo = combo;
    }

    public void addScore(int amount){
        Console.WriteLine("score:"+amount+", adjusted amount:"+(amount+(amount*((int)combo.val-1))/5)+", combo:"+combo.val);
        val+=amount+(amount*((int)combo.val-1))/5;
    }


}
