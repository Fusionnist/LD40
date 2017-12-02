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
    public class Font
    {
        TextureDrawer[] characters;
        public string name;
        public Font(TextureDrawer[] characters_,string name_)
        {
            characters = characters_;
            name = name_;
        }

        public TextureDrawer GetCharacter(char name_)
        {
            foreach(TextureDrawer td in characters)
            {
                if (td.name == name_.ToString()) { return td; }
            }
            return characters[0];
        }

        public void Update(float es_)
        {
            foreach (TextureDrawer td in characters)
            {
                td.Update(es_);
            }
        }
    }
}
