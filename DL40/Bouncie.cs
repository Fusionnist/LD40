﻿using System;
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
        public Bouncie(TextureDrawer[] texes_, Vector2 pos_, float baseXvel): base(texes_, pos_, false)
        {
            Xvel = baseXvel;
            isHurty = true;
        }

        public override void Move(Vector2? input = default(Vector2?), Vector2? extmov = default(Vector2?))
        {
            if (onground)
                Yvel = -100;
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
    }
}
