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
    public class FontDrawer
    {
        public List<Font> fonts;

        public FontDrawer()
        {
            fonts = new List<Font>();
        }

        public void DrawText(string fontname, string text, Rectangle bounds,SpriteBatch sb_)
        {
            foreach(Font f in fonts)
            {
                if (f.name == fontname)
                {
                    Vector2 pos = new Vector2(0,0);
                    for(int x = 0; x < text.Length; x++)
                    {                        
                        List<TextureDrawer> words = new List<TextureDrawer>();
                        int width = 0;
                        for(int i = x; i < text.Length && text[i]!= ' '; i++)
                        {
                            words.Add(f.GetCharacter(text[i]));
                            width += f.GetCharacter(text[i]).c_sourceRect.Width;
                            x = i;   
                        }
                        
                        if (pos.X + width > bounds.Width)
                        {
                            pos.X = 0;
                            pos.Y += f.GetCharacter(' ').c_sourceRect.Height;
                        }
                        foreach (TextureDrawer td in words)
                        {
                            td.Draw(sb_, pos+new Vector2(bounds.X, bounds.Y));
                            pos.X += td.c_sourceRect.Width;
                        }

                        pos.X += f.GetCharacter(' ').c_sourceRect.Width;
                    }
                    //line skip ect ect, create an array of words, test their width and all...
                }
            }
        }

        public void Update(float es_)
        {
            foreach (Font f in fonts)
            {
                f.Update(es_);
            }
        }
    }
}
