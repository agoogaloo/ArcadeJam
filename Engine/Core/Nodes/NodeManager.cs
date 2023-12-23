using System;
using System.Collections.Generic;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class NodeManager {
    private static readonly List<Node> nodes = new();

    public static void Update(GameTime gameTime) {
        for (int i = nodes.Count - 1; i >= 0; i--) {
            if (!nodes[i].Alive) {
                nodes.RemoveAt(i);
            }
            else if (!nodes[i].Paused) { }
            nodes[i].Update(gameTime);

        }

    }

    public static void Draw(GameTime gameTime, SpriteBatch batch) {
        for (int i = nodes.Count - 1; i >= 0; i--) {
            nodes[i].Draw(gameTime, batch);
        }

    }

    public static void AddNode(Node node) {
        nodes.Add(node);
    }

}
