using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using System.Windows.Forms;
using MoonSharp.Interpreter.Loaders;
using Microsoft.Xna.Framework.Input;
using System.IO;
using d4lilah.Data;            

namespace d4lilah.Debug
{
    public class ConsoleManager
    {
        private Game1 _game;
        private bool _consoleOpen = false;
        private Vector2 _consoleSize = new Vector2(0.9f, 0.7f);
        private Vector2 _consolePosition = new Vector2(0.05f, 0.15f);
        private Rectangle _consoleCurrentSize = Rectangle.Empty;
        private float _consoleOpenSpeed = 2000f;
        private float _consoleMaxHeight = 0f;
        private Texture2D _pixel;
        private SpriteFont _font;
        private string _fontName = "Fonts/ConsoleFont";
        private Texture2D _backImage;
        private Color _backColor = new Color(40f/255f, 44f/255f, 52f/255f)* 0.7f;//Color.Black * 0.3f;
        private Color _foreColor = new Color(171f/255f, 178f/255f, 177f/255f);//Color.White;
        private Color _borderColor = Color.White;
        private Color _userTextColor = new Color(152f/255f, 195f/255f, 121f/255f);
        private Color _userInputColor = Color.White;
        private Color _zoomBackgroundColor = Color.Black * 0.6f;
        private Color _zoomForegroundColor = Color.White;
        private int _screenRowCount = 0;
        private int _textHeight;
        private int _charWidth;
        private int _cursor = 0;
        private List<string> _displayedText = new List<string>();
        private int _displayedTextFloor = 0;
        private string _key = "console";
        private string _consoleInput = "";
        private Script _script = new Script();
        private int _textOffset = 5;
        private int _consoleInputLength = 0;
        private int _consoleInputCursor = 0;
        private List<string> _userHistory = new List<string>();
        private int _userHistoryIndex = 0;
        private bool _userKeyDownLastFrame = false;
        private bool _userKeyUpLastFrame = false;
        private bool _userKeyPageDownLastFrame = false;
        private bool _userKeyPageUpLastFrame = false;
        private bool _userKeyLeftLastFrame = false;
        private bool _userKeyRightLastFrame = false;
        private bool _userKeyDelLastFrame = false;
        private bool _userKeyShiftLastFrame = false;
        private bool _userKeyEndLastFrame = false;
        private bool _userKeyHomeLastFrame = false;
        private ExposedConsole _userFunctions;
        private ExposedConsoleInterfaceCommands _interfaceFunctions;
        private ExposedLevelFunctions _levelFunctions;
        private ExposedEntityFunctions _entityFunctions;
        private ExposedInputClass _inputFunctions;
        private bool _userIsBottomed = true;
        private int _userScrollSpeed = 1;
        private string _zoomedMessage = "";
        private Rectangle _zoomedBox;
        private Vector2 _zoomedBoxMouseOffset = new Vector2(16, 16);
        private string _consoleLogFile;
        private bool _mouseVisibleSetting = false;
        private List<int> _userInputIds = new List<int>();
        private string _userInputIndicator = ">>";
        private bool _cursorFlash = true;
        private float _cursorAlphaVal = 1f;
        private float _cursorFlashSpeed = 5f;
        private float _currentTimescale = 1.0f;
        private float _mouseScroll = 0f;
        private int _selectIndex = 0;
        private int _selectLength = 0;
        private Color _selectColor = Color.White * 0.5f;




        public ExposedConsole ExposedConsole
        {
            get
            {
                return _userFunctions;
            }
        }

        public string UserInputIndicator
        {
            get
            {
                return _userInputIndicator;
            }
        }

        public bool ConsoleOpen
        {
            get
            {
                return _consoleOpen;
            }
        }

        public int ConsoleLength
        {
            get
            {
                return _game.Log.LogStack.Length - _displayedTextFloor;
            }
        }

