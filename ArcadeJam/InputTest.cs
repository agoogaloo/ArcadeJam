using System;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Input;
using Microsoft.Xna.Framework;

namespace ArcadeJam;
public class InputTest {
    private Button test1, test2;
    private Analog test3, test4;

    public InputTest() : this(InputHandler.getButton("test1"), InputHandler.getButton("test2"),
        InputHandler.getAnalog("test3"), InputHandler.getAnalog("test4")) {


    }

    public InputTest(Button test1, Button test2, Analog test3, Analog test4) {
        this.test1 = test1;
        this.test2 = test2;
        this.test3 = test3;
        this.test4 = test4;
    }

    public  void Update(GameTime gameTime) {
        Console.WriteLine("1:" + test1.Held + " 2:" + test2.Held + " 3:" + test3.Value + " 4:" + test4.Value);

    }
}

