using Engine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Core;
using System;


namespace Engine.Core;

public interface IDrawable {
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch);

}

public interface IUpdate {
        public void Update(GameTime gameTime);
}
