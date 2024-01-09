using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LevelManager {
    static Level[] levels = { new Intro(), new ShipBossStage(), new Level1(), new Level2(), new Level3() ,new CrabBossStage()};
    static int currentLevel = 0;
    public static int speedBonus =0, maxBonus = 1500;
    static float bonusTimer = 0;
    static bool started = false;

    private static Player player;
    public static ScoreData scoreData;

    

    public static void Update(GameTime gameTime) {
        bonusTimer+=(float)gameTime.ElapsedGameTime.TotalSeconds;
        if(bonusTimer>1.5){
            bonusTimer=0;
            speedBonus-=100;
            speedBonus = Math.Max(0,speedBonus);
        }

        
        levels[currentLevel].Update(gameTime);
        if (started && levels[currentLevel].Cleared) {
            scoreData.addScore(speedBonus);
            speedBonus = 1500;
            bonusTimer = 0;
            Console.WriteLine("starting new level");
            levels[currentLevel].Cleared = false;
            currentLevel++;
            if (currentLevel >= levels.Length) {
                currentLevel = 0;
            }
            
            levels[currentLevel].Start();
            player.upgradeGun();
        }

    }
    public static void startLevels(Player playerVal) {
        player = playerVal;
        currentLevel = 0;
        levels[currentLevel].Start();
        started = true;

    }



}
