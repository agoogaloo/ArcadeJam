using Engine.Core.Input;
using Microsoft.Xna.Framework.Input;
using ArcadeJam;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Systems;
/*public class SystemManager {

    private static readonly List<DrawSystem> drawSystems = new();
    private static readonly List<UpdateSystem> updateSystems = new();

    public static RenderSystem RenderSystem { get; private set; }
    public static UISystem UISystem { get; private set; }
    public static PhysicsSystem PhysicsSystem { get; private set; }

    public static void Start() {
        RenderSystem = new();
        UISystem = new();
        PhysicsSystem = new();
        updateSystems.Add(PhysicsSystem);
        drawSystems.Add(UISystem);
        drawSystems.Add(RenderSystem);
        SortDrawSystems();
        SortUpdateSystems();
        foreach(DrawSystem i in drawSystems){
            Console.WriteLine(i.GetType());
        }
        


    }

    public static void SortUpdateSystems() {
        updateSystems.Sort(delegate (UpdateSystem x, UpdateSystem y) {
            if (x.priority > y.priority) return -1;
            else if (x.priority < y.priority) return 1;
            else return 0;
        });

    }
    public static void SortDrawSystems() {
        drawSystems.Sort(delegate (DrawSystem x, DrawSystem y) {
            if (x.priority > y.priority) return -1;
            else if (x.priority < y.priority) return 1;
            else return 0;
        });

    }

    public static void AddUpdateSystem(UpdateSystem system) {
        updateSystems.Add(system);
    }


    public static void AddDrawSystem(DrawSystem system) {
        drawSystems.Add(system);
    }

    public static UpdateSystem GetUpdateSystem<T>() where T : UpdateSystem {
        foreach (UpdateSystem s in updateSystems) {
            if (s.GetType() == typeof(T)) {
                return s;
            }
        }
        return null;
    }

    public static DrawSystem GetDrawSystem<T>() where T : DrawSystem {
        foreach (DrawSystem s in drawSystems) {
            if (s.GetType() == typeof(T)) {
                return s;
            }
        }
        return null;
    }

    public static void Update(GameTime gameTime) {
        for (int i = updateSystems.Count - 1; i >= 0; i--) {
            updateSystems[i].Update(gameTime);
        }

    }

    public static void Draw(GameTime gameTime, SpriteBatch batch) {
        for (int i = drawSystems.Count - 1; i >= 0; i--) {
            drawSystems[i].Draw(gameTime, batch);
        }

    }

}*/
