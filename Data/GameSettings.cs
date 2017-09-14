using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using Newtonsoft.Json;

namespace d4lilah.Data
{
    public class GameSettings
    {
        [JsonIgnoreAttribute]
        [MoonSharpHidden]
        public Game1                             _game;

        private int                              _width;
        private int                              _height;
        private bool                             _vsync;
        private bool                             _fullscreen;
        private bool                             _showMouse;
        private bool                             _enableLog;
        private bool                             _logTimestamp;
        private bool                             _openLogOnClose; 
        private bool                             _consolePersistentHistory;
        private int                              _scriptRefreshRate;
        private bool                             _consoleEnabled;
        private bool                             _logStopSpam;
        private float                            _masterVolume;
        private float                            _musicVolume;
        private float                            _ambienceVolume;
        private bool                             _dumpLog;
        private int                              _decalCount;
        private bool                             _autoRecord;
        private bool                             _consoleColorCoding;

        public bool ConsoleColorCoding
        {
            get
            {
                return _consoleColorCoding;
            }
            set
            {
                _consoleColorCoding = value;
            }
        }  

        public int DecalCount
        {
            get
            {
                return _decalCount;
            }
            set
            {
                _decalCount = value;
            }

        }

        public bool AutoRecord
        {
            get
            {
                return _autoRecord;
            }
            set
            {
                _autoRecord = value;
            }
        }


        public bool DumpLog
        {
            get
            {
                return _dumpLog;
            }
            set
            {
                _dumpLog = value;
            }
        }


