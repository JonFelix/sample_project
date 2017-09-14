using d4lilah.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace d4lilah.Data
{
    public class InterfaceWidget
    {
        public bool IsMenuWidget = false;
        public int Depth = 0;
        public string LevelSpecific = "";
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin = Vector2.Zero;
        public bool IsInitialized = false;     
        public string RawScript = "";
        public Script Script;
        public ScriptInfo ScriptInfo;
        public DateTime UpdateCheck;
        public string Name;
        public List<Table> MessageQueue = new List<Table>();
        public List<DateTime> Timers = new List<DateTime>();
        public List<Table> TimerPayloads = new List<Table>();
        public List<Texture2D> ScaledSVGs = new List<Texture2D>();
        public List<string> SVGName = new List<string>();
        public Random Random = new Random();

        public Dictionary<string, object> Config = new Dictionary<string, object>();


        private InterfaceFunctions _functions;

        public void InitScript(Game1 game)
        {   
            _functions = new InterfaceFunctions(game, this);
            if(File.Exists(game.ContentLocation + @"\Configuration\Scripts\" + game.Files.Mod + "_" + Name + ".json"))
            {
                string content = File.ReadAllText(game.ContentLocation + @"\Configuration\Scripts\" + game.Files.Mod + "_" + Name + ".json");
                Dictionary<string, object> tmpDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            }
            UserData.RegisterType<ShellVector>();
            UserData.RegisterType<Rectangle>();
            UserData.RegisterType<Stats>();
            UserData.RegisterType<Texture2D>();
            UserData.RegisterType<Performance>();
            UserData.RegisterType<GameSettings>();
            UserData.RegisterType<Video>();
            UserData.RegisterType<Time>();
            UserData.RegisterType<VERSION>();
            Script.Globals["DrawText"] = (Func<string, string, Table, Table, float, int>)_functions.DrawText;
            Script.Globals["DrawRectangle"] = (Func<Table, Table, float, int>)_functions.DrawRect;
            Script.Globals["DrawTexture"] = (Func<string, Table, Table, int>)_functions.DrawTexture;
            Script.Globals["DrawLine"] = (Func<Table, Table, Table, int, float, int>)_functions.DrawLine;
            Script.Globals["TextSize"] = (Func<string, string, ShellVector>)_functions.TextSize;
            Script.Globals["NewTimer"] = (Func<Table, Table, int>)_functions.AddTimerPayload;
            Script.Globals["MouseInRect"] = (Func<Table, bool>)_functions.MouseInRect;
            Script.Globals["TextureSize"] = (Func<string, ShellVector>)_functions.TextureSize;

            Script.Globals["Log"] = (Func<object, int>)game.Log.Write;        

            Script.Globals["GetEntitiesByScript"] = (Func<string, Stats[]>)game.Entities.GetEntitiesByScript;
            Script.Globals["ChangeLevel"] = (Func<string, int>)game.Levels.ChangeLevel;


            Script.Globals["KeyDown"] = (Func<string, bool>)game.Input.KeyDown;
            Script.Globals["KeyPressed"] = (Func<string, bool>)game.Input.KeyPressed;
            Script.Globals["KeyReleased"] = (Func<string, bool>)game.Input.KeyReleased;

            Script.Globals["SetTable"] = (Func<string, Table, int>)_functions.SetVariable;
            Script.Globals["GetTable"] = (Func<string, Table>)_functions.GetVariable;

            Script.Globals["Video"] = game.Video;

            Script.Globals["IsInMenu"] = game.Levels.CurrentLevel.IsMenu;

            Script.Globals["Time"] = game.Time;
            Script.Globals["DeltaTime"] = game.Time.DeltaTime;
            Script.Globals["Version"] = game.VERSION;

            Script.Globals["LoadSVG"] = (Func<string, float, int>)_functions.LoadSVG;
            Script.Globals["DrawSVG"] = (Func<int, Table, Table, int>)_functions.DrawSVG;
            Script.Globals["ResizeSVG"] = (Func<int, float, int>)_functions.ResizeSVG;

            Script.Globals["Random"] = (Func<float, float, float>)_functions.Random;
        }

        public void BakeScript(Game1 game)
        {                 
            Script.Globals["ClientSettings"] = game.ClientSettings;

            Script.Globals["LogStack"] = game.Log.LogInfo.LogStack;
            Script.Globals["LogStackCount"] = game.Log.LogInfo.LogStack.Length;
            Script.Globals["ScreenWidth"] = game.ClientSettings.Width;
            Script.Globals["ScreenHeight"] = game.ClientSettings.Height;
            Script.Globals["Performance"] = game.Performance;                                         

            Script.Globals["TextInput"] = game.Interface.TextInput;

            Script.Globals["MousePosition"] = game.Interface.GetMousePosition;

           
        }

        public void UpdateScript()
        {
            RawScript = ScriptInfo.RawScript;       
            Script.DoString(RawScript);
            UpdateCheck = DateTime.Now;
        }  
    }
}  