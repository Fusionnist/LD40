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
        public string facing;
        public bool isSlippery, isDoory, activated, isLaddery, isFlameTrappy, isHealthPickup;
        public int activID;
        public string actived;
        public bool arrow;
        public Tile(TextureDrawer[] texes_, Vector2 pos_, bool isSolid_, bool a_isHurty, bool isSlippy_, bool isDoory_, int activID_, string facing_, bool arrow_, bool isLaddery_, bool isFlameTrappy_, bool isHealthPickup_, string activated_ = null) : base(texes_, pos_)
        {
            arrow = arrow_;
            facing = facing_;
            isSolid = isSolid_;
            isHurty = a_isHurty;
            isSlippery = isSlippy_;
            isDoory = isDoory_;
            activID = activID_;
            actived = activated_;
            speed = 0;
            activated = false;
            isLaddery = isLaddery_;
            isFlameTrappy = isFlameTrappy_;
            isHealthPickup = isHealthPickup_;
        }

        public virtual void Activate()
        {
            if (isDoory)
            {
                isSolid = !isSolid;
                activated = true;
            }
        }
        protected override void SelectTexWow()
        {
            { SelectTex("idle"); }
            base.SelectTexWow();
            if (activated)
            { SelectTex("openTrapdoor"); }
        }
        public override void Draw(SpriteBatch sb_)
        {          
            if (actived != null)
            {
                if (!isDead) { base.Draw(sb_); }
            }
            else { base.Draw(sb_); }
        }
    }
}
