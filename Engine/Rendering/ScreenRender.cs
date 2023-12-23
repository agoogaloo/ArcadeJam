using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Components;
public class ScreenRender : RenderComponent {
	private Sprite sprite;
	private Vector2Comp position;


	public ScreenRender(Sprite sprite,Vector2Comp position){
        this.sprite = sprite;
        this.position = position;
    }     

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        spriteBatch.Draw(sprite.texture, position.val, Color.White);
        
    }

}
