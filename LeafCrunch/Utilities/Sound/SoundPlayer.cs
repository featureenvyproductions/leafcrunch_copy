using System;
using System.Windows.Media;

namespace LeafCrunch.Utilities.Sound
{
    //for controlling background music independent of rooms
    //some music will need to keep playing between rooms like in the intro part
    //also I gotta figure out how to loop things I don't think this loops
    //oh man I gotta consolidate these....the thing is though
    //we can have layered sound effects but we only want one thing playing as the continuous bg music so
    public class SoundController
    {
        private System.Uri _soundURI;

        private TimeSpan _position = new TimeSpan(0);

        public System.Uri SoundURI
        {
            get { return _soundURI; }
            set { _soundURI = value; }
        }

        private MediaPlayer _mediaPlayer = null;
        public MediaPlayer MediaPlayer
        {
            get
            {
                if (_mediaPlayer == null)
                    _mediaPlayer = new MediaPlayer();
                return _mediaPlayer;
            }
        }

        public void Play(bool stopFirst)
        {
            if (stopFirst)
                MediaPlayer.Stop();

            if (SoundURI != null)
            {
                MediaPlayer.Open(SoundURI);
                MediaPlayer.Play();
            }
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }

        public void Pause()
        {
            if (MediaPlayer.CanPause)
            {
                _position = MediaPlayer.Position;
                MediaPlayer.Pause();
            }
        }

        public void Resume()
        {
            MediaPlayer.Position = _position;
            MediaPlayer.Play();
        }
    }
}
