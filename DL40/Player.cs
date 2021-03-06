﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
//F
namespace DL40
{
    public class Player : Entity
    {
        public int wealth;
        public bool isInvin, canDJump, releasedUp, releasedL, releasedR, dashRight, isDJumpDeactived, isDashDeactived, touchedGroundForDash, isWJumpDeactived, isLadderDeactived, isOnLadder, collidesWLadder;
        public float invinTime, invinTimer, dashInputTime, dashInputTimer, dashTime, dashTimer;
        float dashttimer;
        public Player(TextureDrawer[] texes_, Vector2 pos_, SoundManager seffects): base(texes_, pos_)
        {
            sm = seffects;
            hp = 5;
            isInvin = false;
            invinTime = 3;
            invinTimer = invinTime;
            canDJump = false;
            isOnWall = false;
            releasedUp = false;
            releasedL = false;
            releasedR = false;
            dashRight = true;
            isDJumpDeactived = false;
            isDashDeactived = false;
            isWJumpDeactived = false;
            touchedGroundForDash = true;
            isOnLadder = false;
            collidesWLadder = false;
            isLadderDeactived = false;
            dashInputTime = 0.15f;
            dashInputTimer = 0;
            dashTime = 0.12f;
            dashTimer = 0;
            speed = 150;
        }

        public override void Move(Vector2? input = null, Vector2? extmov = null)
        {
            Vector2 vinput = (Vector2)input;
            prevInput = vinput;
            if (!isOnLadder)
            {
                if(!isOnWall || Yvel < 0)
                    Yvel += 15f;
                else { Yvel += 5f; }
            }

            if (!isDead)
            {
                if (dashTimer <= 0)
                    base.Move(input, extmov);
                else
                {
                    if (dashRight)
                        mov.X = 750;
                    else
                        mov.X = -750;
                }
                if (vinput.Y == -1 && collidesWLadder && !isLadderDeactived)
                { isOnLadder = true; Yvel = 0; }
                if (!collidesWLadder || isLadderDeactived)
                {
                    if (isOnLadder)
                        Yvel = -300;
                    isOnLadder = false;
                }
                if (vinput.Y == -1 && isOnLadder)
                    mov.Y -= 200;
                else if (vinput.Y == 1 && isOnLadder)
                    mov.Y += 200;
                else if (vinput.Y == -1 && onground)
                {
                    if (dashTimer <= 0 && Yvel >= 0) { sm.PlayEffect("jump"); }
                    Yvel = -350;
                }
                else if (vinput.Y == 1 && Yvel < 0)
                    Yvel = 0;
                else if (vinput.Y == -1 && isOnWall && releasedUp && !isWJumpDeactived)
                {
                    if (dashTimer <= 0 && Yvel >= 0) { sm.PlayEffect("jump"); }
                    Yvel = -300;
                }
                else if (vinput.Y == -1 && canDJump && releasedUp && !isDJumpDeactived)
                { if (dashTimer <= 0 && Yvel >= 0) { sm.PlayEffect("jump"); } 
                Yvel = -300; canDJump = false; }
                if (vinput.Y == -1)
                    releasedUp = false;
                else
                    releasedUp = true;
                if (vinput.X == -1)
                {
                    if (releasedL && dashInputTimer > 0 && !dashRight && !isDashDeactived && touchedGroundForDash)
                    { dashTimer = dashTime; dashInputTimer = 0; touchedGroundForDash = false; dashttimer = 0.2f; sm.PlayEffect("dash"); }
                    releasedL = false;
                }
                else if (vinput.X == 1)
                {
                    if (releasedR && dashInputTimer > 0 && dashRight && !isDashDeactived && touchedGroundForDash)
                    { dashTimer = dashTime; dashInputTimer = 0; touchedGroundForDash = false; dashttimer = 0.2f; sm.PlayEffect("dash"); }
                    releasedR = false;
                }
                else
                { releasedL = true; releasedR = true; }
            }
            if (dashTimer > 0)
                Yvel = 0;
            mov.Y += Yvel;
            if (!slipping)
                Xvel = 0;
            else if (Xvel > 4 && dashTimer <= 0)
                Xvel = 4;
            else if (Xvel < -4 && dashTimer <= 0)
                Xvel = -4;
        }

        public override void TakeDamage(int dmg_)
        {
            if (!isInvin && !isDead)
            {
                sm.PlayEffect("hurty");
                base.TakeDamage(dmg_);
                isInvin = true;
            }
        }

        public override Rectangle GetHB()
        {
            return new Rectangle((int)pos.X - 10, (int)pos.Y - 4, 20, 20);
        }

        public override Rectangle GetHBAfterMov()
        {
            return new Rectangle((int)(pos.X - 10 + mov.X), (int)(pos.Y - 4 + mov.Y), 20, 20);
        }

        public override Rectangle GetHBafterX()
        {
            return new Rectangle((int)(pos.X - 10 + mov.X), (int)pos.Y - 4, 20, 20);
        }

        public override Rectangle GetHBafterY()
        {
            return new Rectangle((int)pos.X - 10, (int)(pos.Y - 4 + mov.Y), 20, 20);
        }

        public override void Draw(SpriteBatch sb_)
        {
            if (!isDead)
            {            
                if ((invinTimer * 10) % 2 < 1)
                {
                    if(wealth == 100)
                        getTex("bag1").Draw(sb_, pos, facesLeft);
                    if (wealth == 200)
                        getTex("bag2").Draw(sb_, pos, facesLeft);
                    if (wealth == 300)
                        getTex("bag3").Draw(sb_, pos, facesLeft);
                    if (wealth >= 400)
                        getTex("bag4").Draw(sb_, pos, facesLeft);
                    base.Draw(sb_);
                }
                    
            }
            else { base.Draw(sb_); }
        }

        public override void Update(float es_)
        {
            prevMov = mov;
            if (slipping)
            {
                Xvel += mov.X / 15 ;
                if (dashTimer <= 0)
                    mov.X = Xvel;
            }
            base.Update(es_);
            if (onground)
            { canDJump = true; touchedGroundForDash = true; }
            if (isInvin)
            {
                invinTimer -= es_;
                if (invinTimer <= 0)
                {
                    isInvin = false;
                    invinTimer = invinTime;
                }
            }
            if (dashTimer <= 0)
            {
                if (releasedL && releasedR)
                    dashInputTimer -= es_;
                else if (releasedL)
                { dashRight = true; dashInputTimer = dashInputTime; }
                else if (releasedR)
                { dashRight = false; dashInputTimer = dashInputTime; }
            }
            dashTimer -= es_;
            dashttimer -= es_;
        }

        protected override void SelectTexWow()
        {
            if (prevInput.X < 0)
            { facesLeft = true; }
            if (prevInput.X > 0)
            { facesLeft = false; }

            if (prevmov.Y < 0)
            { SelectTex("jump"); }
            else
            { SelectTex("fall"); }
            if (isOnWall)
            {
                SelectTex("wallclimb");
            }
            

            if (onground)
            {
                SelectTex("ground");
                if (prevmov.X != 0) { SelectTex("walk"); }
            }
            if (slipping) { SelectTex("slip"); }
            if (isOnLadder)
            {
                SelectTex("ladder");
                if(prevMov == Vector2.Zero) { SelectTex("ladderstill"); }
            }
            
            if (dashttimer > 0) { SelectTex("dash"); }
            if (isDead) { SelectTex("dead"); }
        }
    }
}
