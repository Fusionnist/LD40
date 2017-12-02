using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DL40
{
    public class Player : Entity
    {
        public bool isInvin, canDJump, canWJump, releasedUp;
        public float invinTime, invinTimer;

        public Player(TextureDrawer[] texes_, Vector2 pos_): base(texes_, pos_)
        {
            hp = 5;
            isInvin = false;
            invinTime = 3;
            invinTimer = invinTime;
            canDJump = false;
            canWJump = false;
            releasedUp = false;
        }

        public override void Move(Vector2? input = null, Vector2? extmov = null)
        {
            Yvel += 5;
            Vector2 vinput = (Vector2)input;
            if (!isDead)
            {
                if (vinput.Y == -1)
                    releasedUp = false;
                else
                    releasedUp = true;
                base.Move(input, extmov);
                if (vinput.Y == -1 && onground )
                    Yvel = -250;
                else if (vinput.Y == 1 && Yvel < 0)
                    Yvel = 0;
            }
            mov.Y += Yvel;
            if (!slipping)
                Xvel = 0;
        }

        public override void TakeDamage(int dmg_)
        {
            if (!isInvin)
            {
                base.TakeDamage(dmg_);
                isInvin = true;
            }
        }

        public override void Draw(SpriteBatch sb_)
        {
            if ((invinTimer * 10) % 2 < 1)
                base.Draw(sb_);
        }

        public override void Update(float es_)
        {
            if (slipping)
            { Xvel += mov.X; mov.X = Xvel; }
            base.Update(es_);
            if (onground)
                canDJump = true;
            if (isInvin)
            {
                invinTimer -= es_;
                if (invinTimer <= 0)
                {
                    isInvin = false;
                    invinTimer = invinTime;
                }
            }
        }
    }
}
