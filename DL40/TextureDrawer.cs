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
    public class TextureDrawer
    {
        Texture2D src;
        public Rectangle c_sourceRect;
        public Point c_center;
        public string name;
        //anim values
        Rectangle[] sourceRects;
        Point[] centers;
        float frameTime, frameTimer; int frameCount, frameCounter;
        bool loops, anim;

        public TextureDrawer(Texture2D src_, string name_=null)
        {
            name = name_;
            src = src_;
            c_sourceRect = new Rectangle(0, 0, src_.Width, src_.Height);
            c_center = Point.Zero;
        }

        public TextureDrawer(Texture2D src_, Rectangle sourceRect_, Point center_, string name_) //no anim
        {
            name = name_;
            src = src_;
            c_sourceRect = sourceRect_;
            c_center = center_;
        }

        public TextureDrawer(Texture2D src_, Rectangle[] sourceRects_, Point[] centers_, float frameTime_, int frameCount_,bool loops_, string name_) //yes anim
        {
            name = name_;
            anim = true;
            loops = loops_;
            src = src_;
            sourceRects = sourceRects_;
            centers = centers_;
            c_sourceRect = sourceRects[0];
            c_center = centers[0];
            frameTime = frameTime_;
            frameTimer = frameTime_;
            frameCount = frameCount_;
            frameCounter = 0;
        }

        public void Update(float es_)
        {
            if (anim)
            {
                frameTimer -= es_;
                if (frameTimer < 0)
                {
                    frameTimer = frameTime;
                    frameCounter++;
                    if (frameCounter >= frameCount)
                    {
                        if (loops) { frameCounter = 0; }
                        else { frameCounter = frameCount - 1; }
                    }
                }
                c_center = centers[frameCounter];
                c_sourceRect = sourceRects[frameCounter];
            }            
        }
        public TextureDrawer clone()
        {
            if (anim) { return new TextureDrawer(src, sourceRects, centers, frameTime, frameCount, loops, name); }
            else { return new TextureDrawer(src, c_sourceRect, c_center, name); }
        }
        public void Reset()
        {
            frameCounter = 0;
        }
        public void Draw(SpriteBatch sb_, Vector2 pos_, bool flip = false)
        {
            SpriteEffects se = SpriteEffects.None;
            if (flip) { se = SpriteEffects.FlipHorizontally; }
            sb_.Draw(src,position: pos_.ToPoint().ToVector2() - c_center.ToVector2(),sourceRectangle: c_sourceRect, effects: se);
        }

        public bool Ended()
        {
            if (loops)
            {
                return frameCounter == frameCount - 1;
            }
            return false;
        }
    }
}
