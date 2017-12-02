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
    class Tile : Entity
    {
        public Tile(TextureDrawer[] texes_, Vector2 pos_): base(texes_, pos_)
        {
            isSolid = true;
        }
    }
}
