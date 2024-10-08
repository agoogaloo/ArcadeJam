using System;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class LifeEffect : ScoreEffect {
	public LifeEffect(Vector2 loc) : base(loc, 4000) {
		time = 2.5;
	}
	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		Vector2 drawLoc = new((int)loc.X, (int)loc.Y);
		spriteBatch.DrawString(font, "LIFE+", drawLoc, colour);

	}
}
public class ScoreEffect : Node {
	protected Vector2 loc;
	int score;
	protected double time;
	protected Color colour = new(118, 68, 98);
	protected SpriteFont font = Assets.smallNumFont;
	public ScoreEffect(Vector2 loc, int score) {
		renderHeight = 4;
		int length = (score + "").Length;
		loc.X -= length * 2;
		this.time = score / 125f;
		this.loc = loc;
		this.score = score;
		if (score >= 3500) {
			colour = new(244, 224, 99);
		}
		else if (score >= 1000) {
			colour = new(237, 180, 161);
		}
		else if (score > 200) {
			colour = new(169, 104, 104);
		}
		if (score > 100) {
			font = Assets.font;
			loc.X -= length;
		}





	}
	public override void Update(GameTime gameTime) {
		time -= gameTime.ElapsedGameTime.TotalSeconds;
		loc.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
		if (time <= 0) {
			Alive = false;
		}


	}
	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		Vector2 drawLoc = new Vector2((int)loc.X, (int)loc.Y);
		spriteBatch.DrawString(font, score + "", drawLoc, colour);

	}


}
