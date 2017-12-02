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
    class Player : Entity
    {
        public Player(TextureDrawer[] texes_, Vector2 pos_): base(texes_, pos_)
        {

        }

        public override void Move(Vector2? input = null, Vector2? extmov = null)
        {
            base.Move(input, extmov);
            Yvel += 1;
            mov.Y += Yvel;
            Vector2 vinput = (Vector2)input;
            if (vinput.Y == -1 && onground)
            {
                Yvel += 15;
            }
        }
    }
}
