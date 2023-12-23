using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Engine.Core.Systems;
/*public class RenderSystem : DrawSystem {

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
                foreach (RenderComponent component in components.Cast<RenderComponent>()) {
                        component.Draw(gameTime, spriteBatch);
                }
        }

}
public class UISystem : RenderSystem {
        public UISystem():base(){
                priority = 9;
        }
}

public class PhysicsSystem : UpdateSystem {


	public override void Update(GameTime gameTime) {
                InputHandler.Update();                
                foreach (PhysicsComponent component in components.Cast<PhysicsComponent>()) {
                        component.Update(gameTime);
                }
        }

}*/



