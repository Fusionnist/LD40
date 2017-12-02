using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace DL40
{
    public class Tileset
    {
        Point tileDims;
        Texture2D src;
        int count;
        bool[] solid;
        bool[] hurtsmyass;
        bool[] slips;
        int columns;

        public Tileset(Point tileDims_,Texture2D src_,int columns_,int count_,bool[] solid_,bool[] hurtsmyass_, bool[] slips_)
        {
            slips = slips_;
            hurtsmyass = hurtsmyass_;
            count = count_;
            tileDims = tileDims_;
            src = src_;
            columns = columns_;
            solid = solid_;
        }

        public Tile getTile(int id,Vector2 pos_)
        {
            TextureDrawer td = new TextureDrawer(src,new Rectangle((id%columns)*tileDims.X,(id/columns)*tileDims.Y,tileDims.X,tileDims.Y),Point.Zero,"imatile");
            return new Tile(new TextureDrawer[] { td }, pos_, solid[id], hurtsmyass[id],slips[id]);
        }
    }
}
