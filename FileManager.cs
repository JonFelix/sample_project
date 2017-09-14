using System;
using System.IO;
using d4lilah.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace d4lilah
{
    public class FileManager
    {
        private Game1 _game;

        private List<ScriptInfo> _scripts = new List<ScriptInfo>();
        private List<string> _sprites = new List<string>();
        private List<string> _fonts = new List<string>();
        private GameSettings _clientSettings;
        private DateTime _nextUpdate;
        private string _activeMod = "";
        private List<string> _folders = new List<string>();
        private List<string> _svgs = new List<string>();

        public string[] SVGLocations
        {
            get
            {
                return _svgs.ToArray();
            }
        }

        public string[] FontLocations
        {
            get
            {
                return _fonts.ToArray();
            }
        }

        public string[] SpriteLocations
        {
            get
            {
                return _sprites.ToArray();
            }
        }

        public string[] Folders
        {
            get
            {
                return _folders.ToArray();
            }
        }

        public ScriptInfo[] AllScripts
        {
            get
            {
                return _scripts.ToArray();
            }
        }

        public ScriptInfo[] NonDefinedScripts
        {
            get
            {
                List<ScriptInfo> entities = new List<ScriptInfo>();
                for(int i = 0; i < _scripts.Count; i++)
                {
                    if(_scripts[i].ScriptType == Constants.SCRIPT_TYPE_MISC)
                    {
                        entities.Add(_scripts[i]);
                    }
                }
                return entities.ToArray();
            }
        }

        public string[] GetAllMD5()
        {
            List<string> checksums = new List<string>();
            for(int i = 0; i < _scripts.Count; i++)
            {
                checksums.Add(_scripts[i].MD5);
            }
            SpriteInfo[] sprites = _game.Sprites.GetAllSprites;
            for(int i = 0; i < sprites.Length; i++)
            {
                checksums.Add(sprites[i].MD5);
            }
            FontInfo[] fonts = _game.Sprites.GetAllFonts;
            for(int i = 0; i < fonts.Length; i++)
            {
                checksums.Add(fonts[i].MD5);
            }
            return checksums.ToArray();
        }

        public ScriptInfo[] EntityScripts
        {
            get
            {
                List<ScriptInfo> entities = new List<ScriptInfo>();
                for(int i = 0; i < _scripts.Count; i++)
                {
                    if(_scripts[i].ScriptType == Constants.SCRIPT_TYPE_ENTITY)
                    {
                        entities.Add(_scripts[i]);
                    }
                }
                return entities.ToArray();
            }
        }

        public ScriptInfo[] LevelScripts
        {
            get
            {
                List<ScriptInfo> entities = new List<ScriptInfo>();
                for(int i = 0; i < _scripts.Count; i++)
                {
                    if(_scripts[i].ScriptType == Constants.SCRIPT_TYPE_LEVEL)
                    {
                        entities.Add(_scripts[i]);
                    }
                }
                return entities.ToArray();
            }
        }

        public ScriptInfo[] WidgetScripts
        {
            get
            {
                List<ScriptInfo> entities = new List<ScriptInfo>();
                for(int i = 0; i < _scripts.Count; i++)
                {
                    if(_scripts[i].ScriptType == Constants.SCRIPT_TYPE_WIDGET)
                    {
                        entities.Add(_scripts[i]);
                    }
                }
                return entities.ToArray();
            }
        }

        public GameSettings ClientSettings
        {
            get
            {
                return _clientSettings;
            }
        }
        
        public string Mod
        {
            get
            {
                return _activeMod;
            }
        }        

        public FileManager(Game1 game)
        {
            _game = game;
            if(File.Exists(Environment.CurrentDirectory + @"/startmod.txt"))
            {
                if(File.ReadAllLines(Environment.CurrentDirectory + @"/startmod.txt").Length > 0)
                {
                    _activeMod = File.ReadAllLines(Environment.CurrentDirectory + @"/startmod.txt")[0].Trim(new[] { ' ', '\t' });
                }
            }
            if(!Directory.Exists(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory))
            {
                Directory.CreateDirectory(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory);
            }
            FindAllScripts(new DirectoryInfo(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory));
            RetrieveClientSettings();
            _nextUpdate = DateTime.Now.AddSeconds(ClientSettings.ScriptRefreshRate);
            FindAllSprites(new DirectoryInfo(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory));
            FindAllFonts(new DirectoryInfo(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory));
            FindAllSVGs(new DirectoryInfo(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory));
        }

        public void Update()
        {
            if(DateTime.Now >= _nextUpdate)
            {

                for(int i = 0; i < _scripts.Count; i++)
                {
                    _scripts[i].UpdateScript();
                }
                _nextUpdate = DateTime.Now.AddSeconds(ClientSettings.ScriptRefreshRate);
            }
        }

        private void FindAllScripts(DirectoryInfo directory)
        {
            _folders.Add(directory.FullName + @"/?.lua");
            foreach(DirectoryInfo i in directory.GetDirectories())
            {
                FindAllScripts(i);
            }
            foreach(FileInfo i in directory.GetFiles("*." + "lua"))
            {
                if(_activeMod != "")
                {
                    string[] tmpText = File.ReadAllLines(i.FullName);
                    if(tmpText.Length > 0 && tmpText[0].Length > ("--MOD:").Length && tmpText[0].Substring(0, 6) == "--MOD:")
                    {
                        if(tmpText[0].Length == ("--MOD:" + _activeMod).Length)
                        {
                            if((tmpText[0].Substring(6)) == _activeMod)
                            {
                                _scripts.Add(new ScriptInfo(i, _game, GetChecksum(i.FullName)));
                            }
                        }
                    }
                    else
                    {
                        _scripts.Add(new ScriptInfo(i, _game, GetChecksum(i.FullName)));
                    }
                }
                else
                {
                    _scripts.Add(new ScriptInfo(i, _game, GetChecksum(i.FullName)));
                }
                
            }
        }

        public string FindReplay(string name, DirectoryInfo directory = null)
        {
            if(directory == null)
            {     
                directory = new DirectoryInfo(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/");
            }
            foreach(DirectoryInfo i in directory.GetDirectories())
            {
                string ret = FindReplay(name, i);
                if(ret != "")
                {
                    return ret;
                }
            }
            foreach(FileInfo i in directory.GetFiles("*.rep"))
            {
                if(i.Name == name + ".rep")
                {
                    return File.ReadAllText(i.FullName);
                }  
            }
            return "";
        }

        private void FindAllSprites(DirectoryInfo directory)
        {
            if(directory.Name == "Screenshots")
            {
                return;
            }
            foreach(DirectoryInfo i in directory.GetDirectories())
            {
                FindAllSprites(i);
            }
            foreach(FileInfo i in directory.GetFiles("*.png"))
            {
                string name = i.FullName.Substring((System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory).Length + 1);
                name = name.Substring(0, name.Length - 4);
                _sprites.Add(name);   
            }
        }

        private void FindAllSVGs(DirectoryInfo directory)
        {
            foreach(DirectoryInfo i in directory.GetDirectories())
            {
                FindAllSVGs(i);
            }
            foreach(FileInfo i in directory.GetFiles("*.svg"))
            {                                    
                string name = i.FullName.Substring((System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory).Length + 1);
                name = name.Substring(0, name.Length - 4);    
                _svgs.Add(name);
            }
        }

        private void FindAllFonts(DirectoryInfo directory)
        {
            foreach(DirectoryInfo i in directory.GetDirectories())
            {
                FindAllFonts(i);
            }
            foreach(FileInfo i in directory.GetFiles("*.xnb"))
            {
                string name = i.FullName.Substring((System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory).Length + 1);
                name = name.Substring(0, name.Length - 4);
                _fonts.Add(name);
            }
        }

        private void RetrieveClientSettings()
        {
            string location = System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"\Configuration\ClientSettings.json";
            string fileContent = "";
            string line = "";
            StreamReader fStream = new StreamReader(location);
            while((line = fStream.ReadLine()) != null)
            {
                fileContent += line + Environment.NewLine;
            }
            fStream.Close();  
            _clientSettings = JsonConvert.DeserializeObject<GameSettings>(fileContent);
            _clientSettings._game = _game;
            _clientSettings.ApplySettings();
        }

        public string GetChecksum(string filename)
        {
            using(var md5 = MD5.Create())
            {
                using(var stream = File.OpenRead(filename))
                {                                    
                    return BitConverter.ToString(md5.ComputeHash(stream)).ToLower();
                }
            }
        }

        public void SaveSettings()
        {
            string newSettings = JsonConvert.SerializeObject(_clientSettings, Formatting.Indented);
            string location = Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"\Configuration\ClientSettings.json";
            File.WriteAllText(location, newSettings);
            SaveKeyBinds();
        }

        public void LogClientSettings()
        {
            string[] settings = JsonConvert.SerializeObject(_clientSettings, Formatting.Indented).Split('\n');
            foreach(string i in settings)
            {
                _game.Log.Write(i.Replace("\\r", ""));
            }
        }

        public void LogKeybinds()
        {
            string[] keys = File.ReadAllText(Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"\Configuration\Keybinds.json").Split('\n');
            foreach(string i in keys)
            {
                _game.Log.Write(i.Replace("\\r", ""));
            }
        }

        public Keybind[] RetrieveKeybinds()
        {
            string location = Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"\Configuration\Keybinds.json";
            string fileContent = "";
            string line = "";
            StreamReader fStream = new StreamReader(location);
            while((line = fStream.ReadLine()) != null)
            {
                fileContent += line + Environment.NewLine;
            }
            fStream.Close();
            Keybind[] binds = JsonConvert.DeserializeObject<Keybind[]>(fileContent);
            return binds;
        }

        public void SaveKeyBinds()
        {
            if(_game.Input == null)
            {
                return;
            }
            string location = System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"\Configuration\Keybinds.json";
            string newBinds = JsonConvert.SerializeObject(_game.Input.Binds, Formatting.Indented);
            File.WriteAllText(location, newBinds);
        }
    }
}
