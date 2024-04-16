using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LevelManager {
    static Level[] levels;
    public static int currentLevel = 3, loops = 1;
    public static IntData speedBonus = new(0);
    public static float transitionTimer = 0;
    static float bonusTimer = 0;
    static bool started = false;
    public static FloatData BossBar { get; private set; } = new();

    private static Player player;
    public static ScoreData scoreData;

    private static CampCrab campCrab;




    public static void Update(GameTime gameTime) {

        bool cleared = true;
        for (int i = 0; i < loops && currentLevel + i < levels.Length; i++) {

            levels[currentLevel + i].Update(gameTime);
            if (!levels[currentLevel + i].Cleared) {
                cleared = false;
            }
        }
        bonusTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (bonusTimer > .5 && !cleared) {
            bonusTimer = 0;
            speedBonus.val -= 100;
            speedBonus.val = Math.Max(0, speedBonus.val);
        }


        if (started && cleared) {
            transitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            campCrab.use = false;
            if (transitionTimer >= 1.5) {
                nextLevel();
                transitionTimer = 0;
            }

        }

    }
    private static void nextLevel() {
        scoreData.addScore(speedBonus.val);
        bonusTimer = 0;
        Console.WriteLine("starting new level");

        if (currentLevel != 0) {
            player.upgradeGun();
        }
        currentLevel++;
        if (currentLevel >= levels.Length) {
            currentLevel = 0;
            loops++;
        }
        speedBonus.val = 0;
        for (int i = 0; i < loops && currentLevel + i < levels.Length; i++) {
            levels[currentLevel + i].Cleared = false;
            levels[currentLevel + i].Start(scoreData);
            speedBonus.val += levels[currentLevel + i].SpeedBonus;
        }
        BossBar.val += 1f / (levels.Length - 1);
        campCrab.use = true;
        if(currentLevel==levels.Length-1) {
            campCrab.use = false;
        }

    }
    public static void startLevels(Player playerVal) {
        player = playerVal;
        //new CrabBossStage(BossBar)
        levels = new Level[] { new Intro(),new Level1(), new Level2 (), new Level3(),new Level4(),new Level5(),
        new ShipBossStage(),new Level6(),new Level7(),new Level8(),new Level9(),new CrabBossStage(BossBar)};
        currentLevel = 0;
        levels[currentLevel].Start(scoreData);
        started = true;
        loops = 1;
        BossBar.val = 0;
        campCrab = new (speedBonus);
        campCrab.use = false;
        NodeManager.AddNode(campCrab);

    }



}
