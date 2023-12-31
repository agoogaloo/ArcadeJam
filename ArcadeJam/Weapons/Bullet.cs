using System;
using System.Collections.Generic;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Engine.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam.Weapons;

public class BulletBasicMovement {

    private VelMovement movement;
    RectRender renderer;
    RectVisualizer boundsVisualizer;



    public BulletBasicMovement(Vector2Data vel, FloatRect bounds, Sprite texture) {
        movement = new(vel, bounds);
        renderer = new(texture, bounds);
        boundsVisualizer = new(bounds);

    }

    public void Update(GameTime gameTime) {


        movement.Update(gameTime);

    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(gameTime, spriteBatch);
        boundsVisualizer.Draw(spriteBatch);

    }

}

public class PlayerBullet : Node {
    private FloatRect bounds;

    BulletBasicMovement movement;
    Collision collision;
    List<Node> collisions = new();

    string[] collisionGroups = new string[]{"enemy"};
    

    public PlayerBullet(Vector2Data vel, Vector2 startPos) {
        bounds = new(startPos.X-3, startPos.Y-12, 6,12);
        Sprite sprite = new(Assets.icicle);
        movement = new(vel,bounds, sprite);
        collision = new(bounds, this, "playerBullet", collisions);
        

    }
    public override void Update(GameTime gameTime) {
        movement.Update(gameTime);
        collision.Update(gameTime, collisionGroups);
        if (collisions.Count>0 || bounds.Bottom<0){
            Alive = false;
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        movement.Draw(gameTime,spriteBatch);
    }
    public override void End(){
        collision.Update(null);
    }    
}

public class EnemyBullet : Node {
    private FloatRect bounds;

    BulletBasicMovement movement;
    Collision collision;
    List<Node> collisions = new();

    string[] collisionGroups = new string[]{"player"};
    

    public EnemyBullet(Vector2Data vel, Vector2 startPos) {
        bounds = new(startPos.X, startPos.Y, 8,8);
        Sprite sprite = new(Assets.enemyBullet);
        movement = new(vel,bounds, sprite);
        collision = new(bounds, this, "enemyBullet", collisions);
        

    }
    public override void Update(GameTime gameTime) {
        movement.Update(gameTime);
        collision.Update(gameTime, collisionGroups);
        if (collisions.Count>0 || bounds.Top>ArcadeGame.height){
            Alive = false;
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        movement.Draw(gameTime,spriteBatch);
    }
    public override void End(){
        collision.Update(null);
    }
}