        public ConsoleManager(Game1 game)
        {
            _game = game;
            _consoleLogFile = Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/System/consolehistory.con";
            if(!File.Exists(_consoleLogFile))
            {
                (File.Create(_consoleLogFile)).Close();
            }
            if(_game.ClientSettings.ConsolePersistentHistory)
            {
                _userHistory.AddRange(File.ReadAllLines(_consoleLogFile));
            }
            File.WriteAllText(_consoleLogFile, String.Empty);
            _userFunctions = new ExposedConsole(_game);
            _interfaceFunctions = new ExposedConsoleInterfaceCommands(_game);
            _levelFunctions = new ExposedLevelFunctions(_game);
            _entityFunctions = new ExposedEntityFunctions(_game);
            _inputFunctions = new ExposedInputClass(_game);
            _consoleMaxHeight = _game.ClientSettings.Height * _consoleSize.Y;
            _consoleCurrentSize.Width = (int)(_game.ClientSettings.Width * _consoleSize.X);
            _consoleCurrentSize.X = (int)(_game.ClientSettings.Width * _consolePosition.X);
            _consoleCurrentSize.Y = (int)(_game.ClientSettings.Height * _consolePosition.Y) + (int)(_consoleMaxHeight / 2);
            _pixel = _game.Sprites.GetSprite("Sprites/core/white");
            _backImage = _game.Sprites.GetSprite("System/d4lilah_console_img");
            _font = _game.Sprites.GetFont(_fontName);
            _textHeight = (int)_font.MeasureString(" ").Y;
            _charWidth = (int)_font.MeasureString(" ").X;
            _screenRowCount = ((int)_consoleMaxHeight / _textHeight) - 1;
            _game.Window.TextInput += Input;
            UserData.RegisterType<ExposedConsoleInterfaceCommands>();
            UserData.RegisterType<GameSettings>();
            UserData.RegisterType<Performance>();
            UserData.RegisterType<ShellVector>();
            UserData.RegisterType<Rectangle>();
            UserData.RegisterType<ShellTexture>();
            UserData.RegisterType<Stats>();
            UserData.RegisterType<ExposedConsole>();
            UserData.RegisterType<ExposedLevelFunctions>();
            UserData.RegisterType<ExposedEntityFunctions>();
            UserData.RegisterType<ExposedInputClass>();
            UserData.RegisterType<Time>();
            UserData.RegisterType<Level>();
            UserData.RegisterType<VERSION>();
        }


        public void Update()
        {
            if(!_game.ClientSettings.ConsoleEnabled)
            {
                CloseConsole(0);
                return;
            }
            _consoleMaxHeight = _game.ClientSettings.Height * _consoleSize.Y;
            _screenRowCount = ((int)_consoleMaxHeight / _textHeight) - 1;
            if(_consoleOpen)
            {
                AnimateCursor();
                HandleUserInput();
                CalculatedZoomedText();
                RetrieveLines();
                AnimateWindow();
                _consoleInputLength = _consoleInput.Length;
            }
            else
            {
                CleanUpConsoleWindow();
            }
        }

