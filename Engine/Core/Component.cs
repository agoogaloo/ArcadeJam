using Engine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Core;


namespace Engine.Core;

public interface IComponent {

}


public abstract class Function : IComponent {
    protected Entity owner;

    public virtual void Register(Entity owner) {

        this.owner = owner;


    }

}

public abstract class RenderComponent : Function {

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    public override void Register(Entity owner) {
        base.Register(owner);
        SystemManager.Instance.RenderSystem.AddComponent(this);

    }

}

public abstract class PhysicsComponent : Function {
    public abstract void Update(GameTime gameTime);
    public override void Register(Entity owner) {
        base.Register(owner);
        SystemManager.Instance.PhysicsSystem.AddComponent(this);

    }

}
