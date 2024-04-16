using System;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Enemies;

public class CampCrab : Node {
    Sprite sprite = new(Assets.crabEnter), crownSprite = new(Assets.crown);

    private IntData levelBonus;
    private FloatRect bounds = new(60, 150, 43, 30);
    private float speed = 3, returnSpeed = 100;
    public bool use = true;

    public CampCrab(IntData levelTime) {
        this.levelBonus = levelTime;
        
    }

	public override void Update(GameTime gameTime) {
        if(levelBonus.val>000 && bounds.y<150){
            bounds.y+=(float)(returnSpeed*gameTime.ElapsedGameTime.TotalSeconds);
        }else if (use){
            Console.WriteLine("the crabs are coming!");
            bounds.y-=(float)(speed*gameTime.ElapsedGameTime.TotalSeconds);
        }
		
	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
		spriteBatch.Draw(sprite.texture, new Vector2(bounds.x, bounds.y), Color.White);
	}

}