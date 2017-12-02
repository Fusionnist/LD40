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
    public class Tilemap
    {
        Point dims;
        public List<Tile> tiles;

        public Tilemap(List<Tile> tiles_, Point dims_)
        {
            dims = dims_;
            tiles = tiles_;
        }

        public void Draw(SpriteBatch sb_)
        {
            foreach(Tile t in tiles)
            {
                t.Draw(sb_);
            }
        }

        public void Update(float es_)
        {
            foreach (Tile t in tiles)
            {
                t.Update(es_);
            }
        }
    }
}
