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
    public static  List<Node> Nodes{get;private set;} = new();
    private static readonly List<Node> nodesToAdd = new();


    public static void Update(GameTime gameTime) {
        //updating nodes
        for (int i = Nodes.Count - 1; i >= 0; i--) {
            if (!Nodes[i].Alive) {
                Nodes[i].End();
                Nodes.RemoveAt(i);

            }
            else if (!Nodes[i].Paused) {
                Nodes[i].Update(gameTime);
            }
        }
        //adding new nodes
        if (nodesToAdd.Count > 0) {
            foreach (Node i in nodesToAdd) {
                Nodes.Add(i);
            }
            nodesToAdd.Clear();
            //sorting them so the render order is right
            Nodes.Sort(delegate (Node x, Node y) {
                return -x.renderHeight.CompareTo(y.renderHeight);
            });
        }



    }

    public static void Draw(GameTime gameTime, SpriteBatch batch) {

        for (int i = Nodes.Count - 1; i >= 0; i--) {
            Nodes[i].Draw(gameTime, batch);
        }
        String info = "NODES: " + Nodes.Count + "\nFPS: " + Math.Round(10 / gameTime.ElapsedGameTime.TotalSeconds) / 10;
        //batch.DrawString(Assets.font, info, new Vector2(1, -5), Color.White);

    }
    public static void AddNode(Node node) {
        nodesToAdd.Add(node);

    }
    public static void clearNodes() {
        for (int i = Nodes.Count - 1; i >= 0; i--) {
            Nodes[i].End();
            Nodes.RemoveAt(i);
        }
        nodesToAdd.Clear();
    }
}
