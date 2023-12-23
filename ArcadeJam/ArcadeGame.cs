using System;
using Engine.Core;
using Engine.Core.Components;
using Engine.Core.Input;
using Engine.Core.Physics;
using Engine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Player;


namespace ArcadeJam;

public class ArcadeGame : Game {


	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;
	private RenderTarget2D renderTarget;


	const int width = 320, height = 180;

	public ArcadeGame() {
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
		Window.AllowUserResizing = true;

	}

	protected override void Initialize() {

		base.Initialize();

		//setting all the input bindings
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

		PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
		renderTarget = new(graphics.GraphicsDevice, width, height, false,
				GraphicsDevice.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24);


		NodeManager.AddNode(new Player());

	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);
		Assets.Load(Content);
	}

	protected override void Update(GameTime gameTime) {
		base.Update(gameTime);
		InputHandler.Update();
		NodeManager.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {

		//drawing the game to render target at game resolution
		graphics.GraphicsDevice.SetRenderTarget(renderTarget);
		GraphicsDevice.Clear(Color.CornflowerBlue);
		spriteBatch.Begin();
		NodeManager.Draw(gameTime, spriteBatch);
		spriteBatch.End();

	
		//drawing render target to the screen
		graphics.GraphicsDevice.SetRenderTarget(null);
		int screenWidth = GraphicsDevice.Viewport.Width;
		int screenHeight = GraphicsDevice.Viewport.Height;
		
		spriteBatch.Begin(samplerState: SamplerState.PointClamp);
		GraphicsDevice.Clear(Color.Black);

		spriteBatch.Draw(renderTarget, new Rectangle(0, 0,
		screenWidth, screenHeight), Color.White);
		spriteBatch.End();


		base.Draw(gameTime);
	}
}
