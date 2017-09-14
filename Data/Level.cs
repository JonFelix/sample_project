using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace d4lilah.Data
{
    public class Level : Instance
    {
        public bool IsMenu = false;
        public string Name;
        private Rectangle _boundaries = new Rectangle(0, 0, 1024, 1024);
        [MoonSharpHidden]
        public RotatedRectangle[] MapColliders;
        [MoonSharpHidden]
        public float[] MapColliderRads;
        [MoonSharpHidden]
        public CollisionQuad QuadTree;
        [MoonSharpHidden]
        private List<Frame> _keystrokes = new List<Frame>();
        [MoonSharpHidden]
        private ulong _frame = 0;
        [MoonSharpHidden]
        private float _recordThreshold = 0.010f;
        [MoonSharpHidden]
        private float _recordThresholdCounter = 0.010f;
        [MoonSharpHidden]
        public int Seed;
        [MoonSharpHidden]
        public float Timescale = 1.0f;
        [MoonSharpHidden]
        private Color _backColor = new Color(0.309803922f, 0.31372549f, 0.333333333f, 1f);
        [MoonSharpHidden]
        public bool FirstTick = false;

        public int SetBackColor(Table tab)
        {
            _backColor = TranslateColor(tab);
            return 1;
        }

        [MoonSharpHidden]
        public Color HiddenBackColor
        {
            get
            {
                return _backColor;
            }
        }

        public Rectangle Boundaries
        {
            get
            {
                return _boundaries;
            }
            set
            {
                _boundaries = value;
                if(Stats.IsInitialized)
                {
                    QuadTree = new CollisionQuad(Boundaries, 64f);
                }
            }
        }

        [MoonSharpHidden]
        public Level(Game1 game, string script, string name, ScriptInfo info) : base(game, script, name, info)
        {
            Name = name.Substring(0, name.Length - 10);
            int seed = 0;
            for(int i = 0; i < name.Length; i++)
            {
                seed += (int)char.GetNumericValue(name[i]);
            }
            Seed = seed;
            Constants.rand = new Random(Seed);   
        }

        [MoonSharpHidden]
        public void Update(Game1 game)
        {      
            if(_recordThresholdCounter >= _recordThreshold)
            {
                if(!game.Console.ConsoleOpen)
                {                       
                    Frame frame = new Frame();
                    frame.Index = _frame;
                    frame.KeyStrokes = game.Input.GetPressedKeys;    
                    MouseState mouseb = Mouse.GetState();
                    frame.MouseX = mouseb.X;
                    frame.MouseY = mouseb.Y;
                    frame.Scale = Timescale;
                    frame.Time = new FrameTime();
                    frame.Time.Day = game.Time.Day;
                    frame.Time.Hour = game.Time.Hour;
                    frame.Time.Minute = game.Time.Minute;
                    frame.Time.Second = game.Time.Second;
                    frame.Time.Millisecond = game.Time.Millisecond;
                    frame.Time.Year = game.Time.Year;
                    frame.Time.Month = game.Time.Month;
                    frame.Time.DaylightSavingTime = game.Time.DaylightSavingTime;
                    frame.Time.DayOfWeek = game.Time.DayOfWeek;
                    frame.Time.DayOfYear = game.Time.DayOfYear;
                    frame.Time.MonthName = game.Time.MonthName;
                    _keystrokes.Add(frame);
                }
                _frame++;
                _recordThresholdCounter = 0;
            }
            _recordThresholdCounter += game.Time.DeltaTime;
        }

        [MoonSharpHidden]
        public void SaveReplay(Game1 game)
        {
            if(IsMenuLevel || game.Input.IsReplay || !game.ClientSettings.AutoRecord)
            {
                return;
            }
            string loc = System.Environment.CurrentDirectory + @"/" + game.Content.RootDirectory + @"/Replays/";
            if(!Directory.Exists(loc))
            {
                Directory.CreateDirectory(loc);
            }
            string filename = Name + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".rep";
            FrameMaster master = new FrameMaster();
            master.Frame = _keystrokes.ToArray();   
            master.Name = Name;
            master.Date = DateTime.Now.ToString("HH:mm:dd:MM:yyyy");
            master.Interval = _recordThreshold;
            master.Seed = Seed;
            master.Checksums = game.Files.GetAllMD5();
            string filecontent = JsonConvert.SerializeObject(master);
            game.Log.Write("Saving replay '" + filename + "'");
            File.WriteAllText(loc + filename, filecontent);
        }

        [MoonSharpHidden]
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

        [MoonSharpHidden]
        private Table ConvertColorToTable(Color col)
        {
            Table tab = new Table(null);
            tab.Set("R", DynValue.NewNumber(col.R));
            tab.Set("G", DynValue.NewNumber(col.G));
            tab.Set("B", DynValue.NewNumber(col.B));
            tab.Set("A", DynValue.NewNumber(col.A));
            return tab;
        }

        [MoonSharpHidden]
        public void InitiateReplay(Game1 game)
        {
            Frame frame = new Frame();
            frame.Index = _frame;
            frame.KeyStrokes = game.Input.GetPressedKeys;
            MouseState mouseb = Mouse.GetState();
            frame.MouseX = mouseb.X;
            frame.MouseY = mouseb.Y;
            frame.Scale = Timescale;
            frame.Time = new FrameTime();
            frame.Time.Day = game.Time.Day;
            frame.Time.Hour = game.Time.Hour;
            frame.Time.Minute = game.Time.Minute;
            frame.Time.Second = game.Time.Second;
            frame.Time.Millisecond = game.Time.Millisecond;
            frame.Time.Year = game.Time.Year;
            frame.Time.Month = game.Time.Month;
            frame.Time.DaylightSavingTime = game.Time.DaylightSavingTime;
            frame.Time.DayOfWeek = game.Time.DayOfWeek;
            frame.Time.DayOfYear = game.Time.DayOfYear;
            frame.Time.MonthName = game.Time.MonthName;
            _keystrokes.Add(frame);
        }
    }
}
