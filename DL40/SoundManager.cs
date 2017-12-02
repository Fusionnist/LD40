using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace DL40
{
    public class SoundManager
    {
        List<SoundEffect> effects;
        List<Song> songs;
        List<string> effectNames;
        List<string> songNames;

        string currentSong;

        public SoundManager()
        {
            effectNames = new List<string>();
            effects = new List<SoundEffect>();

            songNames = new List<string>();
            songs = new List<Song>();
        }

        public void AddSong(Song song_, string name_)
        {
            songs.Add(song_);
            songNames.Add(name_);
        }

        public void AddEffect(SoundEffect effect_, string name_)
        {
            effects.Add(effect_);
            effectNames.Add(name_);
        }

        public void PlaySong(string name_)
        {
            if(currentSong == null || currentSong != name_)
            {
                for(int i = 0; i < songNames.Count; i++)
                {
                    if (songNames[i] == name_) { currentSong = name_; MediaPlayer.Play(songs[i]); }
                }
            }
        }

        public void StopSong()
        {
            currentSong = null;
            MediaPlayer.Stop();
        }

        public void PlayEffect(string name_)
        {
            for (int i = 0; i < effectNames.Count; i++)
            {
                if (effectNames[i] == name_)
                { effects[i].Play(); }
            }
        }
    }
}
