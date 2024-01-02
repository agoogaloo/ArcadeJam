using System;
using System.Collections;
using System.Collections.Generic;
using ArcadeJam;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Nodes;

public class NodeManager {
    private static readonly List<Node> nodes = new();
    private static readonly List<Node> nodesToAdd = new();


    public static void Update(GameTime gameTime) {
        //updating nodes
        for (int i = nodes.Count - 1; i >= 0; i--) {
            if (!nodes[i].Alive) {
                nodes[i].End();
                nodes.RemoveAt(i);

            }
            else if (!nodes[i].Paused) {
                nodes[i].Update(gameTime);
            }
        }
        //adding new nodes
        if (nodesToAdd.Count > 0) {
            foreach (Node i in nodesToAdd) {
                nodes.Add(i);
            }
            nodesToAdd.Clear();
            //sorting them so the render order is right
            nodes.Sort(delegate (Node x, Node y) {
                return -x.renderHeight.CompareTo(y.renderHeight);
            });
        }



    }

    public static void Draw(GameTime gameTime, SpriteBatch batch) {

        for (int i = nodes.Count - 1; i >= 0; i--) {
            nodes[i].Draw(gameTime, batch);
        }
        String info = "NODES: " + nodes.Count + "\nFPS: " + Math.Round(10 / gameTime.ElapsedGameTime.TotalSeconds)/10;
        batch.DrawString(Assets.font, info, new Vector2(1, -5), Color.White);

    }
    public static void AddNode(Node node) {
        nodesToAdd.Add(node);

    }
}
