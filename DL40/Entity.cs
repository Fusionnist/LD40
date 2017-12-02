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
    public class Entity
    {
        TextureDrawer[] texes;
        TextureDrawer currentTex;
        public Vector2 pos, prevPos, mov, prevMov;
        public bool isSolid;
        public float Yvel;

        public Entity(TextureDrawer[] texes_, Vector2 pos_, bool isSolid_ = false)
        {
            isSolid = isSolid_;
            pos = pos_;
            texes = texes_;
            currentTex = texes[0];
            Yvel = 0;
        }
        public void Move(Vector2? input = null, Vector2? extmov=null)
        {
            if(extmov != null)
            {
                mov += (Vector2)extmov;
            }
            Yvel += 1;
            mov.Y += Yvel;        
        }

        public virtual Rectangle GetHB()
        {
            return new Rectangle((int)pos.X - currentTex.c_center.X, (int)pos.Y - currentTex.c_center.Y, currentTex.c_sourceRect.Width, currentTex.c_sourceRect.Height);
        }

        public virtual Rectangle GetHBafterX()
        {
            return new Rectangle((int)(pos.X - currentTex.c_center.X + mov.X), (int)pos.Y - currentTex.c_center.Y, currentTex.c_sourceRect.Width, currentTex.c_sourceRect.Height);
        }

        public virtual Rectangle GetHBafterY()
        {
            return new Rectangle((int)pos.X - currentTex.c_center.X, (int)(pos.Y - currentTex.c_center.Y + mov.Y), currentTex.c_sourceRect.Width, currentTex.c_sourceRect.Height);
        }

        public virtual Rectangle GetHBAfterMov()
        {
            return new Rectangle((int)(pos.X - currentTex.c_center.X + mov.X), (int)(pos.Y - currentTex.c_center.Y + mov.Y), currentTex.c_sourceRect.Width, currentTex.c_sourceRect.Height);
        }

        public void SelectTex(string name_)
        {
            foreach(TextureDrawer td in texes)
            {
                if(td.name == name_)
                {
                    currentTex = td;
                }
            }
        }
        public void PreUpdate(float es_)
        {
            mov *= es_;
        }
        public void Update(float es_)
        {
            currentTex.Update(es_);
            pos += mov;
        }

        public void Draw(SpriteBatch sb_)
        {
            currentTex.Draw(sb_,pos);
        }
    }
}
