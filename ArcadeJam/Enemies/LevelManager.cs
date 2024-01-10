using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LevelManager {
    static Level[] levels;
    static int currentLevel = 0;
    public static int speedBonus =0, maxBonus = 1500;
    static float bonusTimer = 0;
    static bool started = false;
    public static FloatData BossBar{get; private set;} = new();

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
            bonusTimer = 0;
            Console.WriteLine("starting new level");
            levels[currentLevel].Cleared = false;
            currentLevel++;
            if (currentLevel >= levels.Length) {
                currentLevel = 0;
            }
            
            levels[currentLevel].Start(scoreData);
            speedBonus = levels[currentLevel].SpeedBonus;
            player.upgradeGun();
            BossBar.val+=1f/(levels.Length-1);
        }

    }
    public static void startLevels(Player playerVal) {
        player = playerVal;
        levels = new Level[]{ new Intro(),new Level1(),new ShipBossStage(), new Level1(), new Level2(), new Level3() ,new CrabBossStage(BossBar)};
        currentLevel = 0;
        levels[currentLevel].Start(scoreData);
        started = true;

    }



}
