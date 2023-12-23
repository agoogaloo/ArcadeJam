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
		
		InputHandler.addButtonBind("test1", Keys.Z);
		InputHandler.addButtonBind("test1", Keys.X);
		InputHandler.addButtonBind("test2", Keys.Space);
		InputHandler.addButtonBind("test1", GPadInput.A);
		InputHandler.addAnalogBind("test3", Keys.Left);
		InputHandler.addAnalogBind("test3", Keys.Right);
		InputHandler.addAnalogBind("test3", GPadInput.LStickUp);
		InputHandler.addAnalogBind("test3", GPadInput.LStickLeft);
		InputHandler.addAnalogBind("test4", Keys.Down);

		SystemManager.Start();	

		new Player();	

	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);

		pengImg = this.Content.Load<Texture2D>("peng");
	}

	protected override void Update(GameTime gameTime) {

		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		base.Update(gameTime);
		SystemManager.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);
		// TODO: Add your drawing code here

		spriteBatch.Begin();
		SystemManager.Draw(gameTime, spriteBatch);
		spriteBatch.End();

		base.Draw(gameTime);
	}
}
