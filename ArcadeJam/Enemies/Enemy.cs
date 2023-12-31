using System;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public class Enemy : Node {
	private Sprite sprite = new(Assets.enemy);
    private Vector2Data vel = new(20f,0);
    private FloatRect bounds = new(0,0,10,10);

    private RectRender renderer;
    private VelMovement velMovement;
    Straight pattern;
    Collision collision;

    public Enemy(){
        renderer= new(sprite, bounds);
        velMovement = new(vel, bounds);
        pattern = new(bounds);
        collision = new(bounds, this, "enemy");
    }


	public override void Update(GameTime gameTime) {
        velMovement.Update(gameTime);
        pattern.Update(gameTime);
		
	}
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(gameTime,spriteBatch);
		
	}

	public override void End(){
        collision.Update(null);
    }
}
