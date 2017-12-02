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
    public class Tile : Entity
    {
        public bool isHurty, isSlippery, isDoory;
        public int activID;

        public Tile(TextureDrawer[] texes_, Vector2 pos_, bool isSolid_, bool a_isHurty, bool isSlippy_, bool isDoory_, int activID_): base(texes_, pos_)
        {
            isSolid = isSolid_;
            isHurty = a_isHurty;
            isSlippery = isSlippy_;
            isDoory = isDoory_;
            activID = activID_;
        }

        public virtual void Activate()
        {

        }
    }
}
