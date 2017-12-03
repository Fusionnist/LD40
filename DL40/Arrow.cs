using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DL40
{
    public class Arrow : Entity
    {
        public Vector2 dir;

        public Arrow(TextureDrawer[] texes_, Vector2 pos_, Vector2 dir_): base(texes_, pos_)
        {
            dir = dir_;
            speed = 200;
        }

        public override void Move(Vector2? input = default(Vector2?), Vector2? extmov = default(Vector2?))
        {
            mov.X += speed * dir.X;
            mov.Y += speed * dir.Y;
        }

        public override void Update(float es_)
        {
            if (isOnWall)
            {
                isDead = true;
                dir = new Vector2(0, 1);
                speed = 100;
            }
                
            base.Update(es_);
        }
    }
}
