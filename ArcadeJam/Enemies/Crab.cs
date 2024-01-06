using System;
using ArcadeJam.Enemies;
using ArcadeJam.Weapons;
using Engine.Core.Components;
using Engine.Core.Data;
using Engine.Core.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJam;

public enum CrabState {
    enter, attack1
}

public class CrabBoss :Node, IGrappleable {
    IntData health  = new(150);
    FloatRect bounds = new(0,0,75,48);
    Sprite sprite = new(Assets.crabBody);
    LeftClaw leftClaw;
    RightClaw rightClaw;
    CrabState state = CrabState.enter;
    CrabMovement movement;
    RectRender renderer;
    
    EnemyDamage damager;
    public CrabBoss() {
        leftClaw = new();
        rightClaw = new();
        renderer = new(sprite, bounds);
        movement = new(leftClaw, rightClaw,bounds, ref state);
        damager = new(bounds, null, health);
    }
    

    public  override void Update(GameTime gameTime) {
        damager.Update();
        movement.Update(gameTime);
        if (health.val <= 0) {
            Alive = false;
        }
        if (state==CrabState.enter){
            state=CrabState.attack1;
        }

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        renderer.Draw(gameTime, spriteBatch);
       

    }

	public void GrappleStun() {
		throw new NotImplementedException();
	}

	public void GrappleHit(int damage) {
		throw new NotImplementedException();
	}
}

public class LeftClaw : IGrappleable {
   
	public void GrappleHit(int damage) {
		
	}

	public void GrappleStun() {
		
	}
}
public class RightClaw {
    
}
public class CrabMovement {
    LeftClaw lClaw;
    RightClaw rClaw;
    FloatRect bounds;
    CrabState crabState;
    public CrabMovement(LeftClaw lClaw, RightClaw rClaw,FloatRect bounds, ref CrabState crabState) {
        this.lClaw = lClaw;
        this.rClaw = rClaw;
        this.bounds =bounds;

        this.crabState = crabState;

    }
	public  void Update(GameTime gameTime) {
        if (crabState==CrabState.enter){

        }
        Console.WriteLine(crabState);
	}


}

