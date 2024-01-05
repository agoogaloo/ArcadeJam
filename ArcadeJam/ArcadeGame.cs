using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
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


	public static Player player;
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
		

		PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
		windowTarget = new(graphics.GraphicsDevice, width, height, false,
				GraphicsDevice.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24);
		gameTarget = new(graphics.GraphicsDevice, gameWidth, gameHeight, false,
			GraphicsDevice.PresentationParameters.BackBufferFormat,
			DepthFormat.Depth24);

		/*CycleWindowSettings();
		CycleWindowSettings();
		CycleWindowSettings();*/

		/*****REMEMBER TO UNCOMMENT THIS WHEN YOU RELEASE TO THE CABINET*****/
		//graphics.IsFullScreen = true;
		graphics.ApplyChanges();

		


		//adding all the input bindings
		InputHandler.addButtonBind("A", Keys.X);
		InputHandler.addButtonBind("B", Keys.Z);
		InputHandler.addButtonBind("A", GPadInput.Y);
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
		if (!player.Alive){
			gameOver();
		}
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

		String info = "FPS: " + Math.Round(10 / gameTime.ElapsedGameTime.TotalSeconds)/10;
        spriteBatch.DrawString(Assets.font, info, new Vector2(1, -5), Color.White);
		//spriteBatch.DrawString(Assets.font,  GamePad.GetState(PlayerIndex.One).Buttons.ToString(), new Vector2(1, 5), Color.Red);
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
		String scoreString = score.val.ToString("D6");
		//combo meter
		Rectangle comboRect = new(0,0,31,26);
		comboRect.X = ((int)player.combo.val)*31;
		if(player.combo.val==1){
			comboRect.X = 0;
		}
		double comboDecimal = player.combo.val-Math.Truncate(player.combo.val);
		Rectangle comboBarRect = new(32-(int)(32*comboDecimal),0,32,6);
		
		//actual drawing
		graphics.GraphicsDevice.SetRenderTarget(windowTarget);
		GraphicsDevice.Clear(new Color(255, 0, 0));
		spriteBatch.Begin();
		spriteBatch.Draw(gameTarget, new Vector2(38,0), Color.White);		
		spriteBatch.Draw(Assets.borders,Vector2.Zero, Color.White);
		spriteBatch.Draw(Assets.comboCounter,new Vector2(3,52),comboRect, Color.White);
		spriteBatch.DrawString(Assets.font, scoreString, new Vector2(1,27),new Color(169,104,104));
		spriteBatch.Draw(Assets.comboBar,new Vector2(4,76),comboBarRect, Color.White);
		spriteBatch.Draw(Assets.comboBarBorder,new Vector2(4,76), Color.White);

		for(int i=0;i<player.lives.val;i++){
			spriteBatch.Draw(Assets.lives,new Vector2(4+6*i,139), Color.White);
		}

		spriteBatch.End();


	}

	public void gameOver(){
		NodeManager.clearNodes();
		score.val = 0;
		player = new Player(score);

		NodeManager.AddNode(player);
		LevelManager.startLevels(player);



	}
}
