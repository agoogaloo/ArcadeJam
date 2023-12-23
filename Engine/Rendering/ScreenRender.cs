using System;
using System.Collections.Generic;
using Engine.Core.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Components;
public class ScreenRender  {
	private Sprite sprite;
	private Vector2Data position;


	public ScreenRender(Sprite sprite,Vector2Data position){
        this.sprite = sprite;
        this.position = position;
    }     

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        int x =(int)Math.Round(position.val.X), y = (int)Math.Round(position.val.Y);
        spriteBatch.Draw(sprite.texture, new Vector2(x,y), Color.White);      
    }


}
