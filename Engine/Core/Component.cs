using Engine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Core;


namespace Engine.Core;

public interface IComponent {


}


public abstract class Function : IComponent {
    protected System system = null;
    public void add(System system) {
        system.AddComponent(this);
        this.system = system;
    }
    public void remove(){
        system.RemoveComponent(this);
        this.system = null;
    }

}

public abstract class RenderComponent : Function {

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    public void add(){
        add(SystemManager.RenderSystem);
    }

}

public abstract class PhysicsComponent : Function {
    public abstract void Update(GameTime gameTime);
    public void add(){
        add(SystemManager.PhysicsSystem);
    }

}
