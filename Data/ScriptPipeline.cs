using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;
using System;
using System.IO;

namespace d4lilah.Data
{
    public class ScriptPipeline
    {
        private Game1 _game;

        public ScriptPipeline(Game1 game)
        {
            _game = game;
        }

        public Script InitInstance(Script script, Stats stats)
        {
            UserData.RegisterType<ShellVector>();
            UserData.RegisterType<Vector2>();
            UserData.RegisterType<ScriptInfo>();
            UserData.RegisterType<d4lilah.Debug.ExposedLogInfo>();
            UserData.RegisterType<Rectangle>();
            UserData.RegisterType<Time>();
            UserData.RegisterType<d4lilah.Debug.ExposedConsole>();
            UserData.RegisterType<Level>();
            UserData.RegisterType<ExposedLevelFunctions>();
            script.Globals["SetTexture"] = (Func<string, int>)stats.SetTexture;   

            script.Globals["Move"] = (Func<Table, int>)stats.Move;
            script.Globals["NewTimer"] = (Func<Table, Table, int>)stats.AddTimerPayload;  

            script.Globals["KeyDown"] = (Func<string, bool>)_game.Input.KeyDown;
            script.Globals["KeyPressed"] = (Func<string, bool>)_game.Input.KeyPressed;
            script.Globals["KeyReleased"] = (Func<string, bool>)_game.Input.KeyReleased;
            script.Globals["SetMousePosition"] = (Func<Table, int>)SetMousePosition;
            script.Globals["SetCursorVisible"] = (Func<bool, int>)SetCursorVisible;

            script.Globals["CheckPosition"] = (Func<Table, float, int[], int[]>)_game.Entities.CheckPosition;
            script.Globals["CheckDistance"] = (Func<Table, float, int[]>)_game.Entities.CheckDistance;
            script.Globals["Raycast"] = (Func<Table, Table, int[], Table[]>)_game.Entities.Raycast;

            script.Globals["GetEntity"] = (Func<int, Stats>)_game.Entities.GetEntity;
            script.Globals["GetEntitiesByScript"] = (Func<string, Stats[]>)_game.Entities.GetEntitiesByScript;
            script.Globals["SendMessage"] = (Func<int, Table, int>)_game.Entities.SendMessage;
            script.Globals["SendMessageToWidget"] = (Func<string, Table, int>)_game.Entities.SendMessageToInterface;
            script.Globals["BroadcastMessage"] = (Func<Table, int>)_game.Entities.BroadcastMessage;
            script.Globals["RemoveEntity"] = (Func<int, int>)_game.Entities.RemoveEntity;

            script.Globals["Log"] = (Func<object, int>)_game.Log.Write;
            //script.Globals["LogTable"] = (Func<Table, int>)_game.Log.Table;
            script.Globals["LogInfo"] = _game.Log.LogInfo;

            script.Globals["SpawnEntity"] = (Func<string, Table, Table, int>)_game.Entities.SpawnEntity;

            script.Globals["SetCamera"] = (Func<Table, float, float, int>)_game.Camera.SetCameraPosition;
            script.Globals["SetCameraOrigin"] = (Func<Table, int>)_game.Camera.SetCameraOrigin;

            script.Globals["SpawnParticles"] = (Func<Table, int>)_game.ParticleEffects.AddParticles;
            script.Globals["SpawnDecal"] = (Func<string, Table, Table, float, float, float, int>)_game.Decals.AddDecal;

            script.Globals["DrawText"] = (Func<string, string, Table, Table, int>)_game.Entities.DrawText;
            script.Globals["TextSize"] = (Func<string, string, ShellVector>)_game.Entities.TextSize;
            script.Globals["TextureSize"] = (Func<string, ShellVector>)_game.Entities.TextureSize;
            script.Globals["DrawTexture"] = (Func<string, Table, float, Table, float, int>)_game.Entities.DrawTexture;
            script.Globals["DrawRectangle"] = (Func<Table, Table, float, int>)_game.Entities.DrawRect;
            script.Globals["DrawLine"] = (Func<Table, Table, Table, int, float, int>)_game.Entities.DrawLine;

            script.Globals["Random"] = (Func<float, float, float>)stats.Random;
            script.Globals["SetSeed"] = (Func<int, int>)stats.SetSeed;

            script.Globals["SetTable"] = (Func<string, Table, int>)stats.SaveVariable;
            script.Globals["GetTable"] = (Func<string, Table>)stats.GetVariable;
                                                                  

            //ENVIRONMENT
            script.Globals["Entity"] = stats;
            script.Globals["Time"] = _game.Time;
            script.Globals["Console"] = _game.Console.ExposedConsole;
            script.Globals["Level"] = _game.Levels.ExposedLevelFunctions;

            script.Globals["DeltaTime"] = _game.Time.DeltaTime * _game.Levels.CurrentLevel.Timescale;
            //script.Globals["SetShader"] = (Func<string, int>)SetShader;

            return script;
        }

