using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;
using System.IO;

namespace d4lilah
{
    public class VERSION
    {
        public string LUA = Script.LUA_VERSION;
        public string MoonSharp = Script.VERSION;
        public string d4lilah = "0.01pA";
        public string Commit = "";

        [MoonSharpHidden]
        public VERSION()
        {
            if(!File.Exists(Environment.CurrentDirectory + @"/Internal/System/cominfo"))
            {                                
                string content = "";
                StreamWriter file = File.CreateText(Environment.CurrentDirectory + @"/Internal/System/cominfo");
                if(File.Exists(Environment.CurrentDirectory + @"/../../../../.git/ORIG_HEAD"))
                {
                    content = File.ReadAllText(Environment.CurrentDirectory + @"/../../../../.git/ORIG_HEAD");
                    content = content.Substring(0, 8);
                }
                file.WriteLine(content);
                file.Close();
                Commit = content;
            }
            else
            {
                Commit = File.ReadAllLines(Environment.CurrentDirectory + @"/Internal/System/cominfo")[0];   
            }
        }
    }
}
