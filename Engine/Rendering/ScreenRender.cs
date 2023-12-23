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
        spriteBatch.Draw(sprite.texture, position.val, Color.White);      
    }


}
