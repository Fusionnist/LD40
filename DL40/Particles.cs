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
    public class Particles
    {
        
        List<float> times;
        TextureDrawer particle;
        public List<Vector2> pos, mov;

        public Particles(TextureDrawer s_)
        {
            times = new List<float>();
            pos = new List<Vector2>();
            mov = new List<Vector2>();
            particle = s_;
        }

        public void AddPar(Vector2 pos_, Vector2 mov_)
        {
            pos.Add(pos_);
            mov.Add(mov_);
            times.Add(1.5f);
        }

        public void Update(float es_)
        {
            for(int i = times.Count-1; i >= 0; i--)
            {
                times[i] -= es_;
                pos[i] += mov[i] * es_;
                if(times[i] < 0) { times.RemoveAt(i); pos.RemoveAt(i); mov.RemoveAt(i); }
            }
        }

        public void Draw(SpriteBatch sb_)
        {
            foreach(Vector2 p in pos)
            {
                particle.Draw(sb_, p);
            }
        }
    }
}
