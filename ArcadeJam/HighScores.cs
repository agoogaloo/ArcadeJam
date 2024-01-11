using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using Engine.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ArcadeJam;

public class HighScores {
    private char[] letters = {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
    ,'!','<','>',':','.',' ','_','-'};
    private String defaultScore = "0,-----,", scoresPath = "highScores.txt", name = "-----";
    private String[] scores = new String[10];
    private int score, cursor, currentLetter;

    private double time = 0;

    private bool addingScore = false, stickPressed = false;
    public bool finished = false;

    public HighScores(int score) {
        this.score = score;
        LoadScores();
        foreach (String i in scores) {
            Console.WriteLine(i + ", ");
        }
        if (int.Parse(scores[9]) < score) {
            addingScore = true;
        }
    }
    private void LoadScores() {
        if (File.Exists(scoresPath)) {
            scores = File.ReadAllText(scoresPath).Split(",").Take(10).ToArray();


        }
        else {
            //filling the highscores with empty values
            for (int i = 0; i < 5; i++) {
                scores[i * 2] = "-----";
                scores[i * 2 + 1] = "0";
            }
        }

    }
    private void saveScores() {
        String text = "";
        bool added = false;
        for (int i = 0; i < 5; i++) {
            if (int.Parse(scores[i * 2 + 1]) < score && !added) {
                text += name + "," + score + ",";
                added = true;
            }
            text += scores[i * 2] + "," + scores[i * 2 + 1] + ",";

        }
        File.WriteAllText(scoresPath, text);
        LoadScores();
    }
    public void Update(GameTime gameTime) {
        time += gameTime.ElapsedGameTime.TotalSeconds;
        if (addingScore) {
            AddNewScore(gameTime);
        }
        else if (time > 90 || (time > 1 && InputHandler.getButton("A").JustPressed)) {
            finished = true;
        }



    }

    private void AddNewScore(GameTime gameTime) {
        if (InputHandler.getAnalog("U").Value > 0.5 && !stickPressed) {
            currentLetter++;
            if (currentLetter >= letters.Length) {
                currentLetter = 0;
            }
            name = name.Remove(cursor, 1).Insert(cursor, letters[currentLetter].ToString());
        }
        else if (InputHandler.getAnalog("D").Value > 0.5 && !stickPressed) {
            currentLetter--;
            if (currentLetter < 0) {
                currentLetter = letters.Length - 1;
            }
            name = name.Remove(cursor, 1).Insert(cursor, letters[currentLetter].ToString());
        }
        else if (InputHandler.getAnalog("L").Value > 0.5 && !stickPressed) {
            cursor--;
            cursor = Math.Max(0, cursor);
            currentLetter = Array.IndexOf(letters, name[cursor]);

        }
        else if (InputHandler.getAnalog("R").Value > 0.5 && !stickPressed) {
            cursor++;
            cursor = Math.Min(4, cursor);
            currentLetter = Array.IndexOf(letters, name[cursor]);
        }
        else if (InputHandler.getButton("A").JustPressed && time > 1) {
            //save score
            saveScores();
            addingScore = false;
            time = 0;

        }
        stickPressed = false;
        if (InputHandler.getAnalog("L").Value > 0.5 || InputHandler.getAnalog("R").Value > 0.5 ||
        InputHandler.getAnalog("U").Value > 0.5 || InputHandler.getAnalog("D").Value > 0.5) {
            stickPressed = true;
        }


    }

    public void Draw(SpriteBatch spriteBatch) {
        if (addingScore) {
            spriteBatch.DrawString(Assets.font, "NEW HIGH SCORE!", new Vector2(5, 10), Color.White);
            spriteBatch.DrawString(Assets.font, "NAME:"+name, new Vector2(10, 20), Color.White);
             spriteBatch.Draw(Assets.cursor, new Vector2(10+30+cursor*6,22),Color.White);
        }
        for (int i = 0; i <5; i++) {
            spriteBatch.DrawString(Assets.font, scores[i * 2] + ":" + scores[i * 2 + 1], new Vector2(5, 30 + 6 * i), Color.White);
        }
       


    }

}
