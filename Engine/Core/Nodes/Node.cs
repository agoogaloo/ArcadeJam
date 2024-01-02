using System;
using System.Collections.Generic;
using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Nodes;

public abstract class Node {
    public bool Alive { get; set;} = true;
    public bool Paused {get;set;} = false;

	public int renderHeight{get;protected set;} = 2;

	

	public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    /// <summary>
    /// called when the node is removed from the nodemanager
    /// </summary>
    public virtual void End(){}
}
