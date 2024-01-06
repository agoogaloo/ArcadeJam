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
public class CrabstateData{
    public CrabState val = CrabState.enter;
}

public class CrabBoss : Node, IGrappleable {
    IntData health = new(150);
    FloatRect bounds = new(0, 0, 75, 48);
    Sprite sprite = new(Assets.crabBody);
    LeftClaw leftClaw;
    RightClaw rightClaw;
    CrabstateData state = new();
    CrabMovement movement;
    RectRender renderer;

    EnemyDamage damager;
    EnemyWeapon currentPattern;
    public CrabBoss() {
        leftClaw = new();
        rightClaw = new();
        renderer = new(sprite, bounds);
        movement = new(leftClaw, rightClaw, bounds,  state);
        damager = new(bounds, null, health);
        currentPattern = new Spiral(bounds, 0.5f, 20, 180*2/20, 50);
    }


    public override void Update(GameTime gameTime) {
        damager.Update();
        movement.Update(gameTime);
        if (health.val <= 0) {
            Alive = false;
        }
        switch (state.val) {
            case CrabState.enter:
                if (bounds.Centre.X >= 75) {
                    state.val = CrabState.attack1;
                }
                break;
            case CrabState.attack1:
                currentPattern.Update(gameTime);
                break;


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
    public override void End() {
        base.End();
        damager.End();

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
    CrabstateData crabState;
    public CrabMovement(LeftClaw lClaw, RightClaw rClaw, FloatRect bounds, CrabstateData crabState) {
        this.lClaw = lClaw;
        this.rClaw = rClaw;
        this.bounds = bounds;
        bounds.x = -90;
        bounds.y = 5;
        this.crabState = crabState;

    }
    public void Update(GameTime gameTime) {
        if (crabState.val == CrabState.enter) {
            bounds.x += (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
        }
        Console.WriteLine(crabState);
    }


}

