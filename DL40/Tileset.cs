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
        bool[] door;
        bool[] ladder;
        int[] pool;
        bool[] slimeball;
        string[] actived;
        bool[] arrow;
        string[] facing;
        List<TextureDrawer>[] addTex;
        int columns;

        public Tileset(Point tileDims_,Texture2D src_,int columns_,int count_,bool[] solid_,bool[] hurtsmyass_, bool[] slips_, bool[] door_,
            int[] pool_,string[] actived_,bool[] slimeball_, List<TextureDrawer>[] addTex_, string[] facing_, bool[] arrow_,bool[] ladder_)
        {
            ladder = ladder_;
            facing = facing_;
            arrow = arrow_;
            addTex = addTex_;
            slimeball = slimeball_;
            actived = actived_;
            pool = pool_;
            door = door_;
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
            List<TextureDrawer> ts = new List<TextureDrawer>();
            ts.Add(td);
            foreach(TextureDrawer t in addTex[id])
            {
                ts.Add(t);
            }
            return new Tile(ts.ToArray(), pos_, solid[id], hurtsmyass[id],slips[id],door[id],pool[id],facing[id], arrow[id],ladder[id], actived[id]);
        }
        public Entity GetEntity(int id,Vector2 pos_)
        {
            TextureDrawer td = new TextureDrawer(src, new Rectangle((id % columns) * tileDims.X, (id / columns) * tileDims.Y, tileDims.X, tileDims.Y), Point.Zero, "imatile");
            List<TextureDrawer> ts = new List<TextureDrawer>();
            ts.Add(td);
            foreach (TextureDrawer t in addTex[id])
            {
                ts.Add(t);
            }
            if (slimeball[id])
                return new Bouncie(ts.ToArray(), pos_, 100);
            else
                return null;
        }
        public bool isEntity(int id)
        {
            return slimeball[id];
        }

    }
}
