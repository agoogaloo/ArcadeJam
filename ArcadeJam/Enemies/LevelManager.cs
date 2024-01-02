using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class LevelManager {
    static Level currentLevel;
    static bool started = false;

    public static void Update(GameTime gameTime) {
        currentLevel.Update(gameTime);
        if (started && currentLevel.Cleared) {
            currentLevel = new();
            Console.WriteLine("starting new level");
        }

    }
    public static void startLevels() {
        currentLevel = new(new Enemy[]{
            new BasicEnemy(new MoveToPoint(new Vector2(160,0), new Vector2(160,50))),
            new BasicEnemy(new MoveToPoint(new Vector2(150,0), new Vector2(150,50))),
            new BasicEnemy(new MoveToPoint(new Vector2(180,0), new Vector2(180,50))),
            new BasicEnemy(new MoveToPoint(new Vector2(210,0), new Vector2(210,50))),
            new BasicEnemy(new MoveToPoint(new Vector2(200,0), new Vector2(200,50))),
            
            });
        started = true;
    }



}
