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
    public class Tile : Entity
    {
        public bool isSlippery, isDoory, activated;
        public int activID;
        public string actived;

        public Tile(TextureDrawer[] texes_, Vector2 pos_, bool isSolid_, bool a_isHurty, bool isSlippy_, bool isDoory_, int activID_, string activated_ = null) : base(texes_, pos_)
        {
            isSolid = isSolid_;
            isHurty = a_isHurty;
            isSlippery = isSlippy_;
            isDoory = isDoory_;
            activID = activID_;
            actived = activated_;
            speed = 0;
            activated = false;
        }

        public virtual void Activate()
        {
            if (isDoory)
            {
                isSolid = !isSolid;
            }
        }
    }
}
