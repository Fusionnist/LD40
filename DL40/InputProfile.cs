using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace DL40
{
    public class InputProfile
    {
        KeyManager[] managers;
        public InputProfile(KeyManager[] managers_)
        {
            managers = managers_;
        }
        public void Update(KeyboardState kbs, GamePadState gps)
        {
            foreach(KeyManager km in managers) { km.Update(kbs, gps); }
        }
        public bool Pressed(string name)
        {
            foreach(KeyManager km in managers)
            {
                if (km.name == name) { return km.Pressed(); }
            }
            return false;
        }
        public bool JustPressed(string name)
        {
            foreach(KeyManager km in managers)
            {
                if(km.name == name) { return km.JustPressed(); }
            }
            return false;
        }
    }
}
