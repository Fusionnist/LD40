using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DL40
{
    class Lifebar
    {
        TextureDrawer tex;

        public Lifebar(TextureDrawer tex_)
        {
            tex = tex_;
        }

        public void Draw(SpriteBatch sb, int hp)
        {
            tex.c_sourceRect = new Rectangle(0, 0, 16 * hp, 16);
            tex.Draw(sb, Vector2.Zero);
        }
    }
}
