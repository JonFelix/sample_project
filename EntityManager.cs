using d4lilah.Data;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace d4lilah
{
    public class EntityManager
    {
        private Game1 _game;
        private ScriptInfo[] _entities;
        private List<Instance> _activeEntities = new List<Instance>();
        private int _idCounter = 0;
        private bool _debugColls = false;
        private List<Rectangle> _debugCollsRects = new List<Rectangle>();
        private List<DateTime> _debugCollsTimer = new List<DateTime>();
        private List<float> _debugCollsRotation = new List<float>();
        private List<Color> _debugCollsColor = new List<Color>();
        private Texture2D _whitepixel;
        private TimeSpan _debugCollTimer = new TimeSpan(0, 0, 2);
        
        public bool DebugCollisions
        {
            get
            {
                return _debugColls;
            }
            set
            {
                _debugColls = value;
            }
        }

        public ScriptInfo[] Entities
        {
            get
            {
                return _entities;
            }
        }

        public EntityManager(Game1 game)
        {
            _game = game;
            _entities = _game.Files.EntityScripts;
            _whitepixel = _game.Sprites.GetSprite("Sprites/core/white");
        }

        public void Update()
        {
            _game.Performance.CollisionCount = 0;
            if(_game.Levels.ChangingLevel)
            {
                return;
            }
            if(!_game.Levels.CurrentLevel.FirstTick)
            {
                _game.Levels.CurrentLevel.InitiateReplay(_game);
                _game.Levels.CurrentLevel.FirstTick = true;
            }
            for(int i = 0; i < _activeEntities.Count; i++)
            {
                if(_activeEntities[i].Stats.IsCollidable)
                {
                    _game.Performance.CollisionCount++;
                }
                if(_activeEntities[i].UpdateChecked < _activeEntities[i].ScriptInfo.Updated)
                {
                    _activeEntities[i].UpdateScript();
                }
                _activeEntities[i].PrepareScript();
                
                if(!_activeEntities[i].Stats.IsInitialized && !_activeEntities[i].RemoveMe)
                {    
                    if(_activeEntities[i].Script.Globals["Initialize"] != null)
                    {
                        try
                        {
                            _activeEntities[i].Script.Call(_activeEntities[i].Script.Globals["Initialize"], _activeEntities[i].StartParams);
                        }
                        catch(Exception e)
                        {
                            _game.Log.Write(_activeEntities[i].Stats.Name + ": Initialize :" + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                        }
                    }                                           
                    _activeEntities[i].Stats.IsInitialized = true;       
                }     
                if(_activeEntities[i].Script.Globals["OnMessage"] != null && !_activeEntities[i].RemoveMe)
                {
                    for(int x = 0; x < _activeEntities[i].Timers.Count; x++)
                    {
                        if(_activeEntities[i].Timers[x] <= DateTime.Now)
                        {
                            _activeEntities[i].MessageQueue.Add(_activeEntities[i].TimerPayloads[x]);
                            _activeEntities[i].Timers.RemoveAt(x);
                            _activeEntities[i].TimerPayloads.RemoveAt(x);
                        }
                    }
                    for(int x = 0; x < _activeEntities[i].MessageQueue.Count; x++)
                    {
                        try
                        {
                            _activeEntities[i].Script.Call(_activeEntities[i].Script.Globals["OnMessage"], _activeEntities[i].MessageQueue[x]);   
                        }
                        catch(Exception e)
                        {
                            _game.Log.Write(_activeEntities[i].Stats.Name + ": OnMessage :" + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                        }
                }
                    _activeEntities[i].MessageQueue.Clear();
                }
                else
                {
                    _activeEntities[i].MessageQueue.Clear();
                }
                if(_activeEntities[i].Script.Globals["Update"] != null && !_activeEntities[i].RemoveMe && !_game.Editor.IsActive)
                {
                    try
                    {
                        _activeEntities[i].Script.Call(_activeEntities[i].Script.Globals["Update"]);
                    }
                    catch(Exception e)
                    {
                        _game.Log.Write(_activeEntities[i].Stats.Name + ": Update :" + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                    }
                    CheckLevelBoundaries(i);
                    if(_activeEntities[i].RemoveMe)
                    {
                        if(_activeEntities[i].Stats.IsCollidable)
                        {
                            _game.Levels.CurrentLevel.QuadTree.RemoveEntity(_activeEntities[i].Stats);
                        }
                        _activeEntities.RemoveAt(i);
                        
                    }
                }    
            }
            _game.Performance.EntityCount = _activeEntities.Count;
        }

        public void Draw()
        {
            for(int i = 0; i < _activeEntities.Count; i++)
            {                                        
                if(_activeEntities[i].Stats.IsInitialized && _activeEntities[i].Stats.Texture != null && !_activeEntities[i].RemoveMe)
                {
                    _game.SpriteBatch.Draw(_activeEntities[i].Stats.Texture.Texture2D, 
                        new Rectangle((int)_activeEntities[i].Stats.Position.X, (int)_activeEntities[i].Stats.Position.Y, (int)_activeEntities[i].Stats.Texture.Width, (int)_activeEntities[i].Stats.Texture.Height),
                         null, 
                         Color.White, 
                        _activeEntities[i].Stats.Direction + (float)(Math.PI / 2f), 
                        new Vector2(_activeEntities[i].Stats.Texture.Width / 2, _activeEntities[i].Stats.Texture.Height / 2), 
                        SpriteEffects.None, 
                        _activeEntities[i].Stats.Depth);
                }
                if(_activeEntities[i].Script.Globals["Draw"] != null && !_activeEntities[i].RemoveMe)
                {
                    try
                    {
                        _activeEntities[i].Script.Call(_activeEntities[i].Script.Globals["Draw"]);
                    }
                    catch(Exception e)
                    {
                        _game.Log.Write(_activeEntities[i].Stats.Name + ": Draw : " + ErrorHandling.ScriptError(e, _game.Log.LogName, _game.ClientSettings.DumpLog));
                    }    
                }
            }
            if(_debugColls)
            {
                for(int i = 0; i < _debugCollsRects.Count; i++)
                {
                    _game.SpriteBatch.Draw(_whitepixel, _debugCollsRects[i], null, _debugCollsColor[i], _debugCollsRotation[i], new Vector2(.5f, .5f), SpriteEffects.None, 1f);
                    if(_debugCollsTimer[i] < DateTime.Now)
                    {
                        _debugCollsRects.RemoveAt(i);
                        _debugCollsTimer.RemoveAt(i);
                        _debugCollsRotation.RemoveAt(i);
                        _debugCollsColor.RemoveAt(i);
                    }
                }
            }

        }

        public void CleanUpEntities()
        {
            _activeEntities.Clear();
        }

        private void CheckLevelBoundaries(int index)
        {
            if(!_activeEntities[index].Stats.IsInitialized)
            {
                return;
            }
            int X = (int)_activeEntities[index].Stats.Position.X;
            int Y = (int)_activeEntities[index].Stats.Position.Y;
            Vector2 outsideDistance = Vector2.Zero;
            if(_game.Levels.CurrentLevel.Boundaries.Left > X)
            {
                outsideDistance.X = X - _game.Levels.CurrentLevel.Boundaries.Left;
            }
            else if(_game.Levels.CurrentLevel.Boundaries.Right < X)
            {
                outsideDistance.X = X - _game.Levels.CurrentLevel.Boundaries.Right;
            }
            if(_game.Levels.CurrentLevel.Boundaries.Top > Y)
            {
                outsideDistance.Y = Y - _game.Levels.CurrentLevel.Boundaries.Top;
            }
            else if(_game.Levels.CurrentLevel.Boundaries.Bottom < Y)
            {
                outsideDistance.Y = Y - _game.Levels.CurrentLevel.Boundaries.Bottom;
            }
            if(outsideDistance != Vector2.Zero)
            {
                Table table = new Table(null);
                Table vectorTable = new Table(null);

                vectorTable.Set("X", DynValue.NewNumber(outsideDistance.X));
                vectorTable.Set("Y", DynValue.NewNumber(outsideDistance.Y));
                table.Set("BORDERPASSED", DynValue.NewBoolean(true));
                table.Set("Offset", DynValue.NewTable(vectorTable));
                SendMessage(_activeEntities[index].Stats.ID, table);
            }
        }

        public int SpawnEntity(string name, Table position, Table startParams = null)
        {
            Vector2 pos;
            if(position != null)
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
                pos = new Vector2(x, y);
            }
            else
            {
                _game.Log.Write("Tried to spawn '" + name + "' but position was not understood.");
                return -1;
            }
            if(name.Length < 11)
            {
                name = name + ".entity.lua";
            }
            else if(name.Substring(name.Length - 11, 11) != ".entity.lua")
            {
                name = name + ".entity.lua";
            }

            for(int i = 0; i < _entities.Length; i++)
            {
                if(_entities[i].Name == name)
                {                                           
                    Instance instance = new Instance(_game, _entities[i].RawScript, _entities[i].Name, _entities[i]);   
                    instance.Script = _game.Pipeline.InitInstance(instance.Script, instance.Stats);
                    if(startParams != null)
                    {
                        instance.StartParams = SanitizeTable(startParams, instance.Script);
                    }
                    else
                    {
                        instance.StartParams = new Table(instance.Script);
                    }
                    instance.Stats.ID = _idCounter;
                    instance.Stats.Position.Vector = pos;
                    _idCounter++;
                    _activeEntities.Add(instance);
                    return instance.Stats.ID;
                }
            }
            _game.Log.Write("Could not find entity '" + name + "'");
            return 0;
        }

        public Table SanitizeTable(Table oldTable, Script newOwner)
        {              
            return SanitizeTableInternal(oldTable, new Table(newOwner), newOwner); 
        }

        private Table SanitizeTableInternal(Table table, Table progress, Script newOwner)
        {
            Table newTable = new Table(newOwner);
            foreach(TablePair pair in table.Pairs)
            {
                DynValue key = pair.Key;
                DynValue val = pair.Value;
                if(val.Type == DataType.Table)
                {                                                                                                 
                    newTable.Set(key, DynValue.NewTable(SanitizeTableInternal(pair.Value.Table, newTable, newOwner)));   
                }
                else
                {
                    if(val.Type != DataType.Function)    // IGNORES FUNCTION
                    {
                        newTable.Set(key, DynValue.FromObject(newOwner, val.ToObject()));
                    }                             
                    
                }
            }   
            return newTable;
        }        

        public int[] CheckPosition(Table rect, float rotation = 0f, int[] excludelist = null)
        {
            int x = 0, y = 0, w = 0, h = 0;
            foreach(TablePair pair in rect.Pairs)
            {
                if(pair.Key.String.ToLower() == "x")
                {
                    x = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "y")
                {
                    y = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "w" || pair.Key.String.ToLower() == "width")
                {
                    w = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "h" || pair.Key.String.ToLower() == "height")
                {
                    h = (int)pair.Value.Number;
                }
            }
            Rectangle rect1 = new Rectangle(x-(w/2), y-(h/2), w, h);
            RotatedRectangle rotatedRect1 = new RotatedRectangle(rect1, rotation);
            float collRad = Vector2.Distance(Vector2.Zero, new Vector2((float)rect1.Width, (float)rect1.Height)) / 2f;
            Vector2 myPos = new Vector2(x, y);
            Stats[] colliders = _game.Levels.CurrentLevel.QuadTree.GetAllCollidingEntities(rect1);  
            List<int> collisions = new List<int>();
            List<int> ignoreID = new List<int>();
            if(excludelist != null)
            {
                ignoreID.AddRange(excludelist);
            }
            for(int i = 0; i < colliders.Length; i++)
            {
                bool ignore = false;
                if(colliders[i] == null)
                {
                    continue;
                } 
                for(int j = 0; j < ignoreID.Count; j++)
                {
                    if(ignoreID[j] == colliders[i].ID)
                    {
                        ignore = true;
                        break;
                    }
                }
                if(ignore)
                {
                    continue;
                }
                ignoreID.Add(colliders[i].ID);          
                if(Vector2.Distance(myPos, colliders[i].Position.Vector) <= collRad + colliders[i].Instance.CollisionRad)
                {          
                    int xO = (int)colliders[i].Position.X - ((int)colliders[i].Texture.Width / 2);
                    int yO = (int)colliders[i].Position.Y - ((int)colliders[i].Texture.Height / 2);
                    int wO = (int)colliders[i].Texture.Width;
                    int hO = (int)colliders[i].Texture.Height;
                    Rectangle rect2 = new Rectangle(xO, yO, wO, hO);
                    RotatedRectangle rotatedRect2 = new RotatedRectangle(rect2, colliders[i].Direction);
                    if(_debugColls)
                    {
                        _debugCollsRects.Add(new Rectangle(rect1.X + (rect1.Width / 2), rect1.Y + (rect1.Height / 2), rect1.Width, rect1.Height));
                        _debugCollsRotation.Add(rotation);
                        _debugCollsTimer.Add(DateTime.Now.Add(_debugCollTimer));
                        _debugCollsColor.Add(Color.Red * 0.1f);
                        _debugCollsRects.Add(new Rectangle(rect2.X + (rect2.Width / 2), rect2.Y + (rect2.Height / 2), rect2.Width, rect2.Height));
                        _debugCollsRotation.Add(colliders[i].Direction);
                        _debugCollsTimer.Add(DateTime.Now.Add(_debugCollTimer));
                        _debugCollsColor.Add(Color.Red * 0.1f);
                    }
                    if(rect1.Intersects(rect2))
                    {   
                        //if(rotatedRect1.Intersects(rotatedRect2))
                        //{
                            collisions.Add(colliders[i].ID);
                        //}
                    }
                }
            }
            return collisions.ToArray();
        }

        public Table[] Raycast(Table position, Table direction , int[] ignoreList = null)
        {
            Vector2 p1 = Vector2.Zero;
            if(position != null)
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
                p1 = new Vector2(x, y);
            }
            Vector2 p2 = Vector2.Zero;
            if(direction != null)
            {
                float x = 0, y = 0;
                foreach(TablePair pair in direction.Pairs)
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
                p2 = new Vector2(x, y);
            }
            Vector2 o1 = p1, o2 = p2;
            float left = p1.X >= p2.X ? p2.X : p1.X;
            float right = p1.X >= p2.X ? p1.X : p2.X;
            float top = p1.Y >= p2.Y ? p2.Y : p1.Y;
            float bottom = p1.Y >= p2.Y ? p1.Y : p2.Y;
            Rectangle area = new Rectangle((int)left, (int)top, (int)right - (int)left, (int)bottom - (int)top);
            if(area.Width == 0)
            {                 
                area.Width = 1;  
            }
            if(area.Height == 0)
            {               
                area.Height = 1;   
            }
            Stats[] inRange = _game.Levels.CurrentLevel.QuadTree.GetAllCollidingEntities(area);
            List<int> ignoreID = new List<int>();
            if(ignoreList != null)
            {
                ignoreID.AddRange(ignoreList);
            }
            List<Table> collisions = new List<Table>();
            List<Vector2> positions = new List<Vector2>();
            bool firstRun = false;
            for(int i = 0; i < inRange.Length; i++)
            {
                if(inRange[i] == null)
                {
                    continue;
                }
                if(!firstRun)
                {
                    if(_debugColls)
                    {
                        Rectangle tmpRect = new Rectangle((int)p1.X + (int)((p2.X - p1.X) / 2), (int)p1.Y + (int)((p2.Y - p1.Y) / 2), (int)Vector2.Distance(p1, p2), 1);
                        _debugCollsRects.Add(tmpRect);
                        _debugCollsRotation.Add((float)Math.Atan2(p2.Y - (double)p1.Y, p2.X - (double)p1.X));
                        _debugCollsTimer.Add(DateTime.Now.Add(_debugCollTimer));
                        _debugCollsColor.Add(Color.Blue);
                    }
                    firstRun = true;
                }
                bool ignore = false;
                for(int x = 0; x < ignoreID.Count; x++)
                {
                    if(ignoreID[x] == inRange[i].ID)
                    {
                        ignore = true;
                        break;  
                    }
                }
                if(ignore)
                {
                    continue;
                }
                ignoreID.Add(inRange[i].ID);
                if(_debugColls)
                {                                                                                                                                                            
                    _debugCollsRects.Add(new Rectangle((int)inRange[i].Position.X, (int)inRange[i].Position.Y, (int)inRange[i].Texture.Width, (int)inRange[i].Texture.Height));
                    _debugCollsRotation.Add(0f);
                    _debugCollsTimer.Add(DateTime.Now.Add(_debugCollTimer));
                    _debugCollsColor.Add(Color.Red * 0.1f);
                }
 
                Rectangle rect = new Rectangle((int)(inRange[i].Position.X - (inRange[i].Texture.Width / 2)), (int)(inRange[i].Position.Y - (inRange[i].Texture.Height / 2)), (int)inRange[i].Texture.Width, (int)inRange[i].Texture.Height);
                double u1 = 0, u2 = 1;
                int x0 = (int)p1.X, y0 = (int)p1.Y, x1 = (int)p2.X, y1 = (int)p2.Y;
                int dx = x1 - x0, dy = y1 - y0;
                int[] p = { -dx, dx, -dy, dy };
                int[] q = { x0 - rect.Left, rect.Right - x0, y0 - rect.Top, rect.Bottom - y0 };
                for(int x = 0; x < 4; x++)
                {
                    if(p[x] == 0)
                    {
                        if(q[x] < 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        double u = (double)q[x] / p[x];
                        if(p[x] < 0)
                        {
                            u1 = Math.Max(u, u1);
                        }
                        else
                        {
                            u2 = Math.Min(u, u2);
                        }
                    }
                }
                if(u1 > u2)
                {
                    continue;
                }                       
                Vector2 np1 = new Vector2((float)(x0 + u1 * dx), (float)(y0 + u1 * dy));
                Vector2 np2 = new Vector2((float)(x0 + u2 * dx), (float)(y0 + u2 * dy));
                Table coll = new Table(null);
                Table posTable = new Table(null);
                posTable.Set("X", DynValue.NewNumber(Vector2.Distance(p1, np1) <= Vector2.Distance(p1, np2) ? np1.X : np2.X));
                posTable.Set("Y", DynValue.NewNumber(Vector2.Distance(p1, np1) <= Vector2.Distance(p1, np2) ? np1.Y : np2.Y));
                coll.Set("ID", DynValue.NewNumber(inRange[i].ID));
                coll.Set("Position", DynValue.NewTable(posTable));
                collisions.Add(coll);    
            }
            IComparer<Table> comparer = new CollisionComparer();
            Table[] colls = collisions.ToArray();
            colls = colls.OrderBy(x => Vector2.Distance(o1, new Vector2((float)x.Get("Position").Table.Get("X").Number, (float)x.Get("Position").Table.Get("Y").Number))).ToArray<Table>();
            return colls;
        }          
       
        public int[] CheckDistance(Table vec, float distance = 0f)
        {
            int x = 0, y = 0;
            foreach(TablePair pair in vec.Pairs)
            {
                if(pair.Key.String.ToLower() == "y")
                {
                    y = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "x")
                {
                    x = (int)pair.Value.Number;
                }
            }
            Vector2 orig = new Vector2(x, y);
            List<int> collisions = new List<int>();
            Rectangle area = new Rectangle((int)orig.X - (int)distance, (int)orig.Y - (int)distance, (int)distance * 2, (int)distance * 2);
            Stats[] colliders = _game.Levels.CurrentLevel.QuadTree.GetAllCollidingEntities(area);
            List<int> ignoreID = new List<int>();
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i] == null)
                {
                    continue;
                }
                bool ignore = false;
                for(int j = 0; j < ignoreID.Count; j++)
                {
                    if(ignoreID[j] == colliders[i].ID)
                    {
                        ignore = true;
                        break;

                    }
                }
                if(ignore)
                {
                    continue;
                }
                ignoreID.Add(colliders[i].ID);
                Vector2 posO = colliders[i].Position.Vector;
                float dist0 = Vector2.Distance(Vector2.Zero, new Vector2(colliders[i].Texture.Width, colliders[i].Texture.Height));
                if(Vector2.Distance(posO, orig) <= dist0 + distance)
                {
                    collisions.Add(colliders[i].ID);
                }
            }
            return collisions.ToArray();
        }

        public bool CheckMapCollision(Table rect, float rotation = 0f)
        {        
            int x = 0, y = 0, w = 0, h = 0;
            foreach(TablePair pair in rect.Pairs)
            {
                if(pair.Key.String.ToLower() == "x")
                {
                    x = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "y")
                {
                    y = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "w" || pair.Key.String.ToLower() == "width")
                {
                    w = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "h" || pair.Key.String.ToLower() == "height")
                {
                    h = (int)pair.Value.Number;
                }
            }
            Rectangle rect1 = new Rectangle(x - (w / 2), y - (h / 2), w, h);
            RotatedRectangle rotatedRect1 = new RotatedRectangle(rect1, rotation);
            float collRad = Vector2.Distance(Vector2.Zero, new Vector2((float)rect1.Width, (float)rect1.Height)) / 2f;
            Vector2 myPos = new Vector2(x, y);
            List<int> collisions = new List<int>();
            for(int i = 0; i < _game.Levels.CurrentLevel.MapColliders.Length; i++)
            {
                Vector2 otherVect = new Vector2(_game.Levels.CurrentLevel.MapColliders[i].X + (_game.Levels.CurrentLevel.MapColliders[i].Width / 2), _game.Levels.CurrentLevel.MapColliders[i].Y + (_game.Levels.CurrentLevel.MapColliders[i].Height / 2));
                if(Vector2.Distance(myPos, otherVect) <= collRad + _game.Levels.CurrentLevel.MapColliderRads[i])
                {       
                    int xO = _game.Levels.CurrentLevel.MapColliders[i].X - (_game.Levels.CurrentLevel.MapColliders[i].Width / 2);
                    int yO = _game.Levels.CurrentLevel.MapColliders[i].Y - (_game.Levels.CurrentLevel.MapColliders[i].Height / 2);
                    int wO = _game.Levels.CurrentLevel.MapColliders[i].Width;
                    int hO = _game.Levels.CurrentLevel.MapColliders[i].Height;
                    Rectangle rect2 = new Rectangle(xO, yO, wO, hO);
                    RotatedRectangle rotatedRect2 = new RotatedRectangle(rect2, _game.Levels.CurrentLevel.MapColliders[i].Rotation);
                    if(rotatedRect1.Intersects(rotatedRect2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public Stats GetEntity(int id)
        {
            for(int i = 0; i < _activeEntities.Count; i++)
            {
                if(_activeEntities[i].Stats.ID == id)
                {
                    return _activeEntities[i].Stats;
                }
            }
            return null;
        }

        public int RemoveEntity(int id)
        {
            for(int i = 0; i < _activeEntities.Count; i++)
            {
                if(_activeEntities[i].Stats.ID == id)
                {
                    _activeEntities[i].RemoveMe = true;
                    return 1;
                }
            }
            return -1;
        }

        public Stats[] GetEntitiesByScript(string name)
        {
            List<Stats> entities = new List<Stats>();
            for(int i = 0; i < _activeEntities.Count; i++)
            {
                if(_activeEntities[i].Stats.Name == name)
                {
                    entities.Add(_activeEntities[i].Stats);
                }
            }
            return entities.ToArray();
        }

        public int SendMessage(int id, Table message)
        {
            Instance target = null;
            for(int i = 0; i < _activeEntities.Count; i++)
            {
                if(_activeEntities[i].Stats.ID == id)
                {
                    target = _activeEntities[i];
                    break;
                }
            }
            if(target == null)
            {
                return 0;
            }
            target.MessageQueue.Add(SanitizeTable(message, target.Script));
            return 1;
        }

        public int SendMessageToInterface(string widget, Table data)
        {
            if(widget.Length < 11)
            {
                widget = widget + ".widget.lua";
            }
            else if(widget.Substring(widget.Length - 11, 11) != ".widget.lua")
            {
                widget = widget + ".widget.lua";
            }
            for(int i = 0; i < _game.Interface.ActiveWidgets.Length; i++)
            {
                if(_game.Interface.ActiveWidgets[i].Name == widget)
                {
                    _game.Interface.ActiveWidgets[i].MessageQueue.Add(SanitizeTable(data, _game.Interface.ActiveWidgets[i].Script));
                    return 1;
                }
            }
            return -1;
        }

        public int BroadcastMessage(Table message)
        {
            for(int i = 0; i < _activeEntities.Count; i++)
            {
                _activeEntities[i].MessageQueue.Add(SanitizeTable(message, _activeEntities[i].Script));
            }
            return 1;
        }

        public Table GetActiveEntities()
        {
            Table entities = new Table(null);
            List<string> entNames = new List<string>();
            List<int> entCount = new List<int>();
            for(int i = 0; i < _activeEntities.Count; i++)
            {
                bool isFound = false;
                for(int x = 0; x < entNames.Count; x++)
                {
                    if(entNames[x] == _activeEntities[i].Stats.Name)
                    {
                        isFound = true;
                        entCount[x]++;
                    }
                }
                if(!isFound)
                {
                    entNames.Add(_activeEntities[i].Stats.Name);
                    entCount.Add(1);
                }                       
            }
            for(int i = 0; i < entNames.Count; i++)
            {
                entities.Set(entNames[i], DynValue.NewNumber(entCount[i]));
            } 
            return entities;
        }

        public int DrawText(string font, string text, Table position, Table tcolor)
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
                _game.SpriteBatch.DrawString(sfont, i, pos, col);
                pos.Y += textHeight;
            }

            return 1;
        }

        public int DrawRect(Table rect, Table color, float depth = 0)
        {
            _game.SpriteBatch.Draw(_game.Sprites.GetSprite("Sprites/Core/white"), TranslateRect(rect), null, TranslateColor(color), 0, Vector2.Zero, SpriteEffects.None, depth);
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
            
            _game.SpriteBatch.Draw(_game.Sprites.GetSprite("Sprites/Core/white"), r, null, TranslateColor(color), angle, Vector2.Zero, SpriteEffects.None, depth);
            return 1;
        }

        public ShellVector TextSize(string font, string text)
        {
            SpriteFont sfont = _game.Sprites.GetFont(font);
            if(sfont == null)
            {
                _game.Log.Write(font + " could not be found.");
            }
            return new ShellVector(sfont.MeasureString(text));
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

        public int DrawTexture(string text, Table position, float direction, Table color, float depth = 1)
        {
            Texture2D texture = _game.Sprites.GetSprite(text);
            if(texture != null)
            {
                Color col = TranslateColor(color);
                Vector2 positionVec = TranslateVector(position);
                _game.SpriteBatch.Draw(
                    texture,
                    new Rectangle(
                        (int)positionVec.X,
                        (int)positionVec.Y,
                        texture.Width,
                        texture.Height),
                    null,
                    col,
                    direction + (float)(Math.PI /2),
                    new Vector2(texture.Width/2, texture.Height/2),
                    SpriteEffects.None,
                    depth);
            }
            return 1;
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
            return new Vector2(x, y);
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
            int x = 0, y = 0, w = 0, h = 0;
            foreach(TablePair pair in rect.Pairs)
            {
                if(pair.Key.String.ToLower() == "x")
                {
                    x = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "y")
                {
                    y = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "w" || pair.Key.String.ToLower() == "width")
                {
                    w = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "h" || pair.Key.String.ToLower() == "height")
                {
                    h = (int)pair.Value.Number;
                }
            }
            Vector2 pos = TranslateVector(rect);
            return new Rectangle((int)x, (int)y, w, h);
        }
    }

    public class CollisionComparer : IComparer<Table>
    {
        

        // Compares by Height, Length, and Width.
        public int Compare(Table x, Table y)
        {
            float xD = Vector2.Distance(new Vector2((float)x.Get("Origin").Table.Get("X").Number, (float)x.Get("Origin").Table.Get("Y").Number), new Vector2((float)x.Get("Position").Table.Get("X").Number, (float)x.Get("Position").Table.Get("Y").Number));
            float yD = Vector2.Distance(new Vector2((float)y.Get("Origin").Table.Get("X").Number, (float)y.Get("Origin").Table.Get("Y").Number), new Vector2((float)y.Get("Position").Table.Get("X").Number, (float)y.Get("Position").Table.Get("Y").Number));
            if(xD <= yD)
            {
                return 1;
            }
            return 0;
        }
    }

    public class ExposedEntityFunctions
    {
        [MoonSharpHidden]
        private Game1 _game;

        [MoonSharpHidden]
        public ExposedEntityFunctions(Game1 game)
        {
            _game = game;
        }
            
        public Table GetActiveEntities()
        {
            return _game.Entities.GetActiveEntities();
        }

        public int BroadcastMessage(Table message)
        {
            return _game.Entities.BroadcastMessage(message);
        }

        public int SendMessage(int id, Table message)
        {
            return _game.Entities.SendMessage(id, message);
        }

        public Stats[] GetEntitiesByScript(string name)
        {
            return _game.Entities.GetEntitiesByScript(name);
        }

        public int RemoveEntity(int id)
        {
            return _game.Entities.RemoveEntity(id);
        }

        public Stats GetEntity(int id)
        {
            return _game.Entities.GetEntity(id);
        }

        public int DebugCollision(bool val = true)
        {
             _game.Entities.DebugCollisions = val;
            return 1;
        }

        public int SpawnEntity(string name, Table position, Table startParams = null)
        {
            if(startParams != null)
            {
                return _game.Entities.SpawnEntity(name, position, startParams);
            }
            else
            {
                return _game.Entities.SpawnEntity(name, position);
            }
            
        }  
    }
}
