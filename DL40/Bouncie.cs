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
        public Bouncie(TextureDrawer[] texes_, Vector2 pos_, float baseXvel): base(texes_, pos_, false)
        {
            Xvel = baseXvel;
        }

        public override void Move(Vector2? input = default(Vector2?), Vector2? extmov = default(Vector2?))
        {
            if (onground)
                Yvel = -Yvel;
            else
                Yvel += 5;
            mov.X += Xvel;
            mov.Y += Yvel;
        }
    }
}
