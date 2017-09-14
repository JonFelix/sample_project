using d4lilah.Data;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace d4lilah
{
    public class LevelManager
    {
        private Game1 _game;
        private ScriptInfo[] _levels;
        private Level _activeLevel;
        private int _changeLevel = -1;


        public bool QuadInitiated = false;
        public bool LevelInitiated = false;
        public bool LevelCleanedUp = false;
        public bool Widgetsinitiated = false;
        public bool LevelPrepared = false;
                                           

        public ExposedLevelFunctions ExposedLevelFunctions;

        public Level CurrentLevel
        {
            get
            {
                return _activeLevel;
            }
        }

        public bool ChangingLevel
        {
            get
            {
                return _changeLevel != -1;
            }
        }
        public ScriptInfo NextLevel
        {
            get
            {
                if(_changeLevel != -1)
                {
                    return _levels[_changeLevel];
                }
                return null;
            }
        }

        public string[] Levels
        {
            get
            {
                string[] levels = new string[_levels.Length];
                for(int i = 0; i < _levels.Length; i++)
                {
                    levels[i] = _levels[i].Name;
                }
                return levels;
            }
        }

        public LevelManager(Game1 game)
        {
            _game = game;
            _levels = _game.Files.LevelScripts;
            ExposedLevelFunctions = new ExposedLevelFunctions(_game);
            _game.Exiting += Close;
            SetDefaultLevel();                       
        }

        public void Update()
        {         
            if(_changeLevel != -1)
            {       
                if(!LevelCleanedUp)
                {
                    _game.ParticleEffects.ClearParticles();
                    _game.Interface.CleanUpWidgets();
                    _game.Entities.CleanUpEntities();
                    _game.Decals.CleanUp();
                    LevelCleanedUp = true;
                }
                else if(!LevelPrepared)
                {
                    Level level = new Level(_game, _levels[_changeLevel].RawScript, _levels[_changeLevel].Name, _levels[_changeLevel]);
                    _activeLevel = level;
                    _activeLevel.Script = _game.Pipeline.InitLevel(_activeLevel.Script, _activeLevel.Stats);
                    LevelPrepared = true;
                }
                else if(!LevelInitiated)
                {
                    _activeLevel.PrepareLevel();
                    if(!_activeLevel.Stats.IsInitialized)
                    {
                        if(_activeLevel.Script.Globals["Initialize"] != null)
                        {
                            try
                            {
                                _activeLevel.Script.Call(_activeLevel.Script.Globals["Initialize"]);
                            }
                            catch(Exception e)
                            {
                                _game.Log.Write(_activeLevel.Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                            }
                            _activeLevel.Stats.IsInitialized = true;
                        }
                    }  
                    LevelInitiated = true;
                }
                else if(!QuadInitiated)
                {        
                    _activeLevel.QuadTree = new CollisionQuad(_activeLevel.Boundaries, 64f);
                    QuadInitiated = true;
                }
                else if(!Widgetsinitiated)
                {
                    _game.Interface.InitializeWidgets();
                    Widgetsinitiated = true;
                }
                else
                {
                    _changeLevel = -1;
                }  
                return;
            }
            if(_activeLevel == null)
            {
                return;
            }
            _activeLevel.PrepareLevel();
            if(_activeLevel.UpdateChecked < _activeLevel.ScriptInfo.Updated)
            {
                _activeLevel.UpdateScript();
            }
            if(!_activeLevel.Stats.IsInitialized)
            {
                _activeLevel.Script = _game.Pipeline.InitLevel(_activeLevel.Script, _activeLevel.Stats);
            }
            
            if(_activeLevel.Script.Globals["OnMessage"] != null)
            {
                for(int x = 0; x < _activeLevel.Timers.Count; x++)
                {
                    if(_activeLevel.Timers[x] <= DateTime.Now)
                    {
                        _activeLevel.MessageQueue.Add(_activeLevel.TimerPayloads[x]);
                        _activeLevel.Timers.RemoveAt(x);
                        _activeLevel.TimerPayloads.RemoveAt(x);
                    }
                }
                for(int x = 0; x < _activeLevel.MessageQueue.Count; x++)
                {
                    try
                    {
                        _activeLevel.Script.Call(_activeLevel.Script.Globals["OnMessage"], _activeLevel.MessageQueue[x]);
                    }
                    catch(Exception e)
                    {
                        _game.Log.Write(_activeLevel.Stats.Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                    }
                }
                _activeLevel.MessageQueue.Clear();
            }
            if(_activeLevel.Script.Globals["Update"] != null)
            {
                try
                {
                    _activeLevel.Script.Call(_activeLevel.Script.Globals["Update"]);
                }
                catch(Exception e)
                {
                    _game.Log.Write(_activeLevel.Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                }  
            }
            _activeLevel.Update(_game);
        }

        public int SetDefaultLevel()
        {
            for(int i = 0; i < _levels.Length; i++)
            {
                if(_levels[i].Name == "Start.level.lua")
                {
                    _changeLevel = i;
                    QuadInitiated = false;
                    LevelInitiated = false;
                    LevelCleanedUp = false;
                    Widgetsinitiated = false;
                    LevelPrepared = false;
                    if(CurrentLevel != null)
                    {
                        CurrentLevel.SaveReplay(_game);
                    }
                    return 1;
                }
            }
            _game.Log.Write(Debug.ConsoleColorCoding.Error + "Could not find " + Debug.ConsoleColorCoding.String + "'Start.level.lua'" + Debug.ConsoleColorCoding.Error + ". Did scripts load correctly?");
            return -1;
        }

        public int ChangeLevel(string levelname)
        {
            _game.Input.IsReplay = false;
            _activeLevel.SaveReplay(_game);
            if(levelname == "")
            {
                _game.Log.Write("Empty levelname, Quiting game.");
                _game.Exit();
                return 1;
            }
            for(int i = 0; i < _levels.Length; i++)
            {
                if(_levels[i].Name == levelname + ".level.lua")
                {
                    _changeLevel = i;
                    QuadInitiated = false;
                    LevelInitiated = false;
                    LevelCleanedUp = false;
                    Widgetsinitiated = false;
                    LevelPrepared = false;
                    return 1;
                }
            }
            _game.Log.Write("Level '" + levelname + "' was not found in the map pool.");
            return 0;
        }

        public int SetLevelBoundaries(Table rect)
        {
            _activeLevel.Boundaries = TranslateRect(rect);
            return 1;
        }

        private Rectangle TranslateRect(Table rect)
        {
            int x = 0, y = 0, w = 0, h = 0;
            foreach(TablePair pair in rect.Pairs)
            {
                if(pair.Key.String.ToLower() == "x")
                {
                    x = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "y")
                {
                    y = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "w" || pair.Key.String.ToLower() == "width")
                {
                    w = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "h" || pair.Key.String.ToLower() == "height")
                {
                    h = (int)pair.Value.Number;
                }
            }                                      
            return new Rectangle((int)x, (int)y, w, h);
        }

        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if(writer != null)
                    writer.Close();
            }
        }

        private void Close(object sender, EventArgs e)
        {
            _activeLevel.SaveReplay(_game);
        }

    }

    public class ExposedLevelFunctions
    {
        [JsonIgnoreAttribute]
        [MoonSharpHidden]
        private Game1 _game;

        public ExposedLevelFunctions(Game1 game)
        {
            _game = game;
        }

        public int ChangeLevel(string level)
        {
            return _game.Levels.ChangeLevel(level);
        }

        public Level CurrentLevel
        {
            get
            {
                return _game.Levels.CurrentLevel;
            }
        }

        public string [] Levels
        {
            get
            {
                return _game.Levels.Levels;
            }                             
        }

        public int QuadCount()
        {
            return _game.Levels.CurrentLevel.QuadTree.CountEntities(0);
        }
    }
}
