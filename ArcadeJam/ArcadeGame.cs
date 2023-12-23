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

	private Texture2D pengImg;


	public ArcadeGame() {
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
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

		SystemManager.Start();

		_ = new Player();	

	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);
		Assets.Load(Content);
	}

	protected override void Update(GameTime gameTime) {
		base.Update(gameTime);
		SystemManager.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);
		
		spriteBatch.Begin();
		SystemManager.Draw(gameTime, spriteBatch);
		spriteBatch.End();

		base.Draw(gameTime);
	}
}
