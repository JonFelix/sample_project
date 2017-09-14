using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace d4lilah.Data
{       
    public class Instance
    {
        public Script Script;
        public List<Table> MessageQueue = new List<Table>();
        public List<DateTime> Timers = new List<DateTime>();
        public List<Table> TimerPayloads = new List<Table>();
        public Table StartParams;
        public bool IsMenuLevel = false;
        public DateTime UpdateChecked;
        public bool RemoveMe = false;
        public float CollisionRad = 0f;

        public Random Random;

        private Game1 _game;
        private Stats _stats;
        private string _scriptString;
        private ScriptInfo _scriptInfo;

        public Stats Stats
        {
            get
            {
                return _stats;
            }
        }

        public ScriptInfo ScriptInfo
        {
            get
            {
                return _scriptInfo;
            }
        }
        
        public Instance(Game1 game, string script, string name, ScriptInfo scriptinfo)
        {    
            _game = game;
            _stats = new Stats(_game, this);
            _stats.Name = name;
            _scriptString = script;
            _scriptInfo = scriptinfo;
            UpdateChecked = _scriptInfo.Updated;
            Script = new Script();
            if(File.Exists(_game.ContentLocation + @"\Configuration\Scripts\" + _game.Files.Mod + "_" + _stats.Name + ".json"))
            {
                string content = File.ReadAllText(_game.ContentLocation + @"\Configuration\Scripts\" + _game.Files.Mod + "_" + _stats.Name + ".json");
                Dictionary<string, object> tmpDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            }
            if(_game.Levels.CurrentLevel == null)
            {
                Random = new Random();
            }
            else
            {
                Random = new Random(_game.Levels.CurrentLevel.Seed);
            }
                
        }     

        public void UpdateScript()
        {
            _scriptString = _scriptInfo.RawScript;
            _game.Log.Write(_stats.Name + " refreshed!");
            Script.DoString(_scriptString);
            UpdateChecked = DateTime.Now;  
        }

        public void PrepareScript()
        {                                   
            Script = _game.Pipeline.BakeInstance(Script, Stats);
            if(Script.SourceCodeCount <= 1)
            {
                ((ScriptLoaderBase)Script.Options.ScriptLoader).ModulePaths = _game.Files.Folders;
                try
                {
                    Script.DoString(_scriptString);
                }
                catch(Exception e)
                {
                    _game.Log.Write(_stats.Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                }
            }
        }

        public void PrepareLevel()
        {
            Script = _game.Pipeline.BakeLevel(Script, Stats);
            if(Script.SourceCodeCount <= 1)
            {
                ((ScriptLoaderBase)Script.Options.ScriptLoader).ModulePaths = _game.Files.Folders;
                try
                {
                    Script.DoString(_scriptString);
                }
                catch(SyntaxErrorException e)
                {
                    _game.Log.Write(_stats.Name + ": " + e.DecoratedMessage);
                }
            }
        }       
    }

    public class Stats
    {
        public bool IsInitialized = false;
        public int ID = -1;
        private float _direction = 0;
        public float Depth = 0;
        private ShellVector _position = new ShellVector(new Vector2(0, 0));
        private ShellTexture _texture = null;
        private bool _isCollidable = false;
        public Table StartParameters = null;
        public string Name;
        private Dictionary<string, object> _config = new Dictionary<string, object>();

        [MoonSharpHidden]
        public Instance Instance
        {
            get
            {
                return _parent;
            }
        }

        [MoonSharpHidden]
        private Game1 _game;
        [MoonSharpHidden]
        private Instance _parent;

        public float Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;      
            }
        }

        public ShellTexture Texture
        {
            get
            {
                return _texture;
            }    
        }

            
        public bool IsCollidable
        {
            get
            {
                return _isCollidable;
            }
            set
            {
                if(Texture != null)
                {
                    if(!_isCollidable && value)
                    {
                        _game.Levels.CurrentLevel.QuadTree.PlaceEntity(this);
                    }
                    if(_isCollidable && !value)
                    {
                        _game.Levels.CurrentLevel.QuadTree.RemoveEntity(this);
                    }
                    _isCollidable = value;

                }
                else
                {
                    _game.Log.Write(Name + ": Attempted to set Entity.IsCollidable to true while no texture was assigned");
                }
            }
        }

        public ShellVector Position
        {
            get
            {
                return _position;
            }
            set
            {
                if(IsCollidable && Texture != null)
                {
                    _game.Levels.CurrentLevel.QuadTree.RemoveEntity(this);
                }
                _position = value;
                if(IsCollidable && Texture != null)
                {
                    _game.Levels.CurrentLevel.QuadTree.PlaceEntity(this);
                }
            }
        }

        [MoonSharpHidden]
        public Stats(Game1 game, Instance parent)
        {
            _game = game;
            _parent = parent;
        }

        [MoonSharpHidden]
        public int SetSeed(int seed)
        {            
            _parent.Random = new Random(seed);
            _game.Log.Write("#A8B2AFSet seed #2645FF" + seed + "#A8B2AF for entity #2645FF" + ID);
            return 1;
        }

        [MoonSharpHidden]
        public int Move(Table position)
        {
            float x = 0, y = 0;
            foreach(TablePair pair in position.Pairs)
            {
                if(pair.Key.String.ToLower() == "x")
                {
                    x = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "y")
                {
                    y = (float)pair.Value.Number;
                }

            }
            if(IsCollidable && Texture != null)
            {
                _game.Levels.CurrentLevel.QuadTree.RemoveEntity(this);
            }
            Position.Vector += new Vector2(x, y);
            if(IsCollidable && Texture != null)
            {
                _game.Levels.CurrentLevel.QuadTree.PlaceEntity(this);
            }
            return 1;
        }

        [MoonSharpHidden]
        public float GetDirection()
        {
            return Direction;
        }

        [MoonSharpHidden]
        public int GetID()
        {
            return ID;
        }

        [MoonSharpHidden]
        public float GetDepth()
        {
            return Depth;
        }

        [MoonSharpHidden]
        public ShellVector GetPosition()
        {
            return Position;
        }

        [MoonSharpHidden]
        public ShellTexture GetTexture()
        {
            return Texture;
        }

        [MoonSharpHidden]
        public bool GetCollidable()
        {
            return IsCollidable;
        }

        [MoonSharpHidden]
        public string GetName()
        {
            return Name;
        }
             
        [MoonSharpHidden]
        public int SetTexture(string location)
        {
            if(location == "")
            {
                _texture = null;
                return 1;
            }   
            else
            {
                Texture2D text = _game.Sprites.GetSprite(location);
                if(text != null)
                {
                    _texture = new ShellTexture(text);
                    _parent.CollisionRad = Vector2.Distance(Vector2.Zero, new Vector2(text.Width, text.Height));
                    return 1;
                }
                return -1;
            }
        }
           
        [MoonSharpHidden]
        public int MenuLevel(bool val)
        {
            _parent.IsMenuLevel = val;
            return 1;
        }

        [MoonSharpHidden]
        public int AddTimerPayload(Table time, Table payload)
        {
            int h = 0, m = 0, s = 0, ms = 0;
            foreach(TablePair pair in time.Pairs)
            {
                if(pair.Key.String.ToLower() == "hour")
                {
                    h = Math.Min(Math.Max((int)pair.Value.Number, 0), 23);
                }
                if(pair.Key.String.ToLower() == "minute")
                {
                    m = Math.Min(Math.Max((int)pair.Value.Number, 0), 59);
                }
                if(pair.Key.String.ToLower() == "second")
                {
                    s = Math.Min(Math.Max((int)pair.Value.Number, 0), 59);
                }
                if(pair.Key.String.ToLower() == "millisecond")
                {
                    ms = Math.Min(Math.Max((int)pair.Value.Number, 0), 999);
                }
            }
            DateTime timer = DateTime.Now;
            TimeSpan timerAddition = new TimeSpan(0, h, m, s, ms);
            timer = timer.Add(timerAddition);
            _parent.Timers.Add(timer);
            _parent.TimerPayloads.Add(payload);
            return 1;
        }
        [MoonSharpHidden]
        public float Random(float min, float max)
        {
            return (float)Math.Max(_parent.Random.NextDouble() * max, min);
        }

        [MoonSharpHidden]
        public int SaveVariable(string name, Table table)
        {
            if(!File.Exists(_game.ContentLocation + @"\Configuration\Scripts\" + _game.Files.Mod + "_" + Name + ".json"))
            {
                if(!Directory.Exists(_game.ContentLocation + @"\Configuration\Scripts\"))
                {
                    Directory.CreateDirectory(_game.ContentLocation + @"\Configuration\Scripts\");
                }
                File.Create(_game.ContentLocation + @"\Configuration\Scripts\" + _game.Files.Mod + "_" + Name + ".json");
                _game.Log.Write("Creating settings file for " + Name);
            }
            if(!_config.ContainsKey(name))
            {
                _config.Add(name, JsonConvert.DeserializeObject(MoonSharp.Interpreter.Serialization.Json.JsonTableConverter.TableToJson(table)));
            }
            else
            {
                _config[name] = JsonConvert.DeserializeObject(MoonSharp.Interpreter.Serialization.Json.JsonTableConverter.TableToJson(table));
            }
            string json = JsonConvert.SerializeObject(_config, Formatting.Indented);
            File.WriteAllText(_game.ContentLocation + @"\Configuration\Scripts\" + _game.Files.Mod + "_" + Name + ".json", json);
            return 1;
        }

        [MoonSharpHidden]
        public Table GetVariable(string name)
        {
            if(_config.ContainsKey(name))
            {
                string json = JsonConvert.SerializeObject(_config[name]);
                return MoonSharp.Interpreter.Serialization.Json.JsonTableConverter.JsonToTable(json);
            }
            else
            {
                _game.Log.Write("'" + name + "' was not found.");
                return null;
            }
        }
    }
}
