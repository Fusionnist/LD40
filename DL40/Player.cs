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
        public bool isInvin;
        public float invinTime, invinTimer;

        public Player(TextureDrawer[] texes_, Vector2 pos_): base(texes_, pos_)
        {
            hp = 5;
            isInvin = true;
            invinTime = 3;
            invinTimer = invinTime;
        }

        public override void Move(Vector2? input = null, Vector2? extmov = null)
        {
            Yvel += 5;
            if (!isDead)
            {
                base.Move(input, extmov);
                Vector2 vinput = (Vector2)input;
                if (vinput.Y == -1 && onground)
                    Yvel = -250;
                else if (vinput.Y == 1 && Yvel < 0)
                    Yvel = 0;
            }
            mov.Y += Yvel;
        }

        public override void TakeDamage(int dmg_)
        {
            base.TakeDamage(dmg_);
            isInvin = true;
        }

        public override void Draw(SpriteBatch sb_)
        {
            if ((invinTimer * 10) % 2 > 1)
                base.Draw(sb_);
        }

        public override void Update(float es_)
        {
            base.Update(es_);
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
