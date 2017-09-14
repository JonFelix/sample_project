using MoonSharp.Interpreter;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace d4lilah.Data
{       
    public class ScriptInfo
    {
        public string Name;
        public string RawScript;
        public FileInfo File;
        public int ScriptType;
        public DateTime Updated;
        public string MD5;

        

        private Game1 _game;

        public ScriptInfo(FileInfo file, Game1 game, string md5)
        {                                    
            _game = game;
            Name = file.Name;
            File = file;
            MD5 = md5;
            string scriptContent = "";
            string line = "";
            StreamReader fStream = new StreamReader(file.FullName);
            while((line = fStream.ReadLine()) != null)
            {
                scriptContent += line + Environment.NewLine;
            }
            fStream.Close();
            RawScript = scriptContent;
            Updated = DateTime.Now;
            if(file.Name.Split('.').Length > 2)
            {
                string type = (file.Name.Split('.')[file.Name.Split('.').Length - 2]).ToLower();
                if(type == Constants.SCRIPT_TYPE_ENTITY_NAME)
                {
                    ScriptType = Constants.SCRIPT_TYPE_ENTITY;
                }
                else if(type == Constants.SCRIPT_TYPE_LEVEL_NAME)
                {
                    ScriptType = Constants.SCRIPT_TYPE_LEVEL;
                }
                else if(type == Constants.SCRIPT_TYPE_WIDGET_NAME)
                {
                    ScriptType = Constants.SCRIPT_TYPE_WIDGET;
                }
                else
                {
                    ScriptType = Constants.SCRIPT_TYPE_MISC;
                }
            }
            else
            {
                ScriptType = Constants.SCRIPT_TYPE_MISC;
            }
        }

        public void UpdateScript()
        {
            if(_game != null && _game.ClientSettings.ScriptRefreshRate == 0)
            {
                return;
            }
            string scriptContent = "";
            string line = "";
            StreamReader fStream = new StreamReader(File.FullName);
            while((line = fStream.ReadLine()) != null)
            {
                scriptContent += line + Environment.NewLine;
            }
            fStream.Close();
            if(RawScript != scriptContent)
            {
                RawScript = scriptContent;
                Updated = DateTime.Now;
                _game.Log.Write(Name + " has been refreshed!");
            }
        }
    }
}
