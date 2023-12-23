using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Assets {
    public static Texture2D player = null;
    public static void Load(ContentManager manager){
        player = manager.Load<Texture2D>("peng");

    }

}
