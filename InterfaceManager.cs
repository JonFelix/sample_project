using d4lilah.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using Newtonsoft.Json;
using Svg;
using System;
using System.Collections.Generic;  
using System.IO;
using System.Windows.Forms;

namespace d4lilah
{
    public class InterfaceManager
    {
        private Game1 _game;
        private ScriptInfo[] _scripts;
        private List<InterfaceWidget> _registeredWidgets = new List<InterfaceWidget>();
        private List<InterfaceWidget> _activeWidgets = new List<InterfaceWidget>();

        private bool _toggleInterface = true;

        public string TextInput = "";
                               
        public InterfaceWidget[] ActiveWidgets
        {
            get
            {
                return _activeWidgets.ToArray();
            }
        }

        public ShellVector GetMousePosition
        {
            get
            {
                ShellVector mousepos;
                if(!_game.Input.IsReplay)
                {
                    mousepos = new ShellVector(Mouse.GetState().Position.ToVector2());
                }
                else
                {
                    mousepos = new ShellVector(new Vector2((float)_game.Input.ReplayFrame.MouseX, (float)_game.Input.ReplayFrame.MouseY));
                }
                if(_game.Graphics.IsFullScreen)
                {
                    mousepos.X *= (float)_game.Graphics.PreferredBackBufferWidth / (float)_game.NativeResolution.Width;
                    mousepos.Y *= (float)_game.Graphics.PreferredBackBufferHeight / (float)_game.NativeResolution.Height;
                }
                return mousepos;
            }
        }

        public InterfaceWidget[] RegisteredWidgets
        {
            get
            {
                return _registeredWidgets.ToArray();
            }
        }

        public InterfaceManager(Game1 game)
        {
            _game = game;
            _scripts = _game.Files.WidgetScripts;
            RegisterWidgets();
            _game.Window.TextInput += Input;
        }

