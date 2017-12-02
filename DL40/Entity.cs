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
        public bool isSolid, onground, isDead, slipping, isOnWall;
        public float Yvel, Xvel, speed;
        public int hp;

        public Entity(TextureDrawer[] texes_, Vector2 pos_, bool isSolid_ = false)
        {
            isSolid = isSolid_;
            pos = pos_;
            texes = texes_;
            currentTex = texes[0];
            Yvel = 0;
            Xvel = 0;
            onground = false;
            hp = 1;
            isDead = false;
            speed = 1;
            isOnWall = false;
        }
        public virtual void Move(Vector2? input = null, Vector2? extmov=null)
        {
            Vector2 vinput = (Vector2)input;
            if (!isDead)
            {
                if (vinput.X == -1)
                    mov.X -= speed;
                else if (vinput.X == 1)
                    mov.X += speed;
            }       
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
        public virtual void Update(float es_)
        {
            currentTex.Update(es_);
            pos += mov;
            if (hp <= 0 && !isDead)
            {
                isDead = true;
                isSolid = false;
            }
        }

        public virtual void Draw(SpriteBatch sb_)
        {
            currentTex.Draw(sb_,pos);
        }

        public virtual void TakeDamage(int dmg_)
        {
            hp -= dmg_;
        }
    }
}
