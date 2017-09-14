using d4lilah.Data;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace d4lilah
{
    public class InputManager
    {
        private Game1 _game;
        private List<Keybind> _keybinds = new List<Keybind>();
        private float _scrollVal = 0f;
        public bool IsReplay = false;
        private FrameMaster _replayMaster;   
        private float _replayIntervalCounter = 0f;
        private int _replayIndex = 0;
        private string[] _replayHotkeys = { "console", "showinterface" };
        private List<Keybind> _replayBinds = new List<Keybind>();

        public Frame ReplayFrame
        {
            get
            {
                return _replayMaster.Frame[_replayIndex];
            }
        }

        public string[] GetPressedKeys
        {
            get
            {
                List<string> keys = new List<string>();
                for(int i = 0; i < _keybinds.Count; i++)
                {
                    if(_keybinds[i].Down)
                    {
                        if(_keybinds[i].Name.ToLower() != "console")
                        {
                            keys.Add(_keybinds[i].Name);
                        }
                    }
                }
                return keys.ToArray();
            }
        }

        public Keybind[] Binds
        {
            get
            {
                return _keybinds.ToArray();
            }   
        }

        public InputManager(Game1 game)
        {
            _game = game;
            _keybinds.AddRange(_game.Files.RetrieveKeybinds());
            for(int i = 0; i < _keybinds.Count; i++)
            {
                for(int x = 0; x < _replayHotkeys.Length; x++)
                {
                    if(_keybinds[i].Name == _replayHotkeys[x])
                    {
                        _replayBinds.Add(_keybinds[i]);
                        break;
                    }
                }
            }
        }

        public void Update()
        {
            if(!IsReplay)
            {
                _replayIntervalCounter = 0;
                _replayIndex = 0;
                if(!_game.IsActive && !_game.Graphics.IsFullScreen)
                {
                    return;
                }
                MouseState mousein = Mouse.GetState();
                float scrollVal = _scrollVal - mousein.ScrollWheelValue;
                _scrollVal = mousein.ScrollWheelValue;
                KeyboardState mState = Keyboard.GetState();
                for(int i = 0; i < _keybinds.Count; i++)
                {
                    if(_keybinds[i].XnaKey != null)
                    {
                        _keybinds[i].SetState(mState.IsKeyDown((Keys)_keybinds[i].XnaKey));
                    }
                    else
                    {
                        bool runMouse = true;
                        if(runMouse)
                        {
                            if(_keybinds[i].MouseButton == "mouse1")
                            {
                                _keybinds[i].SetState(mousein.LeftButton == ButtonState.Pressed);
                            }
                            else if(_keybinds[i].MouseButton == "mouse2")
                            {
                                _keybinds[i].SetState(mousein.RightButton == ButtonState.Pressed);
                            }
                            else if(_keybinds[i].MouseButton == "mouse3")
                            {
                                _keybinds[i].SetState(mousein.XButton1 == ButtonState.Pressed);
                            }
                            else if(_keybinds[i].MouseButton == "mouse4")
                            {
                                _keybinds[i].SetState(mousein.XButton2 == ButtonState.Pressed);
                            }
                            if(_keybinds[i].MouseButton == "mwheelup")
                            {
                                _keybinds[i].SetState(0 > scrollVal);
                            }
                            if(_keybinds[i].MouseButton == "mwheeldown")
                            {
                                _keybinds[i].SetState(0 < scrollVal);
                            }
                        }
                    }
                }
            }
            else
            {                                  
                if(_replayMaster == null)
                {
                    IsReplay = false;
                    _game.Log.Write(Debug.ConsoleColorCoding.Warning + "Replay seems to be broken. Restarting game");
                    _game.Levels.SetDefaultLevel();
                    return;
                }
                if(_game.Levels.ChangingLevel)
                {
                    return;
                }
                _game.IsMouseVisible = true;
                _game.Time.Year = ReplayFrame.Time.Year;
                _game.Time.Month = ReplayFrame.Time.Month;
                _game.Time.Day = ReplayFrame.Time.Day;
                _game.Time.Hour = ReplayFrame.Time.Hour;
                _game.Time.Minute = ReplayFrame.Time.Minute;
                _game.Time.Second = ReplayFrame.Time.Second;
                _game.Time.Millisecond = ReplayFrame.Time.Millisecond;
                _game.Time.DaylightSavingTime = ReplayFrame.Time.DaylightSavingTime;
                _game.Time.DayOfWeek = ReplayFrame.Time.DayOfWeek;
                _game.Time.MonthName = ReplayFrame.Time.MonthName;
                _game.Time.DayOfYear = ReplayFrame.Time.DayOfYear;
                _game.Time.WeekOfYear = ReplayFrame.Time.WeekOfYear;
                if(_replayIntervalCounter >= _replayMaster.Interval)
                {
                    _replayIndex++;
                    _replayIntervalCounter = 0;
                    if(_replayIndex >= _replayMaster.Frame.Length)
                    {
                        IsReplay = false;
                        _game.Log.Write("Replay ended.");
                        _game.Levels.SetDefaultLevel();
                        return;
                    }
                }
                _replayIntervalCounter += _game.Time.DeltaTime * _game.Levels.CurrentLevel.Timescale;
                if(!_game.IsActive && !_game.Graphics.IsFullScreen)
                {
                    return;
                }
                MouseState mousein = Mouse.GetState();
                float scrollVal = _scrollVal - mousein.ScrollWheelValue;
                _scrollVal = mousein.ScrollWheelValue;
                KeyboardState mState = Keyboard.GetState();
                for(int i = 0; i < _replayBinds.Count; i++)
                {
                    if(_replayBinds[i].XnaKey != null)
                    {
                        _replayBinds[i].SetState(mState.IsKeyDown((Keys)_replayBinds[i].XnaKey));
                    }
                    else
                    {
                        bool runMouse = true;
                        if(runMouse)
                        {
                            if(_replayBinds[i].MouseButton == "mouse1")
                            {
                                _replayBinds[i].SetState(mousein.LeftButton == ButtonState.Pressed);
                            }
                            else if(_replayBinds[i].MouseButton == "mouse2")
                            {
                                _replayBinds[i].SetState(mousein.RightButton == ButtonState.Pressed);
                            }
                            else if(_replayBinds[i].MouseButton == "mouse3")
                            {
                                _replayBinds[i].SetState(mousein.XButton1 == ButtonState.Pressed);
                            }
                            else if(_replayBinds[i].MouseButton == "mouse4")
                            {
                                _replayBinds[i].SetState(mousein.XButton2 == ButtonState.Pressed);
                            }
                            if(_replayBinds[i].MouseButton == "mwheelup")
                            {
                                _replayBinds[i].SetState(0 > scrollVal);
                            }
                            if(_replayBinds[i].MouseButton == "mwheeldown")
                            {
                                _replayBinds[i].SetState(0 < scrollVal);
                            }
                        }
                    }
                }
            }   
        }

        public void ClearBinds()
        {
            _keybinds.Clear();
        }

        public void AddBind(string name, string key)
        {
            _keybinds.Add(new Keybind(key, name));
            foreach(string i in _replayHotkeys)
            {
                if(name == i)
                {
                    _replayBinds.Add(new Keybind(key, name));
                    return;
                }
            }
        }

        public bool KeyReleased(string name)
        {
            if(!IsReplay)
            {     
                for(int i = 0; i < _keybinds.Count; i++)
                {
                    if(name == _keybinds[i].Name)
                    {
                        if(_keybinds[i].Released)
                        {

                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                for(int i = 0; i < _replayBinds.Count; i++)
                {
                    if(name == _replayBinds[i].Name)
                    {
                        if(_replayBinds[i].Released)
                        {

                            return true;
                        }
                    }
                }
                if(_replayIndex == 0)
                {
                    return false;
                }
                bool isKeyDown = false;
                for(int i = 0; i < _replayMaster.Frame[_replayIndex].KeyStrokes.Length; i++)
                {
                    if(_replayMaster.Frame[_replayIndex].KeyStrokes[i] == name)
                    {
                        isKeyDown = true;
                        break;
                    }
                }
                if(isKeyDown)
                {
                    return false;
                } 
                bool wasKeyDown = false;
                for(int i = 0; i < _replayMaster.Frame[_replayIndex - 1].KeyStrokes.Length; i++)
                {
                    if(_replayMaster.Frame[_replayIndex - 1].KeyStrokes[i] == name)
                    {
                        wasKeyDown = true;
                        break;
                    }
                }
                if(wasKeyDown && !isKeyDown)
                {
                    return true;
                }
                return false;
            }
        }

        public bool KeyPressed(string name)
        {
            if(!IsReplay)
            {
                for(int i = 0; i < _keybinds.Count; i++)
                {
                    if(name == _keybinds[i].Name)
                    {
                        if(_keybinds[i].Pressed)
                        {

                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                for(int i = 0; i < _replayBinds.Count; i++)
                {
                    if(name == _replayBinds[i].Name)
                    {
                        if(_replayBinds[i].Pressed)
                        {

                            return true;
                        }
                    }
                }
                bool isKeyDown = false;
                for(int i = 0; i < _replayMaster.Frame[_replayIndex].KeyStrokes.Length; i++)
                {
                    if(_replayMaster.Frame[_replayIndex].KeyStrokes[i] == name)
                    {
                        isKeyDown = true;
                        break;
                    }
                }
                if(_replayIndex == 0)
                {
                    return isKeyDown;
                }
                bool wasKeyDown = false;
                for(int i = 0; i < _replayMaster.Frame[_replayIndex - 1].KeyStrokes.Length; i++)
                {
                    if(_replayMaster.Frame[_replayIndex - 1].KeyStrokes[i] == name)
                    {
                        wasKeyDown = true;
                        break;
                    }
                }
                if(!wasKeyDown && isKeyDown)
                {
                    return true;
                }                
                return false;
            }
        }

        public bool KeyDown(string name)
        {
            if(!IsReplay)
            {

                for(int i = 0; i < _keybinds.Count; i++)
                {
                    if(name == _keybinds[i].Name)
                    {
                        if(_keybinds[i].Down)
                        {

                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                for(int i = 0; i < _replayBinds.Count; i++)
                {
                    if(name == _replayBinds[i].Name)
                    {
                        if(_replayBinds[i].Down)
                        {

                            return true;
                        }
                    }
                }
                for(int i = 0; i < _replayMaster.Frame[_replayIndex].KeyStrokes.Length; i++)
                {
                    if(_replayMaster.Frame[_replayIndex].KeyStrokes[i] == name)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public int StartReplay(string name)
        {
            string rawMaster = _game.Files.FindReplay(name);
            if(rawMaster == "")
            {
                _game.Log.Write(Debug.ConsoleColorCoding.Error + "Replay '" + Debug.ConsoleColorCoding.String + name + ".rep'" + Debug.ConsoleColorCoding.Error + " was not found!");
                return -1;
            }
            _replayMaster = JsonConvert.DeserializeObject<FrameMaster>(rawMaster);
            if(_replayMaster.Checksums.Length > _game.Files.GetAllMD5().Length)
            {
                _game.Log.Write(Debug.ConsoleColorCoding.Error + "Checksum does not add up. Is correct mod loaded?");
                return -1;
            }
            string[] loadedChecksums = _game.Files.GetAllMD5();
            for(int i = 0; i < _replayMaster.Checksums.Length; i++)
            {
                bool isVerified = false;
                for(int x = 0; x < loadedChecksums.Length; x++)
                {
                    if(_replayMaster.Checksums[i] == loadedChecksums[x])
                    {
                        isVerified = true;
                        break;
                    }
                }
                if(isVerified)
                {
                    _game.Log.Write(Debug.ConsoleColorCoding.Normal + "[" + Debug.ConsoleColorCoding.Numeric + _replayMaster.Checksums[i] + Debug.ConsoleColorCoding.Normal + "]" + Debug.ConsoleColorCoding.Success + " verified!");
                }
                else
                {
                    _game.Log.Write(Debug.ConsoleColorCoding.Normal + "[" + Debug.ConsoleColorCoding.Numeric + _replayMaster.Checksums[i] + Debug.ConsoleColorCoding.Normal + "]" + Debug.ConsoleColorCoding.Error + " could not be verified! Is correct mod loaded?");
                    return -1;
                }
            }
            _game.Log.Write(Debug.ConsoleColorCoding.Success + "Replay '" + Debug.ConsoleColorCoding.String + name + ".rep'" + Debug.ConsoleColorCoding.Success + " found!");
            _game.Log.Write(name + " ticks: " + Debug.ConsoleColorCoding.Numeric + _replayMaster.Frame.Length);
            _game.Log.Write(name + " date: " + Debug.ConsoleColorCoding.String + _replayMaster.Date);
            _game.Log.Write(name + " level: " + Debug.ConsoleColorCoding.String + _replayMaster.Name);
            _game.Log.Write(name + " seed: " + Debug.ConsoleColorCoding.Numeric + _replayMaster.Seed);
            _game.Console.Close();
            _game.Levels.ChangeLevel(_replayMaster.Name);
            IsReplay = true;     
            return 1;
        }

        public int StopReplay()
        {
            if(!IsReplay)
            {
                _game.Log.Write("No active replay.");
                return -1;
            }
            _replayIndex = _replayMaster.Frame.Length - 1;
            _game.Console.Close();
            return 1;
        }
    }

    public class ExposedInputClass
    {
        [MoonSharpHidden]
        private Game1 _game;

        [MoonSharpHidden]
        public ExposedInputClass(Game1 game)
        {
            _game = game;
        }


        public int AddBind(string name, string key)
        {
            _game.Input.AddBind(name, key);
            _game.Files.SaveKeyBinds();
            return 1;
        }

        public int ListBinds(string value = "")
        {
            if(value != "")
            {
                foreach(Keybind entry in _game.Input.Binds)
                {
                    if(entry.XnaKey == KeyTranslation.Translate(value))
                    {
                        if(entry.XnaKey != null)
                        {
                            _game.Log.Write(entry.XnaKey + " : " + entry.Name);
                        }
                        else
                        {
                            _game.Log.Write(entry.MouseButton + " : " + entry.Name);
                        }
                    }
                    if(entry.Name == value)
                    {
                        if(entry.XnaKey != null)
                        {
                            _game.Log.Write(entry.XnaKey + " : " + entry.Name);
                        }
                        else
                        {
                            _game.Log.Write(entry.MouseButton + " : " + entry.Name);
                        }
                    }
                }
            }
            else
            {
                foreach(Keybind entry in _game.Input.Binds)
                {
                    if(entry.XnaKey != null)
                    {
                        _game.Log.Write(entry.XnaKey + " : " + entry.Name);
                    }
                    else
                    {
                        _game.Log.Write(entry.MouseButton + " : " + entry.Name);
                    }
                }
            }
            return 1;
        }

        public int DeleteBind(string value, string key = "")
        {
            if(key != "")
            {
                Dictionary<string, string> newbind = new Dictionary<string, string>();
                _game.Log.Write("Deleting following keys...");
                Keybind[] tmpBinds = _game.Input.Binds;
                _game.Input.ClearBinds();
                foreach(Keybind entry in tmpBinds)
                {
                    if((entry.XnaKey == KeyTranslation.Translate(value) || entry.MouseButton == value) && entry.Name == key)
                    {
                        if(entry.XnaKey != null)
                        {
                            _game.Log.Write(entry.XnaKey.ToString() + " : " + entry.Name);
                        }
                        else
                        {
                            _game.Log.Write(entry.MouseButton + " : " + entry.Name);
                        }
                    }
                    else if((entry.XnaKey == KeyTranslation.Translate(key) || entry.MouseButton == key) && entry.Name == value)
                    {
                        if(entry.XnaKey != null)
                        {
                            _game.Log.Write(entry.XnaKey.ToString() + " : " + entry.Name);
                        }
                        else
                        {
                            _game.Log.Write(entry.MouseButton + " : " + entry.Name);
                        }
                    }
                    else
                    {
                        if(entry.XnaKey != null)
                        {
                            _game.Input.AddBind(entry.Name, entry.XnaKey.ToString());
                        }
                        else
                        {
                            _game.Input.AddBind(entry.Name, entry.MouseButton);
                        }
                    }
                }
            }
            else
            {
                Dictionary<string, string> newbind = new Dictionary<string, string>();
                Keybind[] tmpBinds = _game.Input.Binds;
                _game.Input.ClearBinds();
                _game.Log.Write("Deleting following keys...");
                foreach(Keybind entry in tmpBinds)
                {
                    if(entry.XnaKey.ToString() == value || entry.MouseButton == value)
                    {
                        if(entry.XnaKey != null)
                        {
                            _game.Log.Write(entry.XnaKey + " : " + entry.Name);
                        }
                        else
                        {
                            _game.Log.Write(entry.MouseButton + " : " + entry.Name);
                        }
                    }
                    else if(entry.Name == value)
                    {
                        if(entry.XnaKey != null)
                        {
                            _game.Log.Write(entry.XnaKey + " : " + entry.Name);
                        }
                        else
                        {
                            _game.Log.Write(entry.MouseButton + " : " + entry.Name);
                        }
                    }
                    else
                    {
                        if(entry.XnaKey != null)
                        {
                            _game.Input.AddBind(entry.Name, entry.XnaKey.ToString());
                        }
                        else
                        {
                            _game.Input.AddBind(entry.Name, entry.MouseButton);
                        }
                    }
                }
            }
            _game.Files.SaveKeyBinds();
            return 1;
        }

        public void ListKeycodes()
        {
            _game.Log.Write("mouse1");
            _game.Log.Write("mouse2");
            _game.Log.Write("mouse3");
            _game.Log.Write("mouse4");
            _game.Log.Write("mwheelup");
            _game.Log.Write("mwheeldown");  
            Keys[] values = (Keys[])Enum.GetValues(typeof(Keys));  
            foreach(Keys k in values)
            {
                _game.Log.Write(k.ToString());
            }
        }
    }
}
