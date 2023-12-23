using Engine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Core;
using System;


namespace Engine.Core;




public abstract class Function {
    protected System system = null;
    public void Add(System system) {
        system.AddComponent(this);
        this.system = system;
    }
    public void Remove(){
        system.RemoveComponent(this);
        this.system = null;
    }

	public abstract void Add();
}

public abstract class RenderComponent : Function {

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    public override void Add(){
        Add(SystemManager.RenderSystem);
    }

}

public abstract class PhysicsComponent : Function {
    public abstract void Update(GameTime gameTime);
    public override void Add(){
        Add(SystemManager.PhysicsSystem);
    }

}
