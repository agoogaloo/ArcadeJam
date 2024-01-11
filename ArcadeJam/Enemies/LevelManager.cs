using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LevelManager {
    static Level[] levels;
    public static int currentLevel = 3, loops = 1;
    public static int speedBonus = 0, maxBonus = 1500;
    public static float transitionTimer = 0;
    static float bonusTimer = 0;
    static bool started = false;
    public static FloatData BossBar { get; private set; } = new();

    private static Player player;
    public static ScoreData scoreData;




    public static void Update(GameTime gameTime) {

        bool cleared = true;
        for (int i = 0; i < loops && currentLevel + i < levels.Length; i++) {

            levels[currentLevel + i].Update(gameTime);
            if (!levels[currentLevel + i].Cleared) {
                cleared = false;
            }
        }
        bonusTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (bonusTimer > 1.5 && !cleared) {
            bonusTimer = 0;
            speedBonus -= 100;
            speedBonus = Math.Max(0, speedBonus);
        }


        if (started && cleared) {
            transitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (transitionTimer >= 1.5) {
                nextLevel();
                transitionTimer = 0;
            }

        }

    }
    private static void nextLevel() {
        scoreData.addScore(speedBonus);
        bonusTimer = 0;
        Console.WriteLine("starting new level");
       
        currentLevel++;
        if (currentLevel >= levels.Length) {
            currentLevel = 0;
            loops++;
        }
        speedBonus = 0;
        for (int i = 0; i < loops && currentLevel + i < levels.Length; i++) {
             levels[currentLevel+i].Cleared = false;
            levels[currentLevel + i].Start(scoreData);
            speedBonus += levels[currentLevel + i].SpeedBonus;
        }
        player.upgradeGun();
        BossBar.val += 1f / (levels.Length - 1);

    }
    public static void startLevels(Player playerVal) {
        player = playerVal;
        levels = new Level[] { new Intro(),new Level6(), new Level2(), new Level3(),new Level4(),new Level5(),
        new ShipBossStage(),new Level6() ,new CrabBossStage(BossBar)};
        currentLevel = 0;
        levels[currentLevel].Start(scoreData);
        started = true;

    }



}
