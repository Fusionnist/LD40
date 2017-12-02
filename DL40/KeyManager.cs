using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace DL40
{
    public class KeyManager
    {
        public string name;
        bool pressed, justPressed;
        bool isButton;
        Keys key;
        Buttons button;

        public KeyManager(Keys key_, string name_) //key
        {
            key = key_;
            name = name_;
        }

        public KeyManager(Buttons button_, string name_) //button
        {
            isButton = true;
            button = button_;
            name = name_;
        }

        public void Update(KeyboardState kbs, GamePadState gps)
        {
            if (isButton)
            {
                if (gps.IsButtonDown(button)) { if (!pressed) { justPressed = true; } else { justPressed = false; } pressed = true; }
                else { pressed = false;justPressed = false; }
            }
            else
            {
                if (kbs.IsKeyDown(key)) { if (!pressed) { justPressed = true; } else { justPressed = false; } pressed = true; }
                else { pressed = false; justPressed = false; }
            }
        }

        public bool Pressed()
        {
            return pressed;
        }

        public bool JustPressed()
        {
            return justPressed;
        }
    }
}
