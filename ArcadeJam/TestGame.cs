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
		// TODO: Add your initialization logic here

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


		Entity player = new();
		Vector2Comp position = new(new Vector2(150, 50));
		Vector2Comp vel = new(new Vector2(0,0));
		Sprite spr = new(pengImg);
		ScreenRender renderer = new(spr, position);
		VelMovement mover = new(vel, position);
		Gravity gravity = new(vel);
		Jump jump = new(vel,InputHandler.getButton("test1"));

		player.addData(position);
		player.addData(spr);
		player.addFunc(renderer);
		player.addFunc(mover);
		player.addFunc(gravity);
		player.addFunc(jump);
		player.addFunc(new InputTest());
		

	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);

		pengImg = this.Content.Load<Texture2D>("peng");
	}

	protected override void Update(GameTime gameTime) {

		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		base.Update(gameTime);
		SystemManager.Instance.PhysicsSystem.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.CornflowerBlue);
		// TODO: Add your drawing code here

		spriteBatch.Begin();
		SystemManager.Instance.RenderSystem.Draw(gameTime, spriteBatch);
		spriteBatch.End();

		base.Draw(gameTime);
	}
}
