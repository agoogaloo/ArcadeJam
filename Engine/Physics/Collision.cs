using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class Collision {
    private static Dictionary<String, Dictionary<FloatRect, Node>> groupDict = new();

    private String group;
    String Group{get=>group;set{
        groupDict[group].Remove(bounds);
        group = value;
        groupDict[group].Add(bounds, node);    
    }}
    private FloatRect bounds;
    private Node node;
    private List<Node> collisions;

    public Collision(FloatRect bounds, Node node, String group = "default", List<Node> collisions = null) {
        this.group = group;
        this.bounds = bounds;
        this.node = node;
        this.collisions = collisions;
        if (!groupDict.ContainsKey(group)) {
            groupDict.Add(group, new Dictionary<FloatRect, Node>());
        }
        groupDict[group].Add(bounds, node);

    }
    public void Update(GameTime gameTime) {
        Update(gameTime, new string[] { group });
    }

    public void Update(GameTime gameTime, String[] groups) {
        if (!node.Alive) {
            groupDict[group].Remove(bounds);
            Console.WriteLine("removing dead "+node.ToString());
            return;
        }
        if (collisions == null) {
            return;
        }

        collisions.Clear();
        foreach (String checkgroup in groups) {
            if (groupDict.ContainsKey(checkgroup)) {
                foreach (KeyValuePair<FloatRect, Node> n in groupDict[checkgroup]) {
                    if (n.Key.Intersects(bounds)) {
                        collisions.Add(n.Value);
                    }
                }
            }
        }

    }
}

public interface ICollisionAction{
    public abstract void onCollide();
}
