using System;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Input;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;

namespace ArcadeJam;

public class Player : IComponent {
	Vector2Comp Position { get; set; } = new(new Vector2(150, 50));
	Vector2Comp Vel { get; set; } = new(new Vector2(0, 0));
	DoubleComp moveSpeed { get; set; } = new(0.5);

	public Player() {

		Sprite spr = new Sprite(Assets.player);
		ScreenRender renderer = new(spr, Position);

		VelMovement mover = new(Vel, Position);
		//Gravity gravity = new(Vel);
		//Jump jump = new(Vel,InputHandler.getButton("test1"));

		renderer.add();
		new InputMovement(Vel, moveSpeed).add();
		mover.add();

	}

	public void dispose() {

	}



}

