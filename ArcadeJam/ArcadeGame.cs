﻿using System;
using System.ComponentModel;
using ArcadeJam.Enemies;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Input;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Engine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Player;


namespace ArcadeJam;

public enum WindowMode {
	FScreenPPerfect,
	FScreenBars,
	WindowedPPerfect,
	WindowedBars,

}

public class ArcadeGame : Game {


	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;
	private RenderTarget2D windowTarget, gameTarget;


	public const int width = 200, height = 150, gameWidth = 153, gameHeight = 150;
	private int screenWidth = width * 3, screenHeight = height * 3;
	private WindowMode windowMode = WindowMode.WindowedPPerfect;
	private bool windowToggled = false;

	IntData score = new();


	Player player;
	public ArcadeGame() {
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
		Window.AllowUserResizing = true;

	}

	protected override void Initialize() {



		base.Initialize();

		//window settings
		graphics.PreferredBackBufferWidth = width * 3;
		graphics.PreferredBackBufferHeight = height * 3;
		graphics.ApplyChanges();

		PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
		windowTarget = new(graphics.GraphicsDevice, width, height, false,
				GraphicsDevice.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24);
		gameTarget = new(graphics.GraphicsDevice, gameWidth, gameHeight, false,
			GraphicsDevice.PresentationParameters.BackBufferFormat,
			DepthFormat.Depth24);

		


		//adding all the input bindings
		InputHandler.addButtonBind("A", Keys.X);
		InputHandler.addButtonBind("B", Keys.Z);
		InputHandler.addButtonBind("A", GPadInput.A);
		InputHandler.addButtonBind("B", GPadInput.B);
		InputHandler.addAnalogBind("L", Keys.Left);
		InputHandler.addAnalogBind("R", Keys.Right);
		InputHandler.addAnalogBind("U", Keys.Up);
		InputHandler.addAnalogBind("D", Keys.Down);
		InputHandler.addAnalogBind("L", GPadInput.LStickLeft);
		InputHandler.addAnalogBind("R", GPadInput.LStickRight);
		InputHandler.addAnalogBind("U", GPadInput.LStickUp);
		InputHandler.addAnalogBind("D", GPadInput.LStickDown);

		player = new Player(score);

		NodeManager.AddNode(player);
		LevelManager.startLevels(player);


	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);

		Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
		pixel.SetData(new Color[] { Color.White });

		Assets.Load(Content, pixel);
	}

	protected override void Update(GameTime gameTime) {
		base.Update(gameTime);
		InputHandler.Update();

		NodeManager.Update(gameTime);
		LevelManager.Update(gameTime);
		//changing window modes if they press f11
		if (Keyboard.GetState().IsKeyDown(Keys.F12)) {
			if (!windowToggled) {
				CycleWindowSettings();
			}
			windowToggled = true;
		}
		else {
			windowToggled = false;
		}


	}
	private void CycleWindowSettings() {
		windowMode += 1;
		if (windowMode > WindowMode.WindowedBars) {
			windowMode = 0;
		}
		Console.WriteLine("window mode set to: " + windowMode);

		//setting fullscreen options
		if ((int)windowMode == 0) {
			Window.IsBorderless = true;
			graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			graphics.ApplyChanges();
		}
		//setting windowed options
		else if ((int)windowMode == 2) {

			graphics.PreferredBackBufferWidth = width * 3;
			graphics.PreferredBackBufferHeight = height * 3;
			Window.IsBorderless = false;
			graphics.IsFullScreen = false;

			graphics.ApplyChanges();
		}

	}

	protected override void Draw(GameTime gameTime) {
		drawGame(gameTime);
		drawBorder();

		//drawing game+ui to the screen
		graphics.GraphicsDevice.SetRenderTarget(null);
		screenWidth = GraphicsDevice.Viewport.Width;
		screenHeight = GraphicsDevice.Viewport.Height;

		Rectangle destinationRect;
		float scale;

		scale = Math.Min((float)screenWidth / width, (float)screenHeight / height);
		//making scale pixel perfect
		if (windowMode == WindowMode.FScreenPPerfect || windowMode == WindowMode.WindowedPPerfect) {
			scale = (int)scale;
		}

		int x = (int)((screenWidth - width * scale) / 2);
		int y = (int)((screenHeight - height * scale) / 2);
		destinationRect = new(x, y, (int)(width * scale), (int)(height * scale));



		spriteBatch.Begin(samplerState: SamplerState.PointClamp);
		GraphicsDevice.Clear(Color.Black);

		spriteBatch.Draw(windowTarget, destinationRect, Color.White);
		spriteBatch.End();


		base.Draw(gameTime);
	}
	private void drawGame(GameTime gameTime) {
		//drawing the actual game 
		graphics.GraphicsDevice.SetRenderTarget(gameTarget);
		GraphicsDevice.Clear(new Color(44, 33, 55));
		spriteBatch.Begin();
		NodeManager.Draw(gameTime, spriteBatch);
		spriteBatch.End();

	}
	private void drawBorder() {
		Rectangle comboRect = new(0,0,31,26);
		comboRect.X = (player.combo.val-1)*31;
		//drawing the ui stuff around the game
		graphics.GraphicsDevice.SetRenderTarget(windowTarget);

		GraphicsDevice.Clear(new Color(255, 0, 0));
		spriteBatch.Begin();
		spriteBatch.Draw(gameTarget, new Vector2(38,0), Color.White);

		String scoreString = score.val.ToString("D6");
		spriteBatch.Draw(Assets.borders,Vector2.Zero, Color.White);
		spriteBatch.Draw(Assets.comboCounter,new Vector2(3,52),comboRect, Color.White);
		spriteBatch.DrawString(Assets.font, scoreString, new Vector2(1,27),new Color(169,104,104));
		//NodeManager.Draw(gameTime, spriteBatch);
		spriteBatch.End();



	}
}
