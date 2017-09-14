using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace d4lilah
{
    static class ErrorHandling
    {
        static private List<Exception> _errors = new List<Exception>();

        public static string ScriptError(Exception e, string logName, bool dumpError)
        {
            if(dumpError)
            {

                bool isNewError = true;
                for(int i = 0; i < _errors.Count; i++)
                {
                    if(_errors[i] == e)
                    {
                        isNewError = false;
                        break;
                    }
                }
                if(isNewError)
                {
                    _errors.Add(e);
                    string serialized = JsonConvert.SerializeObject(e, Formatting.Indented);
                    string folder = Environment.CurrentDirectory + @"/Internal/Logs/Dumps";
                    if(!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string fileName = logName + "_" + (_errors.Count).ToString();
                    (File.Create(folder + @"\" + fileName + ".dump")).Close();
                    File.WriteAllText(folder + @"\" + fileName + ".dump", serialized);
                }
            }
            if(e is SyntaxErrorException)
            {
                return Debug.ConsoleColorCoding.Error + ((SyntaxErrorException)e).DecoratedMessage;
            }
            else if(e is ScriptRuntimeException)
            {
                return Debug.ConsoleColorCoding.Error + ((ScriptRuntimeException)e).DecoratedMessage;
            }
            else
            {
                return Debug.ConsoleColorCoding.Error + e.Message;
            }  
        }
    }
}
