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
        public Point vpos;
        public List<Tile> tiles;
        public List<Entity> bouncies;
        public Tilemap(List<Tile> tiles_, List<Entity> bouncies_, Point dims_,Point vpos_)
        {
            bouncies = bouncies_;
            vpos = vpos_;
            dims = dims_;
            tiles = tiles_;

            foreach(Tile t in tiles)
            {
                t.pos.X += vpos.X * 640;
                t.pos.Y += vpos.Y * 320;
            }
            foreach (Entity t in bouncies)
            {
                t.pos.X += vpos.X * 640;
                t.pos.Y += vpos.Y * 320;
            }
        }

        public void Draw(SpriteBatch sb_)
        {
            foreach(Tile t in tiles)
            {
                t.Draw(sb_);
            }
            foreach (Entity t in bouncies)
            {
                t.Draw(sb_);
            }
        }
        public void PreUpdate(float es_)
        {

            foreach (Tile t in tiles)
            {
                t.PreUpdate(es_);
            }
            foreach (Entity t in bouncies)
            {
                t.PreUpdate(es_);
            }
        }
        public void Update(float es_)
        {
            foreach (Tile t in tiles)
            {
                t.Update(es_);
            }
            foreach (Entity t in bouncies)
            {
                t.Update(es_);
            }
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(640 * vpos.X, 320 * vpos.Y, 32*dims.X, 32*dims.Y);
        }
    }
}
