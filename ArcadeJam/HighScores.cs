using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
			if (ArcadeGame.machineType != "cgda") {
				scores[0] = "AGOOG";
				scores[1] = "150000";
				scores[2] = "P.TDX";
				scores[3] = "68000";
				scores[4] = "CRABW";
				scores[5] = "22222";
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
		else if (time > 90 || (time > 2 && InputHandler.getButton("A").JustPressed)) {
			finished = true;
		}



	}

	private void AddNewScore(GameTime gameTime) {
		if (time < 0.5) {
			return;
		}
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
		else if (InputHandler.getButton("A").JustPressed && time > 2) {
			//save score
			saveScores();
			addingScore = false;
			time = 0.9;

		}
		stickPressed = false;
		if (InputHandler.getAnalog("L").Value > 0.5 || InputHandler.getAnalog("R").Value > 0.5 ||
		InputHandler.getAnalog("U").Value > 0.5 || InputHandler.getAnalog("D").Value > 0.5) {
			stickPressed = true;
		}


	}

	public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
		if (time < 0.66) {
			return;
		}
		spriteBatch.DrawString(Assets.font, "BAMBOOZLED BY A BUCCANEER", new Vector2(2, 10), Color.White);
		if (time < 1) {
			return;
		}
		if (addingScore) {
			spriteBatch.DrawString(Assets.font, "NEW HIGH SCORE!", new Vector2(76 - (15 * 3), 25), Color.White);
			spriteBatch.DrawString(Assets.font, "NAME:" + name, new Vector2(76 - 30, 35), Color.White);
			if ((int)(gameTime.TotalGameTime.TotalSeconds * 3) % 2 == 0) {
				spriteBatch.Draw(Assets.cursor, new Vector2(76 + cursor * 6, 37), Color.White);
			}
		}
		else {
			spriteBatch.DrawString(Assets.font, "HIGH SCORES", new Vector2(76 - (11 * 3), 30), Color.White);
		}
		for (int i = 0; i < 5; i++) {
			string text = scores[i * 2] + ":" + scores[i * 2 + 1];
			spriteBatch.DrawString(Assets.font, text, new Vector2(76 - (3 * text.Length), 50 + 6 * i), Color.White);
		}



	}

}