        public float MasterVolume
        {
            get
            {
                return _masterVolume;
            }
            set
            {
                if(_masterVolume == value)
                {
                    return;
                }
                _masterVolume = value;
                if(_game != null)
                    _game.Log.Write("MasterVolume set to " + _masterVolume.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }

        public float MusicVolume
        {
            get
            {
                return _musicVolume;
            }
            set
            {
                if(_musicVolume == value)
                {
                    return;
                }
                _musicVolume = value;
                if(_game != null)
                    _game.Log.Write("MasterVolume set to " + _musicVolume.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }

        public float AmbienceVolume
        {
            get
            {
                return _ambienceVolume;
            }
            set
            {
                if(_ambienceVolume == value)
                {
                    return;
                }
                _ambienceVolume = value;
                if(_game != null)
                    _game.Log.Write("MasterVolume set to " + _ambienceVolume.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }

        

        public int ScriptRefreshRate
        {
            get
            {
                return _scriptRefreshRate;
            }
            set
            {
                if(_scriptRefreshRate == value)
                {
                    return;
                }
                _scriptRefreshRate = value;
                if(_game != null)
                    _game.Log.Write("ScriptRefreshRate set to " + _scriptRefreshRate.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }

        public bool ConsoleEnabled
        {
            get
            {
                return _consoleEnabled;
            }
            set
            {
                if(_consoleEnabled == value)
                {
                    return;
                }
                _consoleEnabled = value;
                if(_game != null)
                    _game.Log.Write("ConsoleEnabled set to " + _consoleEnabled.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }

        public bool LogStopSpam
        {
            get
            {
                return _logStopSpam;
            }
            set
            {
                if(_logStopSpam == value)
                {
                    return;
                }
                _logStopSpam = value;
                if(_game != null)
                    _game.Log.Write("LogStopSpam set to " + _logStopSpam.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }

        public bool ConsolePersistentHistory
        {
            get
            {
                return _consolePersistentHistory;
            }
            set
            {
                if(_consolePersistentHistory == value)
                {
                    return;
                }
                _consolePersistentHistory = value;
                if(_game != null)
                    _game.Log.Write("ConsolePersistentHistory set to " + _consolePersistentHistory.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }          

        public bool OpenLogOnClose
        {
            get
            {
                return _openLogOnClose;
            }
            set
            {
                if(_openLogOnClose == value)
                {
                    return;
                }
                _openLogOnClose = value;
                if(_game != null)
                    _game.Log.Write("OpenLogOnClose set to " + _openLogOnClose.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }

        public bool LogTimestamp
        {
            get
            {
                return _logTimestamp;
            }
            set
            {
                if(_logTimestamp == value)
                {
                    return;
                }
                _logTimestamp = value;
                if(_game != null)
                    _game.Log.Write("LogTimestamp set to " + _logTimestamp.ToString());
                if(_game != null)
                {
                    SerializeSettings();
                }
            }
        }        

        public bool EnableLog
        {
            get
            {
                return _enableLog;
            }
            set
            {
                if(_enableLog == value)
                {
                    return;
                }
                _enableLog = value;
                if(_game != null)
                    _game.Log.Write("EnableLog set to " + _enableLog.ToString());
                if(_game != null)
                {                                       
                    SerializeSettings();
                }
            }
        }


        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                if(_width == value)
                {
                    return;
                }
                _width = value;
                if(_game != null)
                    _game.Log.Write("Width set to " + _width.ToString());
                if(_game != null)
                {
                    _game.Graphics.PreferredBackBufferWidth = _width;
                    _game.Graphics.ApplyChanges();
                    SerializeSettings();
                    CenterScreen();
                    if(_game.Interface != null)
                    {
                        _game.Interface.ReloadInterface();
                    }
                }
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if(_height == value)
                {
                    return;
                }
                _height = value;
                if(_game != null)
                    _game.Log.Write("Height set to " + _height.ToString());
                if(_game != null)
                {
                    _game.Graphics.PreferredBackBufferHeight = _height;
                    _game.Graphics.ApplyChanges();
                    SerializeSettings();
                    CenterScreen();
                    if(_game.Interface != null)
                    {
                        _game.Interface.ReloadInterface();
                    }
                }
            }
        }

        public bool ShowMouse
        {
            get
            {
                return _showMouse;
            }
            set
            {
                if(_showMouse == value)
                {
                    return;
                }
                _showMouse = value;
                if(_game != null)
                    _game.Log.Write("ShowMouse set to " + _showMouse.ToString());
                if(_game != null)
                {
                    _game.IsMouseVisible = _showMouse;
                    SerializeSettings();
                }
            }
        }

        public bool Vsync
        {
            get
            {
                return _vsync;
                }
            set
            {
                if(_vsync == value)
                {
                    return;
                }
                _vsync = value;
                if(_game != null)
                    _game.Log.Write("Vsync set to " + _vsync.ToString());
                if(_game != null)
                {
                    _game.IsFixedTimeStep = _vsync;
                    SerializeSettings();
                    ApplySettings();
                }
            }
        }

        public bool Fullscreen
        {
            get
            {
                return _fullscreen;
            }
            set
            {
                if(_fullscreen == value)
                {
                    return;
                }
                _fullscreen = value;
                if(_game != null)
                    _game.Log.Write("Fullscreen set to " + _fullscreen.ToString());
                if(_game != null)
                {
                    _game.Graphics.IsFullScreen = _fullscreen;
                    _game.Graphics.ApplyChanges();
                    SerializeSettings();
                    if(_game.Interface != null)
                    {
                        _game.Interface.ReloadInterface();
                    }
                }
            }
        }

        [MoonSharpHidden]
        public void ApplySettings()
        {
            if(_game != null)
            {
                _game.IsFixedTimeStep = _vsync;
                _game.IsMouseVisible = _showMouse;
                _game.Graphics.PreferredBackBufferHeight = _height;
                _game.Graphics.PreferredBackBufferWidth = _width;    
                _game.Graphics.IsFullScreen = _fullscreen;
                _game.Graphics.ApplyChanges();    
                CenterScreen();
                if(_game.Interface != null)
                {
                    _game.Interface.ReloadInterface();
                }
            }
        }
                              
        [MoonSharpHidden]
        private void SerializeSettings()
        {
            if(_game == null)
            {
                return;
            }
            _game.Files.SaveSettings();
        }

        [MoonSharpHidden]
        public int CenterScreen()
        {
            if(_game == null)
            {
                return -1;
            }
            System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            _game.Window.Position = new Point((resolution.Width - _width) / 2, (resolution.Height - _height) / 2);
            return 1;
        }
    }
}
