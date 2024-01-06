﻿using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LevelManager {
    static Level[] levels = { new ShipBossStage(), new Enemies.Level1(), new Level2(), new Level3() };
    static int currentLevel = 0;
    static bool started = false;

    private static Player player;

    public static void Update(GameTime gameTime) {
        levels[currentLevel].Update(gameTime);
        if (started && levels[currentLevel].Cleared) {
            Console.WriteLine("starting new level");
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
