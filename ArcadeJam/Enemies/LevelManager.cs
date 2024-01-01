using System;
using ArcadeJam.Enemies;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LevelManager {
    static Level currentLevel;
    static bool started = false;

    public static void Update(GameTime gameTime){
        currentLevel.Update(gameTime);
        if (started && currentLevel.Cleared){
            currentLevel = new();
            Console.WriteLine("starting new level");
        }

    }
    public static void startLevels(){
        currentLevel = new();
        started = true;
    }
    


}
