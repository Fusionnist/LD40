using System;
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
        public bool isInvin, canDJump, releasedUp, releasedL, releasedR, dashRight, isDJumpDeactived, isDashDeactived, touchedGroundForDash, isWJumpDeactived, isLadderDeactived, isOnLadder, collidesWLadder;
        public float invinTime, invinTimer, dashInputTime, dashInputTimer, dashTime, dashTimer;
        public SoundManager sm;

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
                Yvel += 15f;
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
                    isOnLadder = false;
                if (vinput.Y == -1 && isOnLadder)
                    mov.Y -= 200;
                else if (vinput.Y == 1 && isOnLadder)
                    mov.Y += 200;
                else if (vinput.Y == -1 && onground)
                    Yvel = -350;
                else if (vinput.Y == 1 && Yvel < 0)
                    Yvel = 0;
                else if (vinput.Y == -1 && isOnWall && releasedUp && !isWJumpDeactived)
                    Yvel = -300;
                else if (vinput.Y == -1 && canDJump && releasedUp && !isDJumpDeactived)
                { Yvel = -300; canDJump = false; }
                if (vinput.Y == -1)
                    releasedUp = false;
                else
                    releasedUp = true;
                if (vinput.X == -1)
                {
                    if (releasedL && dashInputTimer > 0 && !dashRight && !isDashDeactived && touchedGroundForDash)
                    { dashTimer = dashTime; dashInputTimer = 0; touchedGroundForDash = false; }
                    releasedL = false;
                }
                else if (vinput.X == 1)
                {
                    if (releasedR && dashInputTimer > 0 && dashRight && !isDashDeactived && touchedGroundForDash)
                    { dashTimer = dashTime; dashInputTimer = 0; touchedGroundForDash = false; }
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
            if (!isInvin)
            {
                base.TakeDamage(dmg_);
                isInvin = true;
            }
        }

        public override Rectangle GetHB()
        {
            return new Rectangle((int)pos.X - 16, (int)pos.Y - 16, 32, 32);
        }

        public override Rectangle GetHBAfterMov()
        {
            return new Rectangle((int)(pos.X - 16 + mov.X), (int)(pos.Y - 16 + mov.Y), 32, 32);
        }

        public override Rectangle GetHBafterX()
        {
            return new Rectangle((int)(pos.X - 16 + mov.X), (int)pos.Y - 16, 32, 32);
        }

        public override Rectangle GetHBafterY()
        {
            return new Rectangle((int)pos.X - 16, (int)(pos.Y - 16 + mov.Y), 32, 32);
        }

        public override void Draw(SpriteBatch sb_)
        {
            if (!isDead)
            {
                if ((invinTimer * 10) % 2 < 1)
                    base.Draw(sb_);
            }
            else { base.Draw(sb_); }
        }

        public override void Update(float es_)
        {
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
        }

        protected override void SelectTexWow()
        {
            if (prevInput.X < 0)
            { facesLeft = true; }
            if (prevInput.X > 0)
            { facesLeft = false; }

            if (prevmov.Y < 0)
            { SelectTex("jump"); }
            else if (isOnWall)
            {
                SelectTex("wallclimb");
            }
            else
            { SelectTex("fall"); }

            if (onground)
            {
                SelectTex("ground");
                if (prevmov.X != 0) { SelectTex("walk"); }
            }

            if (isDead) { SelectTex("dead"); }
        }
    }
}