        public void Draw()
        {
            if(!_game.ClientSettings.ConsoleEnabled)
            {
                return;
            }
            if(_consoleOpen)
            {
                _game.SpriteBatch.Draw(_pixel, _consoleCurrentSize, _backColor);
                if(_consoleCurrentSize.Height > _backImage.Height && _consoleCurrentSize.Width > _backImage.Width)
                {
                    _game.SpriteBatch.Draw(_backImage, new Vector2(_consoleCurrentSize.Right - _backImage.Width, _consoleCurrentSize.Top), Color.White);
                }
                int consoleHeight = _consoleCurrentSize.Bottom - (int)_consoleMaxHeight;
                for(int i = 0; i < _displayedText.Count; i++)
                {
                    if((consoleHeight + (i * _textHeight)) < _consoleCurrentSize.Top)
                    {
                        continue;
                    }
                    if(_displayedText[i].StartsWith(_userInputIndicator))
                    {
                        string drawString = "";
                        Color drawStringColor = _userTextColor;
                        int drawStringIndex = 0;
                        int WindowWidthInChar = _consoleCurrentSize.Width / _charWidth;   
                        for(int t = 0; t < (_displayedText[i].Length > WindowWidthInChar ? WindowWidthInChar : _displayedText[i].Length); t++)
                        {
                            if(_displayedText[i][t] == '#')
                            {
                                if(t + 6 <= (_displayedText[i].Length > WindowWidthInChar ? WindowWidthInChar : _displayedText[i].Length))
                                {
                                    _game.SpriteBatch.DrawString(_font, drawString, new Vector2(_consoleCurrentSize.X + _textOffset + (_charWidth * drawStringIndex), consoleHeight + (i * _textHeight)), drawStringColor);
                                    if(_game.ClientSettings.ConsoleColorCoding)
                                    {
                                        string hex = _displayedText[i].Substring(t + 1, 6);
                                        int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                                        int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                                        int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                                        drawStringColor = new Color(r / 255f, g / 255f, b / 255f);
                                    }
                                    _displayedText[i] = _displayedText[i].Remove(t, 7);
                                    drawStringIndex = t;
                                    drawString = "";
                                }
                            }
                            drawString += _displayedText[i][t];  
                        }
                        _game.SpriteBatch.DrawString(_font, drawString, new Vector2(_consoleCurrentSize.X + _textOffset + (_charWidth * drawStringIndex), consoleHeight + (i * _textHeight)), drawStringColor);
                    }
                    else
                    {
                        string drawString = "";
                        Color drawStringColor = _foreColor;
                        int drawStringIndex = 0;
                        int WindowWidthInChar = _consoleCurrentSize.Width / _charWidth; 
                        for(int t = 0; t < (_displayedText[i].Length > WindowWidthInChar ? WindowWidthInChar : _displayedText[i].Length); t++)
                        {
                            if(_displayedText[i][t] == '#')
                            {
                                if(t + 6 <= (_displayedText[i].Length > WindowWidthInChar ? WindowWidthInChar : _displayedText[i].Length))
                                {
                                    _game.SpriteBatch.DrawString(_font, drawString, new Vector2(_consoleCurrentSize.X + _textOffset + (_charWidth * drawStringIndex), consoleHeight + (i * _textHeight)), drawStringColor);
                                    if(_game.ClientSettings.ConsoleColorCoding)
                                    {
                                        string hex = _displayedText[i].Substring(t + 1, 6);
                                        int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                                        int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                                        int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                                        drawStringColor = new Color(r / 255f, g / 255f, b / 255f);
                                    }
                                    _displayedText[i] = _displayedText[i].Remove(t, 7);
                                    drawStringIndex = t;
                                    drawString = "";
                                }
                            }
                            drawString += _displayedText[i][t];
                        }           
                        _game.SpriteBatch.DrawString(_font, drawString, new Vector2(_consoleCurrentSize.X + _textOffset + (_charWidth * drawStringIndex), consoleHeight + (i * _textHeight)), drawStringColor);
                    }
                }
                _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Left, consoleHeight + (_screenRowCount * _textHeight), _consoleCurrentSize.Width, 1), _borderColor);
                _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Left, _consoleCurrentSize.Bottom, _consoleCurrentSize.Width, 1), _borderColor);
                _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Left, _consoleCurrentSize.Top - 1, _consoleCurrentSize.Width, 1), _borderColor);

                _game.SpriteBatch.DrawString(_font, _consoleInput, new Vector2(_consoleCurrentSize.X + _textOffset, (consoleHeight + (_screenRowCount * _textHeight)) + (_textOffset * 2f)), _userInputColor);

                if(_selectLength != 0)
                {
                    int selectrange = Math.Abs(_selectLength);
                    int selectstart = Math.Min(_selectIndex, _selectIndex + _selectLength);
                    _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Left + _textOffset + (selectstart * _charWidth), consoleHeight + (_screenRowCount * _textHeight) + (_textOffset * 2), selectrange * _charWidth, _textHeight), _selectColor);
                }

                _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Left + _textOffset + (_consoleInputCursor * _charWidth), ((consoleHeight + (_screenRowCount * _textHeight)) + _textHeight) + 8, _charWidth, 2), _userInputColor * _cursorAlphaVal);

                _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Left - 1, _consoleCurrentSize.Top - 1, 1, _consoleCurrentSize.Height + 2), _borderColor);
                _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Right, _consoleCurrentSize.Top - 1, 1, _consoleCurrentSize.Height + 2), _borderColor);

                _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Right + 1, 5 + _consoleCurrentSize.Top, 4, _consoleCurrentSize.Height), Color.Black * 0.3f);
                _game.SpriteBatch.Draw(_pixel, new Rectangle(_consoleCurrentSize.Left + 4, _consoleCurrentSize.Bottom + 1, _consoleCurrentSize.Width - 3, 4), Color.Black * 0.3f);

                if(_zoomedMessage != "")
                {
                    _game.SpriteBatch.Draw(_pixel, _zoomedBox, _zoomBackgroundColor);

                    _game.SpriteBatch.Draw(_pixel, new Rectangle(_zoomedBox.Left, _zoomedBox.Top, 1, _zoomedBox.Height), _zoomForegroundColor);
                    _game.SpriteBatch.Draw(_pixel, new Rectangle(_zoomedBox.Right, _zoomedBox.Top, 1, _zoomedBox.Height), _zoomForegroundColor);


                    _game.SpriteBatch.Draw(_pixel, new Rectangle(_zoomedBox.Left, _zoomedBox.Top, _zoomedBox.Width + 1, 1), _zoomForegroundColor);
                    _game.SpriteBatch.Draw(_pixel, new Rectangle(_zoomedBox.Left, _zoomedBox.Bottom, _zoomedBox.Width + 1, 1), _zoomForegroundColor);

                    _game.SpriteBatch.DrawString(_font, _zoomedMessage, new Vector2(_zoomedBox.Left + _textOffset, _zoomedBox.Top + _textOffset), _zoomForegroundColor);
                }
                DrawSystemInformation();
            }
        }

        private void DrawSystemInformation()
        {
            _game.SpriteBatch.DrawString(_font,
                  "VER:" + _game.VERSION.d4lilah + "[" + _game.VERSION.Commit + "]" 
                + " LUA:" + _game.VERSION.LUA
                + " FPS:" + _game.Performance.FPS
                + " CPU:" + _game.Performance.ProcessTimePercentage + "%"
                + " MEM:" + (_game.Performance.AllocatedByteCount / 1048576) + "mB"
                + " ENT:" + _game.Performance.EntityCount
                + " SPR:" + _game.Performance.SpriteCount
                + " DRW:" + _game.Performance.DrawCount
                + " PAR:" + _game.Performance.ParticleCount
                + " DEC:" + _game.Performance.DecalCount
                + " COL:" + _game.Performance.CollisionCount
                , new Vector2(_consoleCurrentSize.Left, _consoleCurrentSize.Top - _textHeight), Color.White);
        }

        private string FormatZoomedText(string text)
        {

            for(int i = 0; i < text.Length - 6; i++)
            {
                if(text[i] == '#')
                {
                    text = text.Remove(i, 7);
                }
            }
            int indentation = 0;
            string newl = Environment.NewLine;
            for(int i = 0; i < text.Length; i++)
            {
                if(text[i] == ']')
                {
                    text = text.Insert(i + 1, newl);
                }
                if(text[i] == '{' || text[i] == '}' || text[i] == ',')
                {

                    if(text[i] == '{')
                    {
                        indentation++;
                    }
                    if(text[i] == '}')
                    {
                        indentation--;
                    }
                    indentation = Math.Max(indentation, 0);
                    if(text.Length > i + 1)
                    {
                        if(text[i + 1] == ',')
                        {
                            continue;
                        }
                    }
                    string indentationText = "";
                    for(int x = 0; x < indentation; x++)
                    {
                        indentationText += "   ";
                    }
                    text = text.Insert(i + 1, newl + indentationText);
                }
            }
            return text.Trim();
        }

        private float getDelta()
        {
            return (float)_game.GameTime.ElapsedGameTime.TotalMilliseconds / 1000;
        }

        private void CloseConsole(int len)
        {
            if(_consoleInput.Length > len)        //In case users Console key registered in input
            {
                len = _consoleInput.Length - len;
                _consoleInput = _consoleInput.Remove(_consoleInputCursor - 1, 1);
                _consoleInputCursor--;
            }
            _game.IsMouseVisible = _mouseVisibleSetting;
            _consoleOpen = false;
            _displayedText.Clear();
            _consoleCurrentSize.Height = 0;
            _game.Levels.CurrentLevel.Timescale = _currentTimescale;
        }

        public void Close()
        {
            CloseConsole(_consoleInput.Length);
        }

        private void Input(object sender, TextInputEventArgs e)
        {
            if(!_consoleOpen)
            {
                return;
            }
            char charInput = e.Character;

            if(charInput == '\b')
            {
                if(_consoleInput.Length > 0)
                {
                    if(_consoleInputCursor > 0)
                    {
                        if(_consoleInput[_consoleInputCursor - 1] == '(' && _consoleInput.Length > _consoleInputCursor && _consoleInput[_consoleInputCursor] == ')')
                        {
                            _consoleInput = _consoleInput.Remove(_consoleInputCursor, 1);
                        }
                        if(_consoleInput[_consoleInputCursor - 1] == '[' && _consoleInput.Length > _consoleInputCursor && _consoleInput[_consoleInputCursor] == ']')
                        {
                            _consoleInput = _consoleInput.Remove(_consoleInputCursor, 1);
                        }
                        if(_consoleInput[_consoleInputCursor - 1] == '{' && _consoleInput.Length > _consoleInputCursor && _consoleInput[_consoleInputCursor] == '}')
                        {
                            _consoleInput = _consoleInput.Remove(_consoleInputCursor, 1);
                        }
                        if(_consoleInput[_consoleInputCursor - 1] == '"' && _consoleInput.Length > _consoleInputCursor && _consoleInput[_consoleInputCursor] == '"')
                        {
                            _consoleInput = _consoleInput.Remove(_consoleInputCursor, 1);
                        }
                        if(_consoleInput[_consoleInputCursor - 1] == '\'' && _consoleInput.Length > _consoleInputCursor && _consoleInput[_consoleInputCursor] == '\'')
                        {
                            _consoleInput = _consoleInput.Remove(_consoleInputCursor, 1);
                        }
                        _consoleInputCursor--;
                        _consoleInput = _consoleInput.Remove(_consoleInputCursor, 1);
                    }
                }
            }
            else if(charInput == Convert.ToChar(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                if(_game.ClientSettings.ConsolePersistentHistory)
                {
                    File.AppendAllText(_consoleLogFile, _consoleInput + Environment.NewLine);
                }
                Execute();
                _userHistory.Add(_consoleInput);
                _consoleInputCursor = 0;
                _consoleInput = "";
                _selectIndex = 0;
                _selectLength = 0;
            }
            else if(charInput == '\t')
            {

            }
            else if(charInput == '\u001b') // '\u001b' == escape
            {
                _selectLength = 0;  
            }
            else if(charInput == '\u0016') // '\u0016' is Ctrl+V. copyPasta
            {
                if(Clipboard.ContainsText())
                {
                    if(_selectLength != 0)
                    {
                        int selectrange = Math.Abs(_selectLength);
                        int selectstart = Math.Min(_selectIndex, _selectIndex + _selectLength);
                        _consoleInput = _consoleInput.Remove(selectstart, selectrange);
                        if(_selectLength > 0)
                        {
                            _consoleInputCursor -= _selectLength;
                        }
                        _selectLength = 0;      
                    }
                    string clipboard = Clipboard.GetText();
                    _consoleInput = _consoleInput.Insert(_consoleInputCursor, clipboard);
                    _consoleInputCursor += clipboard.Length;
                }
            }
            else if(charInput == '\u0003') // '\u0003' is Ctrl+C
            {
                int selectrange = Math.Abs(_selectLength);
                int selectstart = Math.Min(_selectIndex, _selectIndex + _selectLength);
                Clipboard.SetText(_consoleInput.Substring(selectstart, selectrange));
            }
            else if(charInput == '\u0018') // '\u0018' is Ctrl+X
            {

            }
            else if(charInput == '\u0001') // '\u0001' is Ctrl+A
            {
                _selectIndex = 0;
                _selectLength = _consoleInput.Length;
                _consoleInputCursor = _consoleInput.Length;
            }
            else if(charInput == '§' || charInput == '\t')
            {
            }
            else if(Convert.ToChar(Data.KeyTranslation.Translate(_key)) == charInput)
            {

            }
            else if(e.Character == '\n') // NEWLINE BRO
            {

            }
            else
            {
                if(_selectLength != 0)
                {
                    int selectrange = Math.Abs(_selectLength);
                    int selectstart = Math.Min(_selectIndex, _selectIndex + _selectLength);
                    _consoleInput = _consoleInput.Remove(selectstart, selectrange);
                    if(_selectLength > 0)
                    {
                        _consoleInputCursor -= _selectLength;
                        if(_consoleInputCursor < 0)
                        {
                            _consoleInputCursor = 0;
                        }
                    }
                    _selectLength = 0;     
                }
                string content = charInput.ToString();
                if(content == "(")
                {
                    content += ")";
                }
                if(content == "{")
                {
                    content += "}";
                }
                if(content == "[")
                {
                    content += "]";
                }
                if(content == "'")
                {
                    content += "'";
                }
                if(content == "\"")
                {
                    content += "\"";
                }
                if(_consoleInputCursor > _consoleInput.Length)
                {
                    _consoleInputCursor = _consoleInput.Length;
                }
                _consoleInput = _consoleInput.Insert(_consoleInputCursor, content);
                _consoleInputCursor++;
            }
        }

        public void Execute(string input = null)
        {
            string command = _consoleInput;
            string rawCommand = _consoleInput;
            for(int i = 0; i < command.Length - 6; i++)
            {
                if(command[i] == '#')
                {
                    command = command.Remove(i, 7);
                }
            }
            if(input != null)
            {
                command = input;
            }

            if(input == null)
            {
                _userInputIds.Add(_game.Log.LogStackCount);
                _game.Log.Write(_userInputIndicator + " " + rawCommand);
            }

            DynValue result = null;
            _script = BakeScript(_script);
            ((ScriptLoaderBase)_script.Options.ScriptLoader).ModulePaths = new string[] { Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/?.lua" };
            bool userReturns = false;
            try
            {

                if(command.Length > 8)
                {
                    if(command.StartsWith("return "))
                    {
                        userReturns = true;
                    }
                }
                if(!userReturns)
                {
                    command = "return " + command;
                }
                result = _script.DoString(command);
                if(!result.IsVoid())
                {
                    _game.Log.Write(result);
                } 
            }
            catch(Exception e)
            {
                if(e.Message.EndsWith("expected near '='") && !userReturns)
                {
                    command = command.Substring(7);
                    try
                    {

                        result = _script.DoString(command);
                        if(!result.IsVoid())
                        {
                            _game.Log.Write(result);
                        }
                    }
                    catch(Exception e2)
                    {
                        _game.Log.Write("console_output " + ErrorHandling.ScriptError(e2, _game.Log.LogName, _game.ClientSettings.DumpLog));
                    }
                }
                else
                {
                    _game.Log.Write("console_output " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                }
            }
            _cursor = _game.Log.LogStack.Length;
            _userIsBottomed = true;
        }

        private Script BakeScript(Script script)
        {

            script.Globals["Quit"] = (Func<int>)ExitGame;
            script.Globals["Replay"] = (Func<string, int>)_game.Input.StartReplay;
            script.Globals["Stop"] = (Func<int>)_game.Input.StopReplay;
            script.Globals["Screenshot"] = (Func<int>)_game.TakeScreenshot;
            script.Globals["Disconnect"] = (Func<int>)_game.Levels.SetDefaultLevel;

            script.Globals["ClientSettings"] = _game.ClientSettings;
            script.Globals["Performance"] = _game.Performance;    
            script.Globals["Console"] = _userFunctions;
            script.Globals["Interface"] = _interfaceFunctions;
            script.Globals["Level"] = _levelFunctions;
            script.Globals["Entities"] = _entityFunctions;
            script.Globals["Input"] = _inputFunctions;
            script.Globals["Time"] = _game.Time;
            script.Globals["Version"] = _game.VERSION;



            return script;
        }

        public void ClearConsole()
        {
            _displayedTextFloor = _cursor + 2;
        }

        public void ClearPersistentHistory()
        {
            File.WriteAllText(_consoleLogFile, String.Empty);
        }

        private int ExitGame()
        {
            _game.Exit();
            return 1;
        }

        private void AnimateCursor()
        {
            if(_cursorFlash)
            {
                _cursorAlphaVal = MathHelper.Lerp(_cursorAlphaVal, 0f, _cursorFlashSpeed * getDelta());
                if(_cursorAlphaVal <= 0.1f)
                {
                    _cursorFlash = false;
                }
            }
            else
            {
                _cursorAlphaVal = MathHelper.Lerp(_cursorAlphaVal, 1f, _cursorFlashSpeed * getDelta());
                if(_cursorAlphaVal >= 0.9f)
                {
                    _cursorFlash = true;
                }
            }
        }

        private void AnimateWindow()
        {
            float delta = getDelta();
            if(_consoleCurrentSize.Height < _consoleMaxHeight)
            {
                _consoleCurrentSize.Height += (int)(_consoleOpenSpeed * delta) * 2;
                _consoleCurrentSize.Y -= (int)(_consoleOpenSpeed * delta);
                if(_consoleCurrentSize.Height > _consoleMaxHeight)
                {
                    _consoleCurrentSize.Height = (int)_consoleMaxHeight;
                }

            }
        }

        private void CalculatedZoomedText()
        {
            MouseState mouse = Mouse.GetState();
            _zoomedMessage = "";
            if(_consoleCurrentSize.Bottom > mouse.Position.Y && _consoleCurrentSize.Top < mouse.Position.Y && _consoleCurrentSize.Left < mouse.Position.X && _consoleCurrentSize.Right > mouse.Position.X)
            {
                if(mouse.Y < _consoleCurrentSize.Top + (Math.Min(_screenRowCount, _displayedText.Count) * _textHeight))
                {
                    int selectedMessage = (mouse.Y - _consoleCurrentSize.Top) / _textHeight;
                    if(Math.Max(_displayedTextFloor, _cursor - _screenRowCount) + selectedMessage < _game.Log.LogStack.Length)
                    {
                        _zoomedMessage = FormatZoomedText(_game.Log.LogStack[Math.Max(_displayedTextFloor, _cursor - _screenRowCount) + selectedMessage]);
                        Vector2 zoomedSize = _font.MeasureString(_zoomedMessage);
                        _zoomedBox = new Rectangle(mouse.Position.X + (int)_zoomedBoxMouseOffset.X, mouse.Position.Y + (int)_zoomedBoxMouseOffset.Y, (int)zoomedSize.X + _textOffset + _textOffset, (int)zoomedSize.Y + _textOffset + _textOffset);
                        while(_zoomedBox.Right > _game.ClientSettings.Width - 5)
                        {
                            _zoomedBox.X--;
                        }
                        while(_zoomedBox.Bottom > _game.ClientSettings.Height - 5)
                        {
                            if(_zoomedBox.Top <= 5)
                            {
                                break;
                            }
                            _zoomedBox.Y--;
                        }
                    }
                }
            }
        }

        private void HandleUserInput()
        {
            KeyboardState keyb = Keyboard.GetState();
            MouseState mouseb = Mouse.GetState();

            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) && !_userKeyDownLastFrame && _userHistory.Count > 0)
            {
                _userHistoryIndex++;
                if(_userHistoryIndex >= _userHistory.Count)
                {
                    _userHistoryIndex = 0;
                }
                _consoleInput = _userHistory[_userHistoryIndex];
                _consoleInputCursor = _consoleInput.Length;
            }
            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) && !_userKeyUpLastFrame && _userHistory.Count > 0)
            {
                _userHistoryIndex--;
                if(_userHistoryIndex < 0)
                {
                    _userHistoryIndex = _userHistory.Count - 1;
                }
                _consoleInput = _userHistory[_userHistoryIndex];
                _consoleInputCursor = _consoleInput.Length;
            }
            if((keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageUp) || mouseb.ScrollWheelValue > _mouseScroll) && !_userKeyPageUpLastFrame)
            {
                _cursor -= _userScrollSpeed;
                _userIsBottomed = false;
                if(_cursor - _screenRowCount < _displayedTextFloor)
                {
                    _cursor = _displayedTextFloor + _screenRowCount;
                }
            }
            if((keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageDown) || mouseb.ScrollWheelValue < _mouseScroll) && !_userKeyPageDownLastFrame)
            {
                _cursor += _userScrollSpeed;
                if(_cursor > _game.Log.LogStack.Length)
                {
                    _cursor = _game.Log.LogStack.Length;
                }
                if(_cursor == _game.Log.LogStack.Length)
                {
                    _userIsBottomed = true;
                }
            }

            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) && !_userKeyLeftLastFrame)
            {     
                if(_consoleInputCursor > 0)
                {
                    _consoleInputCursor--;
                }
                if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                {
                    if(!_userKeyShiftLastFrame && _selectLength == 0)
                    {
                        _selectIndex = (_consoleInputCursor + 1);                  
                    }              
                    _selectLength = _consoleInputCursor - _selectIndex;          
                    _userKeyShiftLastFrame = true;
                }
                else
                {
                    _selectLength = 0;                
                }
            }
            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) && !_userKeyRightLastFrame)
            {
                if(_consoleInputCursor < _consoleInput.Length)
                {
                    _consoleInputCursor++;
                }
                if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                {
                    if(!_userKeyShiftLastFrame && _selectLength == 0)
                    {
                        _selectIndex = (_consoleInputCursor - 1);                     
                    }    
                    _selectLength = _consoleInputCursor - _selectIndex;    
                    _userKeyShiftLastFrame = true;
                }
                else
                {
                    _selectLength = 0; 
                }
            }
            if(!keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && !keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
            {
                _userKeyShiftLastFrame = false;
            }
                if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Delete) && !_userKeyDelLastFrame)
            {
                if(_consoleInputCursor < _consoleInput.Length)
                {
                    _consoleInput = _consoleInput.Remove(_consoleInputCursor, 1);
                }
            }

            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Home) && !_userKeyHomeLastFrame)
            {
                if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                {
                    if(_selectLength == 0)
                    {
                        _selectIndex = 0;
                        _selectLength = _consoleInputCursor;
                    }
                    else
                    {                                       
                        _selectLength -= _consoleInputCursor;
                        if(_selectIndex + _selectLength < 0)
                        {
                            _selectLength = -_selectIndex;
                        }
                    }
                }
                _consoleInputCursor = 0;
            }
            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.End))
            {
                if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                {
                    if(_selectLength == 0)
                    {
                        _selectIndex = _consoleInput.Length;
                        _selectLength = _consoleInputCursor - _consoleInput.Length;
                    }
                    else
                    {
                        _selectLength += _consoleInput.Length - _consoleInputCursor;
                        if(_selectIndex + _selectLength > _consoleInput.Length)
                        {
                            _selectLength = _consoleInput.Length - (_selectIndex + _selectLength);
                        }
                    }
                }
                _consoleInputCursor = _consoleInput.Length;
            }

            _userKeyDownLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down);
            _userKeyUpLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up);
            _userKeyLeftLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left);
            _userKeyRightLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right);
            _userKeyPageDownLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageDown);
            _userKeyPageUpLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageUp);
            _userKeyDelLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Delete);
            _userKeyHomeLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Home);
            _userKeyEndLastFrame = keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.End);

            if(_game.Input.KeyPressed(_key))
            {
                CloseConsole(_consoleInputLength);
                return;
            }
            if(_userIsBottomed)
            {
                _cursor = _game.Log.LogStack.Length;
            }
                        
            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) && !_userKeyDownLastFrame && _userHistory.Count > 0)
            {
                _userHistoryIndex++;
                if(_userHistoryIndex >= _userHistory.Count)
                {
                    _userHistoryIndex = 0;
                }
                _consoleInput = _userHistory[_userHistoryIndex];
                _consoleInputCursor = _consoleInput.Length;
            }
            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) && !_userKeyUpLastFrame && _userHistory.Count > 0)
            {
                _userHistoryIndex--;
                if(_userHistoryIndex < 0)
                {
                    _userHistoryIndex = _userHistory.Count - 1;
                }
                _consoleInput = _userHistory[_userHistoryIndex];
                _consoleInputCursor = _consoleInput.Length;
            }
            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageUp) && !_userKeyPageUpLastFrame)
            {
                _cursor -= _userScrollSpeed;
                _userIsBottomed = false;
                if(_cursor - _screenRowCount < _displayedTextFloor)
                {
                    _cursor = _displayedTextFloor + _screenRowCount;
                }
            }
            if(keyb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageDown) && !_userKeyPageDownLastFrame)
            {
                _cursor += _userScrollSpeed;
                if(_cursor > _game.Log.LogStack.Length)
                {
                    _cursor = _game.Log.LogStack.Length;
                }
                if(_cursor == _game.Log.LogStack.Length)
                {
                    _userIsBottomed = true;
                }
            }
            _mouseScroll = mouseb.ScrollWheelValue;
        }

        private void RetrieveLines()
        {
            _displayedText.Clear();
            for(int i = Math.Max(_displayedTextFloor, _cursor - _screenRowCount); i < _cursor; i++)
            {
                string text = _game.Log.LogStack[i];
                //int len = text.Length;
                //for(int x = 0; x < text.Length - 6; i++)
                //{
                //    if(text[x] == '#')
                //    {
                //        len -= 7;
                //        x += 7;
                //    }
                //}
                //int WindowWidthInChar = _consoleCurrentSize.Width / _charWidth;
                //if(text.Length > WindowWidthInChar)
                //{
                //    text = text.Substring(0, WindowWidthInChar);
                //}
                _displayedText.Add(text);
            }
        }

        private void CleanUpConsoleWindow()
        {
            _mouseVisibleSetting = _game.IsMouseVisible;
            if(_game.Input.KeyPressed(_key))
            {
                _game.IsMouseVisible = true;
                _consoleOpen = true;
                _currentTimescale = _game.Levels.CurrentLevel.Timescale;
                _game.Levels.CurrentLevel.Timescale = 0.0f;
            }
            _consoleCurrentSize.Width = (int)(_game.ClientSettings.Width * _consoleSize.X);
            _consoleCurrentSize.X = (int)(_game.ClientSettings.Width * _consolePosition.X);
            _consoleCurrentSize.Y = (int)(_game.ClientSettings.Height * _consolePosition.Y) + (int)(_consoleMaxHeight / 2);
        }
    }

    public class ExposedConsole
    {
        private Game1 _game;

        [MoonSharpHidden]
        public ExposedConsole(Game1 game)
        {
            _game = game;
        }

        public bool Open
        {
            get
            {
                return _game.Console.ConsoleOpen;
            }
        }

        public int Length
        {
            get
            {
                return _game.Console.ConsoleLength;
            }
        }

        public int FullLength
        {
            get
            {
                return _game.Log.LogStack.Length;
            }
        }

        public int Clear()
        {
            _game.Console.ClearConsole();
            return 1;
        }

        public int ClearPersistentHistory()
        {
            _game.Console.ClearPersistentHistory();
            return 1;
        }

        public int Run(string file)
        {
            if(!File.Exists(System.Environment.CurrentDirectory + @"/" + file))
            {
                _game.Log.Write("Script was not found: " + file);
                return -1;
            }
            string content = File.ReadAllText(System.Environment.CurrentDirectory + @"/" + file);
            _game.Console.Execute(content);
            return 1;
        }       
    }
}