        public void Update()
        {
            if(_game.Input.KeyPressed("showinterface"))
            {
                _toggleInterface = !_toggleInterface;
            }
            for(int i = 0; i < _activeWidgets.Count; i++)
            {
                if(_activeWidgets[i].UpdateCheck < _activeWidgets[i].ScriptInfo.Updated)
                {
                    _activeWidgets[i].UpdateScript();
                }
                _activeWidgets[i].BakeScript(_game);
                if(!_activeWidgets[i].IsInitialized)
                {
                    if(_activeWidgets[i].Script.Globals["Initialize"] != null)
                    {
                        try
                        {
                            _activeWidgets[i].Script.Call(_activeWidgets[i].Script.Globals["Initialize"]);
                        }
                        catch(Exception e)
                        {
                            _game.Log.Write(_activeWidgets[i].Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                        }
                    }
                    _activeWidgets[i].IsInitialized = true;
                }
                if(_activeWidgets[i].Script.Globals["OnMessage"] != null)
                {
                    for(int x = 0; x < _activeWidgets[i].Timers.Count; x++)
                    {
                        if(_activeWidgets[i].Timers[x] <= DateTime.Now)
                        {
                            _activeWidgets[i].MessageQueue.Add(_activeWidgets[i].TimerPayloads[x]);
                            _activeWidgets[i].Timers.RemoveAt(x);
                            _activeWidgets[i].TimerPayloads.RemoveAt(x);
                        }
                    }
                    for(int x = 0; x < _activeWidgets[i].MessageQueue.Count; x++)
                    {
                        try
                        {
                            _activeWidgets[i].Script.Call(_activeWidgets[i].Script.Globals["OnMessage"], _activeWidgets[i].MessageQueue[x]);
                        }
                        catch(Exception e)
                        {
                            _game.Log.Write(_activeWidgets[i].Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                        }
                    }
                    _activeWidgets[i].MessageQueue.Clear();
                }
                if(_activeWidgets[i].Script.Globals["Update"] != null)
                {
                    try
                    {
                        _activeWidgets[i].Script.Call(_activeWidgets[i].Script.Globals["Update"]);
                    }
                    catch(Exception e)
                    {
                        _game.Log.Write(_activeWidgets[i].Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                    }
                }
            }
        }

        public void Draw()
        {
            if(_toggleInterface)
            {    
                for(int i = 0; i < _activeWidgets.Count; i++)
                {                                        
                    if(_activeWidgets[i].IsInitialized)
                    {
                        if(_activeWidgets[i].Script.Globals["Draw"] != null)
                        {
                            try
                            {
                                _activeWidgets[i].Script.Call(_activeWidgets[i].Script.Globals["Draw"]);
                            }
                            catch(Exception e)
                            {
                                _game.Log.Write(_activeWidgets[i].Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                            }
                        }
                    }
                }
            }
        }

        private void Input(object sender, TextInputEventArgs e)
        {
            
            char charInput = e.Character;

            if(charInput == '\b')
            {
                
            }    
            else if(charInput == '\t')
            {

            }
            else if(charInput == '\u001b') // '\u001b' == escape
            {

            }
            else if(charInput == '\u0016') // '\u0016' is Ctrl+V. copyPasta
            {
                if(Clipboard.ContainsText())
                {
                    string clipboard = Clipboard.GetText();
                    TextInput +=  clipboard;                                    
                }
            }
            else if(charInput == '\u0003') // '\u0003' is Ctrl+C
            {

            }
            else if(charInput == '\u0018') // '\u0018' is Ctrl+X
            {

            }
            else if(charInput == '\u0001') // '\u0001' is Ctrl+A
            {

            }
            else if(charInput == '§' || charInput == '\t')
            {
            }     
            else
            {
                TextInput += charInput;
            }
        }

        private void RegisterWidgets()
        {
            for(int i = 0; i < _scripts.Length; i++)
            {
                if(_scripts[i] != null)
                {
                    Script script = new Script();
                    ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = _game.Files.Folders;
                    try
                    {
                        script.DoString(_scripts[i].RawScript);
                    }
                    catch(Exception e)
                    {
                        _game.Log.Write(_scripts[i].Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                        continue;
                    }
                    if(script.Globals["RegisterWidget"] != null)
                    {
                        DynValue settings = null;
                        try
                        {      
                            settings = script.Call(script.Globals["RegisterWidget"]);
                            if(settings.Type == DataType.Table)
                            {
                                InterfaceWidget widget = new InterfaceWidget();
                                widget.Name = _scripts[i].Name;
                                widget.RawScript = _scripts[i].RawScript;
                                widget.ScriptInfo = _scripts[i];
                                widget.UpdateCheck = _scripts[i].Updated;
                                foreach(TablePair pair in settings.Table.Pairs)
                                {
                                    if(pair.Key.String == "Position")
                                    {
                                        widget.Position.X = (float)(pair.Value.Table.Get("X")).Number;
                                        widget.Position.Y = (float)(pair.Value.Table.Get("Y")).Number;
                                    }
                                    if(pair.Key.String == "MenuWidget")
                                    {
                                        widget.IsMenuWidget = (pair.Value.Boolean);
                                    }
                                    if(pair.Key.String == "Origin")
                                    {
                                        widget.Origin.X = (float)(pair.Value.Table.Get("X")).Number;
                                        widget.Origin.Y = (float)(pair.Value.Table.Get("Y")).Number;
                                    }
                                    if(pair.Key.String == "Depth")
                                    {
                                        widget.Depth = (int)(pair.Value.Number);
                                    }
                                    if(pair.Key.String == "Level")
                                    {
                                        widget.LevelSpecific = (pair.Value.String);
                                    }
                                }
                                _registeredWidgets.Add(widget);
                            }
                            else
                            {
                                _game.Log.Write("Widget " + _scripts[i].Name + " did not register correctly: Return value was not recognized as a table.");
                                _scripts[i] = null;
                            }
                        }
                        catch(Exception e)
                        {
                            _game.Log.Write(_scripts[i].Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                        }
                    }
                    else
                    {
                        _game.Log.Write("Widget " + _scripts[i].Name + " did not register correctly: No RegisterWidget function.");
                        _scripts[i] = null;
                    }
                }
            }
        }

        public void CleanUpWidgets()
        {
            _activeWidgets.Clear();
        }


        public void InitializeWidgets()
        {     
            for(int i = 0; i < _registeredWidgets.Count; i++)
            {
                if(_registeredWidgets[i].IsMenuWidget == _game.Levels.CurrentLevel.IsMenuLevel)
                {
                    if(_registeredWidgets[i].LevelSpecific != "")
                    {
                        if(_registeredWidgets[i].LevelSpecific != _game.Levels.CurrentLevel.Name)
                        {
                            continue;
                        }
                    }
                    _activeWidgets.Add(_registeredWidgets[i]);
                    _activeWidgets[_activeWidgets.Count - 1].Script = new Script();
                    _activeWidgets[_activeWidgets.Count - 1].InitScript(_game);  
                    _activeWidgets[_activeWidgets.Count - 1].IsInitialized = false;
                    try
                    {
                        _activeWidgets[_activeWidgets.Count - 1].Script.DoString(_registeredWidgets[i].RawScript);
                    }
                    catch(Exception e)
                    {
                        _game.Log.Write(_scripts[i].Name + ": " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                        _activeWidgets.RemoveAt(i);
                    }
                }
            }
        }         

        public int ReloadInterface()
        {
            _game.Interface = new InterfaceManager(_game);
            _game.Interface.InitializeWidgets();
            return 1;
        }
    }

    public class InterfaceFunctions
    {
        private Game1 _game;
        private InterfaceWidget _parent;
        private Texture2D _pixel;   


        public InterfaceFunctions(Game1 game, InterfaceWidget parent)
        {
            _game = game;
            _parent = parent;
            _pixel = _game.Sprites.GetSprite("Sprites/Core/White");
        }

        public int DrawText(string font, string text, Table position, Table tcolor, float depth = 0)
        {
            if(File.Exists(_game.ContentLocation + font + ".xnb"))
            {
                SpriteFont sfont = _game.Sprites.GetFont(font);
                if(sfont == null)
                {
                    _game.Log.Write(font + " could not be found.");
                }
                string[] output = text.Split('\b');
                float textHeight = sfont.MeasureString(" ").Y;
                Vector2 pos = TranslateVector(position);
                Color col = TranslateColor(tcolor);
                if(col == null)
                {
                    _game.Log.Write("Color was not recognized.");
                }
                foreach(string i in output)
                {
                    _game.SpriteBatch.DrawString(sfont, i, pos, col, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
                    pos.Y += textHeight;
                }

                return 1;
            }
            else
            {
                _game.Log.Write("Font '" + _game.ContentLocation.Replace('\\', '/') + font.Replace('\\', '/') + "' was not found!");
                return -1;
            }
           
        }

        public int DrawRect(Table rect, Table color, float depth = 0)
        {
            _game.SpriteBatch.Draw(_pixel, TranslateRect(rect), null, TranslateColor(color), 0, Vector2.Zero, SpriteEffects.None, depth);
            return 1;
        }

        public int DrawLine(Table pos1, Table pos2, Table color, int width = 1, float depth = 0)
        {
            Vector2 p1 = TranslateVector(pos1);
            Vector2 p2 = TranslateVector(pos2);
            Rectangle r = new Rectangle((int)p1.X, (int)p1.Y, (int)(p2 - p1).Length() + width, width);
            Vector2 v = Vector2.Normalize(p1 - p2);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if(p1.Y > p2.Y)
                angle = MathHelper.TwoPi - angle;

            _game.SpriteBatch.Draw(_game.Sprites.GetSprite("Sprites/Core/white"), r, null, TranslateColor(color), angle, Vector2.Zero, SpriteEffects.None, 0);
            return 1;
        }
                           
        public ShellVector TextSize(string font, string text)
        {
            if(File.Exists(_game.ContentLocation + font + ".xnb"))
            {

                SpriteFont sfont = _game.Sprites.GetFont(font);
                if(sfont == null)
                {
                    _game.Log.Write(font + " could not be found.");
                }
                return new ShellVector(sfont.MeasureString(text));
            }
            else
            {
                _game.Log.Write("Font '" + _game.ContentLocation.Replace('\\', '/') + font.Replace('\\', '/') + "' was not found!");
                return new ShellVector(Vector2.Zero);
            }
        }

        public ShellVector TextureSize(string text)
        {
            Texture2D texture = _game.Sprites.GetSprite(text);
                if(texture != null)
                {
                    return new ShellVector(new Vector2(texture.Width, texture.Height));
                }
                return new ShellVector(Vector2.Zero);  
        }

        public int DrawTexture(string text, Table position, Table color)
        {
            Texture2D texture = _game.Sprites.GetSprite(text);
            if(texture != null)
            {
                _game.SpriteBatch.Draw(texture, TranslateVector(position), null, TranslateColor(color));
                return 1;
            }
            return -1;
        }

        private Vector2 TranslateVector(Table position)
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
            return new Vector2(
                (_game.ClientSettings.Width * _parent.Position.X) + (_parent.Origin.X) + x,
                (_game.ClientSettings.Height * _parent.Position.Y) + (_parent.Origin.Y) + y
           );
        }

        private Color TranslateColor(Table table)
        {
            int r = 0, g = 0, b = 0, a = 0;
            foreach(TablePair pair in table.Pairs)
            {
                if(pair.Key.String.ToLower() == "r" || pair.Key.String.ToLower() == "red")
                {
                    r = Math.Min(Math.Max((int)pair.Value.Number, 0), 255);
                }
                if(pair.Key.String.ToLower() == "g" || pair.Key.String.ToLower() == "green")
                {
                    g = Math.Min(Math.Max((int)pair.Value.Number, 0), 255);
                }
                if(pair.Key.String.ToLower() == "b" || pair.Key.String.ToLower() == "blue")
                {
                    b = Math.Min(Math.Max((int)pair.Value.Number, 0), 255);
                }
                if(pair.Key.String.ToLower() == "a" || pair.Key.String.ToLower() == "alpha")
                {
                    a = Math.Min(Math.Max((int)pair.Value.Number, 0), 255);
                }
            }
            Color col = new Color(r / 255f, g / 255f, b / 255f);
            col *= a / 255f;
            return col;
        }

        private Rectangle TranslateRect(Table rect)
        {
            float x = 0, y = 0, w = 0, h = 0;
            foreach(TablePair pair in rect.Pairs)
            {
                if(pair.Key.String.ToLower() == "x")
                {
                    x = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "y")
                {
                    y = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "w" || pair.Key.String.ToLower() == "width")
                {
                    w = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "h" || pair.Key.String.ToLower() == "height")
                {
                    h = (float)pair.Value.Number;
                }
            }
            Vector2 pos = TranslateVector(rect);
            return new Rectangle((int)((_game.ClientSettings.Width * _parent.Position.X) + (_parent.Origin.X) + x), (int)((_game.ClientSettings.Height * _parent.Position.Y) +(_parent.Origin.Y) + y), (int)w, (int)h);
        }

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

        public bool MouseInRect(Table rect)
        {
            Rectangle rect1 = TranslateRect(rect);
            Vector2 mousepos;      
            if(!_game.Input.IsReplay)
            {
                mousepos = Mouse.GetState().Position.ToVector2();
            }
            else
            {
                mousepos = new Vector2((float)_game.Input.ReplayFrame.MouseX, (float)_game.Input.ReplayFrame.MouseY);
            }
            if(_game.Graphics.IsFullScreen)
            {
                mousepos.X *= (float)_game.Graphics.PreferredBackBufferWidth / (float)_game.NativeResolution.Width;
                mousepos.Y *= (float)_game.Graphics.PreferredBackBufferHeight / (float)_game.NativeResolution.Height;    
            }
            if(rect1.Left < mousepos.X && rect1.Right > mousepos.X && rect1.Top < mousepos.Y && rect1.Bottom > mousepos.Y)
            {
                return true;
            }         
            return false;
        }
                
        public int SetVariable(string name, Table table)
        {
            if(!File.Exists(_game.ContentLocation + @"\Configuration\Scripts\" + _game.Files.Mod + "_" + _parent.Name + ".json"))
            {
                if(!Directory.Exists(_game.ContentLocation + @"\Configuration\Scripts\"))
                {
                    Directory.CreateDirectory(_game.ContentLocation + @"\Configuration\Scripts\");
                }
                File.Create(_game.ContentLocation + @"\Configuration\Scripts\" + _game.Files.Mod + "_" + _parent.Name + ".json");
                _game.Log.Write("Creating settings file for " + _parent.Name);
            }
            if(!_parent.Config.ContainsKey(name))
            {
                _parent.Config.Add(name, JsonConvert.DeserializeObject(MoonSharp.Interpreter.Serialization.Json.JsonTableConverter.TableToJson(table)));
            }
            else
            {
                _parent.Config[name] = JsonConvert.DeserializeObject(MoonSharp.Interpreter.Serialization.Json.JsonTableConverter.TableToJson(table));
            }
            string json = JsonConvert.SerializeObject(_parent.Config, Formatting.Indented);
            File.WriteAllText(_game.ContentLocation + @"\Configuration\Scripts\" + _game.Files.Mod + "_" + _parent.Name + ".json", json);
            return 1;
        }

        public Table GetVariable(string name)
        {
            if(_parent.Config.ContainsKey(name))
            {
                string json = JsonConvert.SerializeObject(_parent.Config[name]);
                return MoonSharp.Interpreter.Serialization.Json.JsonTableConverter.JsonToTable(json);
            }
            else
            {
                _game.Log.Write("'" + name + "' was not found.");
                return null;
            }
        }

        public int LoadSVG(string name, float scale = 1)
        {
            string info = _game.Sprites.GetSVG(name);
            if(info == null)
            {
                return -1;
            }
            SvgDocument svgDocument = SvgDocument.Open(info);
            svgDocument.Height = (int)((128) * scale);
            svgDocument.Width = (int)((128) * scale);
            svgDocument.ShapeRendering = SvgShapeRendering.CrispEdges; 
            _parent.ScaledSVGs.Add(GetTexture(_game.GraphicsDevice, svgDocument.Draw()));
            _parent.SVGName.Add(name);
            return _parent.ScaledSVGs.Count - 1;
        }

        public int DrawSVG(int id, Table position, Table color)
        {
            if(id == -1)
            {
                _game.Log.Write("#D81733SVG was not properly loaded!");
                return -1;
            }                                                            
            _game.SpriteBatch.Draw(_parent.ScaledSVGs[id], TranslateVector(position), null, TranslateColor(color));
            return 1;                                   
        }

        public int ResizeSVG(int id, float scale)
        {
            if(id == -1)
            {
                _game.Log.Write("#D81733SVG was not properly loaded!");
                return -1;
            }  
            string info = _game.Sprites.GetSVG(_parent.SVGName[id]);
            if(info == null)
            {
                _game.Log.Write("#D81733SVG was not properly loaded!");
                return -1;
            }
            SvgDocument svgDocument = SvgDocument.Open(info);
            svgDocument.Height = (int)((128) * scale);
            svgDocument.Width = (int)((128) * scale);
            svgDocument.ShapeRendering = SvgShapeRendering.CrispEdges;   
            _parent.ScaledSVGs[id] = GetTexture(_game.GraphicsDevice, svgDocument.Draw());
            return 1;
        }

        private Texture2D GetTexture(GraphicsDevice dev, System.Drawing.Bitmap bmp)
        {
            int[] imgData = new int[bmp.Width * bmp.Height];
            Texture2D texture = new Texture2D(dev, bmp.Width, bmp.Height);

            unsafe
            {
                // lock bitmap
                System.Drawing.Imaging.BitmapData origdata =
                    bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                uint* byteData = (uint*)origdata.Scan0;

                // Switch bgra -> rgba
                for(int i = 0; i < imgData.Length; i++)
                {
                    byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
                }

                // copy data
                System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, bmp.Width * bmp.Height);

                byteData = null;

                // unlock bitmap
                bmp.UnlockBits(origdata);
            }

            texture.SetData(imgData);

            return texture;
        }

        public float Random(float min, float max)
        {
            return (float)Math.Max(_parent.Random.NextDouble() * max, min);
        }
    }

    public class ExposedConsoleInterfaceCommands
    {
        [JsonIgnoreAttribute]
        [MoonSharpHidden]
        private Game1 _game;



        [MoonSharpHidden]
        public ExposedConsoleInterfaceCommands(Game1 game)
        {
            _game = game;
        }


        public int Reload()
        {
            int res = _game.Interface.ReloadInterface();
            _game.Interface.InitializeWidgets();
            return res;
        }

        public Table GetWidgetSettings(string name)
        {
            if(name.Length < 11)
            {
                name = name + ".widget.lua";
            }
            else if(name.Substring(name.Length - 11, 11) != ".widget.lua")
            {
                name = name + ".widget.lua";
            }
            Table settings = new Table(null);
            for(int i = 0; i < _game.Interface.RegisteredWidgets.Length; i++)
            {
                if(_game.Interface.RegisteredWidgets[i].Name == name)
                {
                    settings.Set("IsMenuWidget", DynValue.NewBoolean(_game.Interface.RegisteredWidgets[i].IsMenuWidget));
                    settings.Set("Depth", DynValue.NewNumber(_game.Interface.RegisteredWidgets[i].Depth));
                    settings.Set("Level", DynValue.NewString(_game.Interface.RegisteredWidgets[i].LevelSpecific));
                    Table postable = new Table(null);
                    postable.Set("X", DynValue.NewNumber(_game.Interface.RegisteredWidgets[i].Position.X));
                    postable.Set("Y", DynValue.NewNumber(_game.Interface.RegisteredWidgets[i].Position.Y));
                    settings.Set("Position", DynValue.NewTable(postable));
                    Table origintable = new Table(null);
                    origintable.Set("X", DynValue.NewNumber(_game.Interface.RegisteredWidgets[i].Origin.X));
                    origintable.Set("Y", DynValue.NewNumber(_game.Interface.RegisteredWidgets[i].Origin.Y));
                    settings.Set("Origin", DynValue.NewTable(origintable));
                }
            }
            return settings;
        }

        public Table GetWidgets()
        {
            Table Widgets = new Table(null);
            for(int i = 0; i < _game.Interface.RegisteredWidgets.Length; i++)
            {

                Widgets.Set(_game.Interface.RegisteredWidgets[i].Name, DynValue.NewBoolean(_game.Interface.RegisteredWidgets[i].IsInitialized));

            }
            return Widgets;
        }   
    }
}
