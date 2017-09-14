using d4lilah.Data;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace d4lilah.Debug
{
    public class LogManager
    {
        private Game1 _game;
        private string _logLocation = @"/Logs/";
        private string _logName = "";
        private string _logExtension = ".log";
        private StreamWriter _currentLog;
        private List<string> _logStack = new List<string>();
        private ExposedLogInfo _logInfo;
        private int _logStackMaxCount = 100;
        private int _logStackCount = 0;
        private Stream _inputStream;
        private List<string> _alreadyLoggedMessages = new List<string>();

        public string[] LogStack
        {
            get
            {
                return _logStack.ToArray();
            }
        }

        public ExposedLogInfo LogInfo
        {
            get
            {
                return _logInfo;
            }
        }

        public int LogStackCount
        {
            get
            {
                return _logStackCount;
            }
        }

        public string LogName
        {
            get
            {
                return _logName;
            }
        }


        public LogManager(Game1 game)
        {
            _game = game;
            _logInfo = new ExposedLogInfo(_game);
            if(!_game.ClientSettings.EnableLog)
            {
                return;
            }
            _logLocation = Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/" + _logLocation + @"/";
            if(!Directory.Exists(_logLocation))
            {
                Directory.CreateDirectory(_logLocation);
            }
            _logName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour + DateTime.Now.Second + DateTime.Now.Millisecond;
            _currentLog = File.CreateText(_logLocation + _logName + _logExtension);
            _game.Exiting += OpenLogOnClose;
            Script.DefaultOptions.Stdout = InputStream; 
        }

        public void Update()
        {
            if(_inputStream != null)
            {
                byte[] buffer = new byte[2048]; // read in chunks of 2KB
                int bytesRead;
                string line = "";
                while((bytesRead = _inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    line += bytesRead.ToString();
                }
                Write(line);
            }
        }

        public int Write(object obj)
        {
            string message = "";
            Table dummyTable = new MoonSharp.Interpreter.Table(null);
            if(obj == null)
            {
                return -1;
            }
            if(obj.GetType() == message.GetType())
            {
                message = obj.ToString();
            }
            else if(obj is Table)
            {
                message = Table((Table)obj);
            }
            else if(IsNumber(obj))
            {
                message = obj.ToString();
            }
            else if(obj is DynValue)
            {
                if(((DynValue)obj).UserData != null)
                {    
                    Table userdata = new Table(null);
                    object blob = ((DynValue)obj).UserData.Object;
                    Type type = blob.GetType();
                    IList<MemberInfo> members = new List<MemberInfo>(type.GetMembers());
                    foreach(MemberInfo member in members)
                    {
                        if(member.MemberType == MemberTypes.Constructor || member.MemberType == MemberTypes.TypeInfo)
                        {
                            continue;
                        }
                        if(member.MemberType == MemberTypes.Method)
                        {
                            if(((MemberInfo)member).Name == "ToString" || ((MemberInfo)member).Name == "Equals" || ((MemberInfo)member).Name == "GetHashCode" || ((MemberInfo)member).Name == "GetType" || (((MemberInfo)member).Name.Length > 4 && ((MemberInfo)member).Name.Substring(0, 4) == "get_") || (((MemberInfo)member).Name.Length > 4 && ((MemberInfo)member).Name.Substring(0, 4) == "set_"))
                            {
                                continue;
                            }
                        }
                        if(member.GetCustomAttribute<MoonSharpHiddenAttribute>() != null)
                        {
                            continue;
                        }
                        object value = null;
                        switch(member.MemberType)
                        {
                            case MemberTypes.Field:
                                value = ((FieldInfo)member).GetValue(blob);
                                break;
                            case MemberTypes.Property:
                                value = ((PropertyInfo)member).GetValue(blob);
                                break;
                            case MemberTypes.Method:
                                value = "Method";
                                break;
                        }
                        if(value is Game1)
                        {
                            continue;    
                        }
                        DynValue val = DynValue.FromObject(null, value);
                        DynValue key = DynValue.NewString(member.Name);
                        userdata.Set(key, val);
                    }
                    message = Table(userdata);
                }
                else if(((DynValue)obj).Table != null)
                {        
                    message = Table(((DynValue)obj).Table);
                }
                else
                {
                    message = ((DynValue)obj).ToString();
                }
            } 
            else
            {
                try
                {
                    Table userdata = new Table(null);                 
                    Type type = obj.GetType();
                    IList<MemberInfo> members = new List<MemberInfo>(type.GetMembers());
                    foreach(MemberInfo member in members)
                    {
                        if(member.MemberType == MemberTypes.Constructor  || member.MemberType == MemberTypes.TypeInfo)
                        {
                            continue;
                        }
                        if(member.MemberType == MemberTypes.Method)
                        {
                            {
                                continue;
                            }
                        }
                        if(member.GetCustomAttribute<MoonSharpHiddenAttribute>() != null)
                        {
                            continue;
                        }
                        object value = null;
                        switch(member.MemberType)
                        {
                            case MemberTypes.Field:
                                value = ((FieldInfo)member).GetValue(obj);
                                break;
                            case MemberTypes.Property:
                                value = ((PropertyInfo)member).GetValue(obj);
                                break;
                            case MemberTypes.Method:
                                value = "Method";
                                break;
                        }
                        DynValue val = DynValue.FromObject(null, value);
                        DynValue key = DynValue.NewString(member.Name);
                        userdata.Set(key, val);
                        message = Table(userdata);
                    }
                }
                catch(Exception e)
                {
                    message = "Tried to log something that was not recognized. " + e.Message;
                    
                }
            }                                                                       
            message = message.Replace("\t", "").Replace("\r", "").Replace("\n", "");
            for(int i = 1; i < 10; i++)
            {
                string removeString = "chunk_" + i;
                int index = message.IndexOf(removeString);
                message = (index < 0)
                    ? message
                    : message.Remove(index, removeString.Length); 
            }        
            string timestamp = "";
            if(_game.ClientSettings.LogTimestamp)
            {
                DateTime currentTime = DateTime.Now;
                string hourExtension = "";
                if(currentTime.Hour != 0)
                {
                    int hourLength = (int)Math.Log10(currentTime.Hour) + 1;
                    for(int i = hourLength; i < 2; i++)
                    {
                        hourExtension += "0";
                    }
                    hourExtension += currentTime.Hour;
                }
                else
                {
                    hourExtension = "00";
                }
                string minuteExtension = "";
                if(currentTime.Minute != 0)
                {
                    int minuteLength = (int)Math.Log10(currentTime.Minute) + 1;
                    for(int i = minuteLength; i < 2; i++)
                    {
                        minuteExtension += "0";
                    }
                    minuteExtension += currentTime.Minute;
                }
                else
                {
                    minuteExtension = "00";
                }
                string secondExtension = "";
                if(currentTime.Second != 0)
                {

                    int secondLength = (int)Math.Log10(currentTime.Second) + 1;
                    for(int i = secondLength; i < 2; i++)
                    {
                        secondExtension += "0";
                    }
                    secondExtension += currentTime.Second;
                }
                else
                {
                    secondExtension = "00";
                }
                string millisecondExtension = "";
                if(currentTime.Millisecond != 0)
                {

                    int millisecondLength = (int)Math.Log10(currentTime.Millisecond) + 1;      
                    for(int i = millisecondLength; i < 3; i++)
                    {
                        millisecondExtension += "0";
                    }
                    millisecondExtension += currentTime.Millisecond;
                }
                else
                {
                    millisecondExtension = "000";
                }
                timestamp = "[" + hourExtension + ":" + minuteExtension + ":"  + secondExtension + ":" + millisecondExtension + "] ";
            }
            string rawMessage = message;
            for(int i = 0; i < message.Length - 6; i++)
            {
                if(message[i] == '#')
                {
                    message = message.Remove(i, 7);  
                }
            }
            bool alreadyLogged = false;
            if(_game.ClientSettings.LogStopSpam)
            {
                for(int i = 0; i < _alreadyLoggedMessages.Count; i++)
                {
                    if(message == _alreadyLoggedMessages[i])
                    {
                        alreadyLogged = true;
                        break;
                    }
                }
                _alreadyLoggedMessages.Add(message);
            }                       
            if(_game.ClientSettings.EnableLog)
            {
                if(!alreadyLogged)
                {
                    _currentLog.WriteLine(timestamp + message);
                }
            }
            message = rawMessage;
            if(!message.StartsWith(_game.Console == null ? "" : _game.Console.UserInputIndicator))
            {
                int dupCounters = 0;
                for(int i = 0; i < _logStack.Count; i++)
                {
                    string otherString;
                    string thisString;
                    otherString = _logStack[i];
                    thisString = message;

                    string tmpOther = otherString;
                    if(otherString[0] == '(')
                    {
                        tmpOther = otherString.Substring(otherString.IndexOf(')') + 2);
                    }
                    if(tmpOther == thisString)
                    {

                        if(_logStack[i][0] == '(')
                        {
                            string prevDupText = otherString.Substring(1, otherString.IndexOf(')') - 1);
                            int prevDups = int.Parse(prevDupText);
                            dupCounters += prevDups;
                        }
                        _logStack.RemoveAt(i);
                        dupCounters++;
                    }
                }
                if(dupCounters > 0)
                {
                    message = message.Insert(0, "(" + dupCounters.ToString() + ") ");
                }
            }
            _logStack.Add(message);
            while(_logStack.Count > _logStackMaxCount)
            {
                _logStack.RemoveAt(0);
            }
            return 1;
        }

        private  bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public string Table(Table table)
        {            
            string message = "";
            message = SerializeTable(table, message);
            return message;
        }

        public void Dispose()
        {
            _currentLog.Close();
        }

        private string SerializeTable(Table table, string progress)
        {
            string message = "{ ";        
            foreach(TablePair pair in table.Pairs)
            {
                
                if(pair.Value.Type == DataType.Table)
                {
                    message += ConsoleColorCoding.Variable + pair.Key.ToPrintString() + ConsoleColorCoding.Normal + " : ";
                    message = SerializeTable(pair.Value.Table, message);
                    message += ConsoleColorCoding.Normal + ", ";
                }
                else if(pair.Value.Type == DataType.UserData)
                {
                    message += ConsoleColorCoding.Variable + pair.Key.ToPrintString() + ConsoleColorCoding.Normal + " :{ " + ConsoleColorCoding.String + SerializeUserData(pair.Value.UserData) + ConsoleColorCoding.Normal + " }, ";
                }
                else
                {
                    message += ConsoleColorCoding.Variable + pair.Key.ToPrintString() + ConsoleColorCoding.Normal + " : " + ConsoleColorCoding.String + pair.Value.ToPrintString() + ConsoleColorCoding.Normal + ", ";
                }
            }
            if(message.Length > 3)
            {
                message = message.Substring(0, message.Length - 2);
            }
            message += " }";
            progress += message;
            return progress;
        }

        public void OpenLogOnClose(object sender, EventArgs e)
        {
            if(_game.ClientSettings.OpenLogOnClose && _game.ClientSettings.EnableLog)
            {
                Process.Start(_logLocation + _logName + _logExtension, "");
            }
        }

        private string SerializeUserData(UserData data)
        {
            Table userdata = new Table(null);
            object obj = data.Object;
            Type type = obj.GetType();
            IList<MemberInfo> members = new List<MemberInfo>(type.GetMembers());
            foreach(MemberInfo member in members)
            {
                if(member.MemberType == MemberTypes.Constructor || member.MemberType == MemberTypes.TypeInfo)
                {
                    continue;
                }
                if(member.MemberType == MemberTypes.Method)
                {
                    if(((MemberInfo)member).Name == "ToString" || ((MemberInfo)member).Name == "Equals" || ((MemberInfo)member).Name == "GetHashCode" || ((MemberInfo)member).Name == "GetType" || (((MemberInfo)member).Name.Length > 4 && ((MemberInfo)member).Name.Substring(0, 4) == "get_") || (((MemberInfo)member).Name.Length > 4 && ((MemberInfo)member).Name.Substring(0, 4) == "set_"))
                    {
                        continue;
                    }
                }
                if(member.GetCustomAttribute<MoonSharpHiddenAttribute>() != null || member.GetCustomAttribute<MoonSharpHideMemberAttribute>() != null)
                {
                    continue;
                }
                object value = null;
                switch(member.MemberType)
                {
                    case MemberTypes.Field:
                        value = ((FieldInfo)member).GetValue(obj);
                        break;
                    case MemberTypes.Property:
                        value = ((PropertyInfo)member).GetValue(obj);
                        break;
                    case MemberTypes.Method:
                        value = "Method";
                        break;
                }
                if(value is Game1)
                {
                    continue;
                }
                DynValue val = DynValue.FromObject(null, value);
                DynValue key = DynValue.NewString(member.Name);
                userdata.Set(key, val);
            }                 
            return Table(userdata);
        }

        private Stream InputStream
        {
            get
            {
                return _inputStream;
            }   
        }

    }

    

    public class ExposedLogInfo
    {
        public string[] LogStack
        {
            get
            {
                if(_game.Log.LogStack == null)
                {
                    return new string[0];
                }
                return _game.Log.LogStack;
            } 
        }

        public void Write(object obj)
        {
            _game.Log.Write(obj);
        }

        [MoonSharpHidden]
        private Game1 _game;

        [MoonSharpHidden]
        public ExposedLogInfo(Game1 game)
        {
            _game = game;
        }  
    }           
}
