using System;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Input;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class Player:IComponent{
    Vector2Comp Position {get;set;}= new(new Vector2(150, 50));
	Vector2Comp Vel {get;set;}= new(new Vector2(0,0));

    public Player() {

		Sprite spr = null;
		ScreenRender renderer = new(spr, Position);
		VelMovement mover = new(Vel, Position);
		Gravity gravity = new(Vel);
		Jump jump = new(Vel,InputHandler.getButton("test1"));

		//renderer.add();
		mover.add();
		gravity.add();
		jump.add();
    }



}

