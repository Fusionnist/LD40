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
        Vector2 pos, prevPos, mov, prevMov;
        public bool isSolid;

        public Entity(TextureDrawer[] texes_, Vector2 pos_, bool isSolid_ = false)
        {
            isSolid = isSolid_;
            pos = pos_;
            texes = texes_;
            currentTex = texes[0];
        }
        public void Move(Vector2? input = null, Vector2? extmov=null)
        {
            if(extmov != null)
            {
                mov += (Vector2)extmov;
            }        
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
        }

        public void Draw(SpriteBatch sb_)
        {
            currentTex.Draw(sb_,pos);
        }
    }
}
