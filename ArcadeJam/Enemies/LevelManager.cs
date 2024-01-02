using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LevelManager {
    static Level[] levels = {new Level1(), new Level2(), new Level3()};
    static int currentLevel = 0;
    static bool started = false;

    public static void Update(GameTime gameTime) {
        levels[currentLevel].Update(gameTime);
        if (started && levels[currentLevel].Cleared) {
            Console.WriteLine("starting new level");
            currentLevel++;            
            if(currentLevel>=levels.Length){
                currentLevel = 0;
                levels[0] = new Level();
            }
             levels[currentLevel].Start();
        }

    }
    public static void startLevels() {
        levels[currentLevel].Start();
        started = true;
    }



}
