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
    public class Bouncie : Entity
    {
        bool left;
        public Bouncie(TextureDrawer[] texes_, Vector2 pos_, float baseXvel, SoundManager sm_): base(texes_, pos_,sm_, false)
        {
            Xvel = baseXvel;
            isHurty = true;
        }

        public override void Move(Vector2? input = default(Vector2?), Vector2? extmov = default(Vector2?))
        {
            if (onground)
            {
                Yvel = -Yvel;
                sm.PlayEffect("jump2");
            }
            else
                Yvel += 4;

            if (isOnWall)
                left = !left;

            if(left)
                mov.X -= Xvel;
            else
                mov.X += Xvel;
            mov.Y += Yvel;
        }

        public override void Update(float es_)
        {
            base.Update(es_);
        }

        public override void Draw(SpriteBatch sb_)
        {
            base.Draw(sb_);
        }

        protected override void SelectTexWow()
        {
            base.SelectTexWow();
            facesLeft = false;
        }
    }
}
