using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using d4lilah.Data;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace d4lilah
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        FileManager _fileManager;
        EntityManager _entityManager;
        InputManager _inputManager;
        d4lilah.Debug.LogManager _logManager;
        LevelManager _levelManager;
        ScriptPipeline _scriptPipeline;
        InterfaceManager _interfaceManager;
        CameraManager _cameraManager;
        d4lilah.Debug.ConsoleManager _consoleManager;
        ParticleEffectsManager _particlesManager;
        MemoryManager _memoryManager;
        Performance _performance;
        GameTime _gameTime;
        DisplayMode[] _availableResolutions;
        Rectangle _nativeResolution;
        Effect _selectedGameShader;
        Video _video;
        AudioManager _audioManager;
        LevelEditor _editor;
        SpriteManager _spriteManager;
        DecalManager _decalManager;
        Time _time;
        string _contentLocation = "";
        bool _takeScreenshot = false;

        TimeSpan _lastCPUTime;
        TimeSpan _startCPU;
        DateTime _startTime;

        public VERSION VERSION = new VERSION();


        private int _frameCounter = 0;
        private DateTime _frameCounterDate;

        private Color _clearColor = new Color(29f/255f, 30f/255f, 42f/255f);

        private bool _altEnterDown = false;

        public DecalManager Decals
        {
            get
            {
                return _decalManager;
            }
        }

        public SpriteManager Sprites
        {
            get
            {
                return _spriteManager;
            }
        }

        public Time Time
        {
            get
            {
                return _time;
            }
        }

        public FileManager Files
        {
            get
            {
                return _fileManager;
            }
        }

        public Effect GameShader
        {
            get
            {
                return _selectedGameShader;
            }
            set
            {
                _selectedGameShader = value;
            }
        }

        public EntityManager Entities
        {
            get
            {
                return _entityManager;
            }
        }

        public SpriteBatch SpriteBatch
        {
            get
            {
                return _spriteBatch;
            }
        }

        public MemoryManager MemoryManager
        {
            get
            {
                return _memoryManager;
            }
        }

        public InputManager Input
        {
            get
            {
                return _inputManager;
            }
        }

        public AudioManager Audio
        {
            get
            {
                return _audioManager;
            }
        }

        public d4lilah.Debug.LogManager Log
        {
            get
            {
                return _logManager;
            }
        }

        public LevelManager Levels
        {
            get
            {
                return _levelManager;
            }
        }

        public InterfaceManager Interface
        {
            get
            {
                return _interfaceManager;
            }
            set
            {
                _interfaceManager = value;
            }
        }

        public CameraManager Camera
        {
            get
            {
                return _cameraManager;
            }
        }

        public GameTime GameTime
        {
            get
            {
                return _gameTime;
            }
        }

        public ScriptPipeline Pipeline
        {
            get
            {
                return _scriptPipeline;
            }
        }

        public d4lilah.Debug.ConsoleManager Console
        {
            get
            {
                return _consoleManager;
            }
        }

        public GameSettings ClientSettings
        {
            get
            {
                return _fileManager.ClientSettings;
            }
        }
        public Performance Performance
        {
            get
            {
                return _performance;
            }
        }

        public ParticleEffectsManager ParticleEffects
        {
            get
            {
                return _particlesManager;
            }
        }

        public GraphicsDeviceManager Graphics
        {
            get
            {
                return _graphics;
            }
        }

        public DisplayMode[] Resolutions
        {
            get
            {
                return _availableResolutions;
            }
        }

        public Rectangle NativeResolution
        {
            get
            {
                return _nativeResolution;
            }
        }

        public string ContentLocation
        {
            get
            {
                return _contentLocation;
            }
        }

        public Video Video
        {
            get
            {
                return _video;
            }
        }

        public LevelEditor Editor
        {
            get
            {
                return _editor;
            }
        }

        public int TakeScreenshot()
        {
            _takeScreenshot = true;
            return 1;
        }


        public Game1()
        {
            _time = new Time(this);
            Content.RootDirectory = "Internal";
            _contentLocation = Environment.CurrentDirectory + @"\" + Content.RootDirectory + @"\";
            _graphics = new GraphicsDeviceManager(this);
            Window.Title = "SHATTERSHOT";
            _fileManager = new FileManager(this);
            _logManager = new d4lilah.Debug.LogManager(this);
            _memoryManager = new MemoryManager(this);
            _nativeResolution = new Rectangle(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            List<DisplayMode> res = new List<DisplayMode>();
            foreach(DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                res.Add(mode);
            }
            _availableResolutions = res.ToArray();
            if(_fileManager.ClientSettings.Width == 0 && _fileManager.ClientSettings.Height == 0)
            {
                _logManager.Write("First time setup!");
                _logManager.Write("Calibrating settings.");
                System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                for(int i = _availableResolutions.Length - 1; i >= 0; i--)
                {
                    if(_availableResolutions[i].Width <= resolution.Width && _availableResolutions[i].Height <= resolution.Height)
                    {
                        _fileManager.ClientSettings.Width = _availableResolutions[i].Width;
                        _fileManager.ClientSettings.Height = _availableResolutions[i].Height;
                        break;
                    }
                }
            }
            _video = new Video(this);
        }


        protected override void Initialize()
        {
            _spriteManager = new SpriteManager(this);
            for(int i = 0; i < _fileManager.AllScripts.Length; i++)
            {
                _logManager.Write(Debug.ConsoleColorCoding.Variable + _fileManager.AllScripts[i].Name + Debug.ConsoleColorCoding.Normal + "[" + Debug.ConsoleColorCoding.Numeric + _fileManager.AllScripts[i].MD5 + Debug.ConsoleColorCoding.Normal + "]");
            }
            _levelManager = new LevelManager(this);
            _editor = new LevelEditor(this);
            _scriptPipeline = new ScriptPipeline(this);
            _cameraManager = new CameraManager(this);
            _consoleManager = new d4lilah.Debug.ConsoleManager(this);
            _particlesManager = new ParticleEffectsManager(this);
            _decalManager = new DecalManager(this);
            _performance = new Performance();
            Script.DefaultOptions.DebugPrint = s => Log.Write(s.ToLower());
            _inputManager = new InputManager(this);
            _audioManager = new AudioManager(this);
            _entityManager = new EntityManager(this);
            _interfaceManager = new InterfaceManager(this);
            base.Initialize();
            _frameCounterDate = DateTime.Now.AddSeconds(1);
            Camera.Position = new Vector2(0, 0);
            _logManager.Write("ClientSettings.json");
            _consoleManager.Execute("ClientSettings");
            _logManager.Write("Keybinds.json");

            _logManager.Write("Scripts loaded!");
            _logManager.Write(Debug.ConsoleColorCoding.Success + "d4lilah Initialized!");
            _logManager.Write("Created by " + Debug.ConsoleColorCoding.String + "d4!");
            _logManager.Write("Version: " + Debug.ConsoleColorCoding.Numeric + VERSION.d4lilah + Debug.ConsoleColorCoding.Normal + "[" + Debug.ConsoleColorCoding.Numeric + VERSION.Commit + Debug.ConsoleColorCoding.Normal + "]");
            _logManager.Write("d4lilah is running LUA " + Debug.ConsoleColorCoding.Numeric + Script.LUA_VERSION);
            _logManager.Write("===========================");
            if(_fileManager.Mod == "")
            {
                _logManager.Write(Debug.ConsoleColorCoding.Warning + "No mod specified. Loaded everything!");
            }
            else
            {
                _logManager.Write(Debug.ConsoleColorCoding.Success + "Loaded Mod: " + _fileManager.Mod);
            }
            _logManager.Write("Loaded " + Debug.ConsoleColorCoding.Numeric + _fileManager.AllScripts.Length + Debug.ConsoleColorCoding.Normal + " scripts");
            _logManager.Write(Debug.ConsoleColorCoding.Numeric + _levelManager.Levels.Length + Debug.ConsoleColorCoding.Normal + " levels");
            _logManager.Write(Debug.ConsoleColorCoding.Numeric + _entityManager.Entities.Length + Debug.ConsoleColorCoding.Normal + " entities");
            _logManager.Write(Debug.ConsoleColorCoding.Numeric + _interfaceManager.RegisteredWidgets.Length + Debug.ConsoleColorCoding.Normal + " widgets");
            _logManager.Write(Debug.ConsoleColorCoding.Numeric + _fileManager.NonDefinedScripts.Length + Debug.ConsoleColorCoding.Normal + " nondefined scripts");

            _lastCPUTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
            _startCPU = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
            _startTime = DateTime.Now;
        }


        protected override void LoadContent()
        {                                                     
            _spriteBatch = new SpriteBatch(GraphicsDevice);
                                                                      
        }
         
        protected override void UnloadContent()
        {

        }                                                                           

        protected override void Update(GameTime gameTime)
        {
            _time.Update();
            KeyboardState keyb = Keyboard.GetState();
            if((keyb.IsKeyDown(Keys.LeftAlt) || keyb.IsKeyDown(Keys.RightAlt)) && keyb.IsKeyDown(Keys.Enter))
            {
                if(!_altEnterDown)
                {
                    _fileManager.ClientSettings.Fullscreen = !_fileManager.ClientSettings.Fullscreen;
                }
                _altEnterDown = true;
            }
            else
            {
                _altEnterDown = false;
            }
            if(_inputManager.KeyPressed("screenshot"))
            {
                _takeScreenshot = true;
            }    
            _performance.DeltaTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            _gameTime = gameTime;
            _logManager.Update();
            _inputManager.Update();
            _levelManager.Update();
            _entityManager.Update();
            _interfaceManager.Update();
            _consoleManager.Update();
            _fileManager.Update();
            _particlesManager.Update();
            _decalManager.Update();
            _audioManager.Update();
            _performance.AllocatedByteCount = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
            _performance.ProcessTimePercentage = (int)((System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime - _startCPU).TotalSeconds / (DateTime.Now.Subtract(_startTime).TotalSeconds) * 100);
            _lastCPUTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
            _performance.ProcessorTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds;

            base.Update(gameTime);
        }
                                                                                 
        protected override void Draw(GameTime gameTime)
        {
            if(DateTime.Now >= _frameCounterDate)
            {
                _performance.FPS = _frameCounter;
                _frameCounter = 0;
                _frameCounterDate = DateTime.Now.AddSeconds(1);
            }
            _frameCounter++;

            if(_levelManager.CurrentLevel != null)
            {
                GraphicsDevice.Clear(_levelManager.CurrentLevel.HiddenBackColor);
            }
            else
            {
                GraphicsDevice.Clear(_clearColor);
            }
            
            
            if(!_levelManager.ChangingLevel)
            {                                                                                                                                                  
                if(_takeScreenshot)
                {
                    _takeScreenshot = false;
                    RenderTarget2D screenshot = new RenderTarget2D(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, false, SurfaceFormat.Bgra32, DepthFormat.None);
                    GraphicsDevice.SetRenderTarget(screenshot);
                    GraphicsDevice.Clear(_levelManager.CurrentLevel.HiddenBackColor);
                    _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
                           null, null, null, _selectedGameShader, _cameraManager.GetTransform() * Matrix.CreateScale(_cameraManager.GetScreenScale()));

                    _entityManager.Draw();
                    _particlesManager.Draw();
                    _decalManager.Draw();
                    _spriteBatch.End();

                    _spriteBatch.Begin(SpriteSortMode.FrontToBack);
                    _interfaceManager.Draw();
                    _spriteBatch.End();
                    GraphicsDevice.Present();

                    GraphicsDevice.SetRenderTarget(null);
                    byte[] textureData = new byte[screenshot.Width * screenshot.Height * 4];
                    screenshot.GetData(textureData, 0, screenshot.Width * screenshot.Height * 4);
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(screenshot.Width, screenshot.Height);
                    int x = 0, y = 0;
                    for(int i = 0; i < screenshot.Width * screenshot.Height * 4; i += 4)
                    {
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb((int)textureData[i + 3], (int)textureData[i + 2], (int)textureData[i + 1], (int)textureData[i + 0]));
                        x++;
                        if(x >= screenshot.Width)
                        {
                            x = 0;
                            y++;
                        }
                    }
                    string filename = (DateTime.Now.ToString().Replace('\\', '_').Replace('/', '_').Replace(':', '_').Replace(' ', '_')) + ".png";
                    string pathname = _contentLocation + @"Screenshots\";
                    int counter = 0;
                    while(System.IO.File.Exists(pathname + filename))
                    {
                        filename = (DateTime.Now.ToString().Replace('\\', '_').Replace('/', '_').Replace(':', '_').Replace(' ', '_')) + "_" + counter + "_.png";
                        counter++;
                    }
                    bitmap.Save(pathname + filename, System.Drawing.Imaging.ImageFormat.Png);
                    _logManager.Write("#24FE00Saving screenshot #0025FE" + filename);
                }

                _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
                           null, null, null, _selectedGameShader, _cameraManager.GetTransform() * Matrix.CreateScale(_cameraManager.GetScreenScale()));
                
                _entityManager.Draw();
                _particlesManager.Draw();
                _decalManager.Draw();
                _spriteBatch.End();

                _spriteBatch.Begin(SpriteSortMode.FrontToBack);
                _interfaceManager.Draw();
                _spriteBatch.End();

                if(_takeScreenshot)
                {
                    
                }

            }
            else
            {
                float stage = 1;
                float stageMax = 6;
                string loadingInfo = "Cleaning Up Level...";
               
                if(_levelManager.LevelCleanedUp && !_levelManager.LevelPrepared)
                {
                    stage++;
                    loadingInfo = "Preparing Level...";
                }
                if(_levelManager.LevelPrepared && !_levelManager.LevelInitiated)
                {
                    stage++;
                    loadingInfo = "Initiating Level...";
                }
                if(_levelManager.LevelInitiated && !_levelManager.QuadInitiated)
                {
                    stage++;
                    loadingInfo = "Loading Quad...";
                }
                if(_levelManager.QuadInitiated && !_levelManager.Widgetsinitiated)
                {
                    stage++;
                    loadingInfo = "Initiating Widgets...";
                }
                if(_levelManager.Widgetsinitiated)
                {
                    stage++;
                    loadingInfo = "Starting level...";
                }    
                Log.Write("Loading level (" + _levelManager.NextLevel.Name + ")... " + loadingInfo);
                _spriteBatch.Begin();
                SpriteFont font = Sprites.GetFont("Fonts/ConsoleFont");
                string text = "Loading " + _levelManager.NextLevel.Name;
                Texture2D whitebox = Sprites.GetSprite("Sprites/core/white");
                _spriteBatch.DrawString(font, text, new Vector2((_fileManager.ClientSettings.Width - font.MeasureString(text).X) / 2, (_fileManager.ClientSettings.Height - font.MeasureString(text).Y) * 0.55f), Color.White);

                _spriteBatch.Draw(
                    whitebox,
                    new Rectangle(
                        (int)((_fileManager.ClientSettings.Width - font.MeasureString(text).X) * 0.2),
                        (int)((_fileManager.ClientSettings.Height - font.MeasureString(text).Y) * 0.6),
                        (int)((_fileManager.ClientSettings.Width - font.MeasureString(text).X) * 0.8), 20),
                    Color.White * 0.1f
                );
                _spriteBatch.Draw(
                    whitebox,
                    new Rectangle(
                        (int)((_fileManager.ClientSettings.Width - font.MeasureString(text).X) * 0.2),
                        (int)((_fileManager.ClientSettings.Height - font.MeasureString(text).Y) * 0.6),
                        (int)(((_fileManager.ClientSettings.Width - font.MeasureString(text).X) * 0.8) * (float)(stage / stageMax)),
                        20),
                    Color.White
                );                                        
                _spriteBatch.DrawString(font, loadingInfo, new Vector2((_fileManager.ClientSettings.Width - font.MeasureString(loadingInfo).X) / 2, (float)((_fileManager.ClientSettings.Height - font.MeasureString(text).Y) * 0.6) + 30), Color.White);
                _spriteBatch.End();
                
                //LOADING SCREEN LOGIC HERE
            }
            _spriteBatch.Begin();
            _consoleManager.Draw();
            _spriteBatch.End();
            _performance.DrawCount = (ulong)_graphics.GraphicsDevice.Metrics.DrawCount;
            _performance.SpriteCount = (ulong)_graphics.GraphicsDevice.Metrics.SpriteCount;  
            base.Draw(gameTime);
        }    

        protected override void Dispose(bool disposing)
        {
            _logManager.Dispose();
            base.Dispose(disposing);
        }            
    }
}
