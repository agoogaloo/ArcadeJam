using System;
using Engine.Core.Data;
using Engine.Core.Input;
using Engine.Core.Nodes;
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


	public static string machineType = "normal";//used to change keybinds and other stuff for different machines
	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;
	private RenderTarget2D windowTarget, gameTarget;


	public const int width = 200, height = 150, gameWidth = 153, gameHeight = 150;
	private int screenWidth = width * 3, screenHeight = height * 3;
	private WindowMode windowMode = WindowMode.WindowedPPerfect;
	private bool windowToggled = false;

	ScoreData score;
	FloatData combo = new();


	public static Player player;
	private HighScores gameOverScreen;
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

		//setting fullscreen looks bad on most machines that arent the gdc cabinet
		if (machineType == "gdc") {
			graphics.IsFullScreen = true;
		}
		graphics.ApplyChanges();

		//setting special controls for cgda machine
		if (machineType == "cgda") {
			//left side controls
			InputHandler.addAnalogBind("U", Keys.W);
			InputHandler.addAnalogBind("L", Keys.A);
			InputHandler.addAnalogBind("D", Keys.S);
			InputHandler.addAnalogBind("R", Keys.D);

			InputHandler.addButtonBind("A", Keys.R);
			InputHandler.addButtonBind("A", Keys.T);
			InputHandler.addButtonBind("A", Keys.Y);
			InputHandler.addButtonBind("A", Keys.C);

			InputHandler.addButtonBind("B", Keys.F);
			InputHandler.addButtonBind("B", Keys.G);
			InputHandler.addButtonBind("B", Keys.H);
			InputHandler.addButtonBind("B", Keys.V);

			InputHandler.addAnalogBind("L", Keys.Left);
			InputHandler.addAnalogBind("R", Keys.Right);
			InputHandler.addAnalogBind("U", Keys.Up);
			InputHandler.addAnalogBind("D", Keys.Down);

			InputHandler.addButtonBind("A", Keys.U);
			InputHandler.addButtonBind("A", Keys.I);
			InputHandler.addButtonBind("A", Keys.O);
			InputHandler.addButtonBind("A", Keys.N);

			InputHandler.addButtonBind("B", Keys.J);
			InputHandler.addButtonBind("B", Keys.K);
			InputHandler.addButtonBind("B", Keys.L);
			InputHandler.addButtonBind("B", Keys.M);
		}
		else {
			//adding all the input bindings
			InputHandler.addButtonBind("A", Keys.X);
			InputHandler.addButtonBind("B", Keys.Z);
			InputHandler.addButtonBind("A", GPadInput.Y);
			InputHandler.addButtonBind("B", GPadInput.B);
			InputHandler.addButtonBind("A", GPadInput.A);
			InputHandler.addButtonBind("B", GPadInput.X);
			InputHandler.addButtonBind("A", GPadInput.LTrigger);
			InputHandler.addButtonBind("B", GPadInput.RTrigger);
			InputHandler.addButtonBind("B", GPadInput.LShoulder);
			InputHandler.addButtonBind("A", GPadInput.RShoulder);

			InputHandler.addAnalogBind("L", Keys.Left);
			InputHandler.addAnalogBind("R", Keys.Right);
			InputHandler.addAnalogBind("U", Keys.Up);
			InputHandler.addAnalogBind("D", Keys.Down);
			InputHandler.addAnalogBind("L", GPadInput.LStickLeft);
			InputHandler.addAnalogBind("R", GPadInput.LStickRight);
			InputHandler.addAnalogBind("U", GPadInput.LStickUp);
			InputHandler.addAnalogBind("D", GPadInput.LStickDown);

			InputHandler.addAnalogBind("L", GPadInput.DLeft);
			InputHandler.addAnalogBind("R", GPadInput.DRight);
			InputHandler.addAnalogBind("U", GPadInput.DUp);
			InputHandler.addAnalogBind("D", GPadInput.DDown);
		}


		score = new(combo);
		player = new Player(score, combo);

		NodeManager.AddNode(player);
		LevelManager.startLevels(player);
		LevelManager.scoreData = score;

		Window.Title = "Captain Grapple";


	}

	protected override void LoadContent() {

		spriteBatch = new SpriteBatch(GraphicsDevice);

		Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
		pixel.SetData(new Color[] { Color.White });

		Assets.Load(Content, pixel);
	}

	protected override void Update(GameTime gameTime) {
		base.Update(gameTime);
		InputHandler.Update(gameTime);

		NodeManager.Update(gameTime);
		if (!player.Alive) {
			gameOver(gameTime);
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
		if (machineType == "cgda" && Mouse.GetState().MiddleButton == ButtonState.Pressed) {
			Console.WriteLine("sdkfjhkjfd");
			Exit();
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

		String info = "FPS: " + Math.Round(10 / gameTime.ElapsedGameTime.TotalSeconds) / 10;
		//spriteBatch.DrawString(Assets.font, info, new Vector2(1, -5), Color.White);
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
		if (gameOverScreen != null) {
			gameOverScreen.Draw(spriteBatch, gameTime);
		}
		spriteBatch.End();

	}
	private void drawBorder() {
		String scoreString = score.val.ToString("D6");
		String bonusString = "+" + LevelManager.speedBonus.val.ToString("D4");
		//combo meter
		Rectangle comboRect = new(0, 0, 31, 26);
		comboRect.X = ((int)player.combo.val) * 31;
		if (player.combo.val == 1) {
			comboRect.X = 0;
		}
		double comboDecimal = player.combo.val - Math.Truncate(player.combo.val);
		Rectangle comboBarRect = new(32 - (int)(32 * comboDecimal), 0, 32, 6);

		//actual drawing
		graphics.GraphicsDevice.SetRenderTarget(windowTarget);
		GraphicsDevice.Clear(new Color(169, 104, 104));
		spriteBatch.Begin();
		spriteBatch.Draw(gameTarget, new Vector2(38, 0), Color.White);
		spriteBatch.Draw(Assets.borders, Vector2.Zero, Color.White);
		spriteBatch.DrawString(Assets.font, scoreString, new Vector2(1, 27), new Color(169, 104, 104));
		if ((int)(LevelManager.transitionTimer * 8) % 2 == 0) {
			spriteBatch.DrawString(Assets.font, bonusString, new Vector2(4, 54), new Color(169, 104, 104));
			spriteBatch.DrawString(Assets.font, LevelManager.loops + "-" + LevelManager.currentLevel, new Vector2(9, 6), new Color(169, 104, 104));

		}

		//combo bar
		spriteBatch.Draw(Assets.comboBar, new Vector2(4, 102), comboBarRect, Color.White);
		spriteBatch.Draw(Assets.comboBarBorder, new Vector2(4, 102), Color.White);
		spriteBatch.Draw(Assets.comboCounter, new Vector2(3, 78), comboRect, Color.White);
		//boss health
		Rectangle barRect = new(0, 0, 4, (int)(Assets.bossBar.Height * LevelManager.BossBar.val));
		spriteBatch.Draw(Assets.bossBar, new Vector2(194, 11 + Assets.bossBar.Height - barRect.Height), barRect, Color.White);
		spriteBatch.Draw(Assets.bossBarFrame, new Vector2(173, 0), Color.White);

		for (int i = 0; i < player.lives.val; i++) {
			spriteBatch.Draw(Assets.lives, new Vector2(4 + 6 * i, 139), Color.White);
		}

		spriteBatch.End();


	}

	public void gameOver(GameTime gameTime) {
		if (gameOverScreen == null) {
			gameOverScreen = new HighScores(score.val);

		}
		else if (gameOverScreen.finished) {
			NodeManager.clearNodes();

			player = new Player(score, combo);
			NodeManager.AddNode(player);
			score.val = 0;
			combo.val = 0;
			gameOverScreen = null;
			LevelManager.startLevels(player);
		}
		else {
			gameOverScreen.Update(gameTime);
		}


	}
}