        public Script BakeInstance(Script script, Stats stats)
        {         
            script.Globals["ScreenWidth"] = _game.ClientSettings.Width;
            script.Globals["ScreenHeight"] = _game.ClientSettings.Height;

            script.Globals["MousePosition"] = GetMousePosition();

            
            script.Globals["CameraPosition"] = _game.Camera.Position;
            script.Globals["CameraZoom"] = _game.Camera.Zoom;
            script.Globals["CameraRotation"] = _game.Camera.Rotation;        

            

            return script;
        }

        public Script InitLevel(Script script, Stats stats)
        {
            UserData.RegisterType<ShellVector>();
            UserData.RegisterType<ScriptInfo>();
            UserData.RegisterType<d4lilah.Debug.ExposedLogInfo>();
            UserData.RegisterType<Rectangle>();
            UserData.RegisterType<Time>();
            script.Globals["KeyDown"] = (Func<string, bool>)_game.Input.KeyDown;
            script.Globals["KeyPressed"] = (Func<string, bool>)_game.Input.KeyPressed;
            script.Globals["KeyReleased"] = (Func<string, bool>)_game.Input.KeyReleased;

            script.Globals["CheckPosition"] = (Func<Table, float, int[], int[]>)_game.Entities.CheckPosition;
            script.Globals["Raycast"] = (Func<Table, Table, int[], Table[]>)_game.Entities.Raycast;


            script.Globals["Log"] = (Func<object, int>)_game.Log.Write;
            //script.Globals["LogTable"] = (Func<Table, int>)_game.Log.Table;
            script.Globals["LogInfo"] = _game.Log.LogInfo;

            script.Globals["GetEntitiesByScript"] = (Func<string, Stats[]>)_game.Entities.GetEntitiesByScript;
            script.Globals["SpawnEntity"] = (Func<string, Table, Table, int>)_game.Entities.SpawnEntity;  
            script.Globals["RemoveEntity"] = (Func<int, int>)_game.Entities.RemoveEntity;
            script.Globals["GetEntity"] = (Func<int, Stats>)_game.Entities.GetEntity;
            script.Globals["SendMessage"] = (Func<int, Table, int>)_game.Entities.SendMessage;
            script.Globals["BroadcastMessage"] = (Func<Table, int>)_game.Entities.BroadcastMessage;

            script.Globals["SetCamera"] = (Func<Table, float, float, int>)_game.Camera.SetCameraPosition;
            script.Globals["SetCameraOrigin"] = (Func<Table, int>)_game.Camera.SetCameraOrigin;

            script.Globals["SetMenuLevel"] = (Func<bool, int>)stats.MenuLevel;

            script.Globals["NewTimer"] = (Func<Table, Table, int>)stats.AddTimerPayload;  
            script.Globals["ChangeLevel"] = (Func<string, int>)_game.Levels.ChangeLevel;
            script.Globals["SetBoundaries"] = (Func<Table, int>)_game.Levels.SetLevelBoundaries;

            script.Globals["Random"] = (Func<float, float, float>)stats.Random;



            script.Globals["SetTable"] = (Func<string, Table, int>)stats.SaveVariable;
            script.Globals["GetTable"] = (Func<string, Table>)stats.GetVariable;

            



            return script;
        }

        public Script BakeLevel(Script script, Stats stats)
        {
            

            script.Globals["ScreenWidth"] = _game.ClientSettings.Width;
            script.Globals["ScreenHeight"] = _game.ClientSettings.Height;
            script.Globals["SetBackColor"] = (Func<Table, int>)_game.Levels.CurrentLevel.SetBackColor;

            
            script.Globals["ID"] = stats.ID;    

            script.Globals["MousePosition"] = GetMousePosition();

            script.Globals["DeltaTime"] = (_game.GameTime.ElapsedGameTime.TotalMilliseconds / 1000) * _game.Levels.CurrentLevel.Timescale;    

            
            script.Globals["CameraPosition"] = new ShellVector(_game.Camera.Position);
            script.Globals["CameraZoom"] = _game.Camera.Zoom;
            script.Globals["CameraRotation"] = _game.Camera.Rotation;

            script.Globals["IsInMenu"] = _game.Levels.CurrentLevel.IsMenu;
            script.Globals["Time"] = _game.Time;


            return script;
        }

        private ShellVector GetMousePosition()
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
            mousepos.X += +(_game.Camera.Position - _game.Camera.Origin).X;
            mousepos.Y += +(_game.Camera.Position - _game.Camera.Origin).Y;
            return mousepos;
        }

        private int SetMousePosition(Table position)
        {
            int x = 0, y = 0;
            foreach(TablePair pair in position.Pairs)
            {
                if(pair.Key.String.ToLower() == "x")
                {
                    x = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "y")
                {
                    y = (int)pair.Value.Number;
                }

            }                                        
            Mouse.SetPosition(x, y);
            return 1;
        }

        private int SetCursorVisible(bool state)
        {
            _game.IsMouseVisible = state;
            return 1;
        }

        private int SetShader(string location)
        {                                                                
            if(File.Exists(_game.ContentLocation + location + ".xnb"))
            {
                _game.GameShader = _game.Content.Load<Effect>(location);
                return 1;
            }
            else
            {
                _game.Log.Write("Shader '" + _game.ContentLocation.Replace('\\', '/') + location.Replace('\\', '/') + "' was not found!");
            }

            return -1;
        }
    }
}
