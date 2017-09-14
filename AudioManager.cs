using d4lilah.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.IO;

namespace d4lilah
{
    public class AudioManager
    {
        private Game1 _game;
        private int _id = 0;
        private int maxCount = 200;
        private AudioListener _listener;
        private List<SoundItem> _instances = new List<SoundItem>();

        public AudioManager(Game1 game)
        {
            _game = game;
            _listener = new AudioListener();
        }

        public void Update()
        {
            while(_instances.Count > 200)
            {
                _instances.RemoveAt(0);
            }
        }

        public int PlayAudio(string path, Vector2 position, string channel = "")
        {
            if(File.Exists(System.Environment.CurrentDirectory + @"/" + path + ".wav") || File.Exists(System.Environment.CurrentDirectory + @"/" + path + ".mp3"))
            {
                SoundEffect _sound = _game.Content.Load<SoundEffect>(path);
                SoundItem item = new SoundItem();
                item.ID = _id++;
                item.Position = new ShellVector(position);
                AudioEmitter emitter = new AudioEmitter();
                emitter.Position = new Vector3(position.X, position.Y, 0f);
                item.Instance.Volume = 1;
                if(channel.ToLower() == "music")
                {
                    item.Instance.Volume = _game.ClientSettings.MusicVolume;
                }
                if(channel.ToLower() == "ambience")
                {
                    item.Instance.Volume = _game.ClientSettings.AmbienceVolume;
                }
                item.Instance.Volume *= _game.ClientSettings.MasterVolume;
                item.Instance = _sound.CreateInstance();
                item.Instance.Apply3D(_listener, emitter);
                item.Instance.Play();
                return item.ID;
                
            }
            else
            {
                _game.Log.Write(" Could not find '" + System.Environment.CurrentDirectory + @"/" + path + ".wav' or '" + File.Exists(System.Environment.CurrentDirectory + @"/" + path + ".mp3"));
            }
            return -1;
        }

        public int StopAudio(int id)
        {
            for(int i = 0; i < _instances.Count; i++)
            {
                if(_instances[i].ID == id)
                {
                    _instances[i].Instance.Stop();
                    return 1;
                }
            }
            return -1;
        }

        public int AudioPosition(int id, Vector2 position)
        {
            for(int i = 0; i < _instances.Count; i++)
            {
                if(_instances[i].ID == id)
                {
                    AudioEmitter emitter = new AudioEmitter();
                    emitter.Position = new Vector3(position.X, position.Y, 0f);
                    _instances[i].Instance.Apply3D(_listener, emitter);
                    return 1;
                }
            }
            return -1;
        }
    }
}
