using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace d4lilah.Data
{
    public class CollisionQuad
    {
        public Rectangle FullQuad;
        public CollisionQuad TopLeftQuad = null;
        public CollisionQuad TopRightQuad = null;
        public CollisionQuad BottomLeftQuad = null;
        public CollisionQuad BottomRightQuad = null;

        private Stats[] _entities;       

        public CollisionQuad(Rectangle rect, float QuadDepth = 16)
        {
            FullQuad = rect;
            if(rect.Width <= QuadDepth  || rect.Height <= QuadDepth)
            {

            }
            else
            {
                TopLeftQuad = new CollisionQuad(new Rectangle(FullQuad.X, FullQuad.Y, FullQuad.Width / 2, FullQuad.Height / 2), QuadDepth);
                TopRightQuad = new CollisionQuad(new Rectangle(FullQuad.X + FullQuad.Width / 2, FullQuad.Y, FullQuad.Width / 2, FullQuad.Height / 2), QuadDepth);
                BottomLeftQuad = new CollisionQuad(new Rectangle(FullQuad.X, FullQuad.Y + FullQuad.Height / 2, FullQuad.Width / 2, FullQuad.Height / 2), QuadDepth);
                BottomRightQuad = new CollisionQuad(new Rectangle(FullQuad.X + FullQuad.Width / 2, FullQuad.Y + FullQuad.Height / 2, FullQuad.Width / 2, FullQuad.Height / 2), QuadDepth);
            }
        }

        public void PlaceEntity(Stats ent)
        {
            if(TopLeftQuad == null)
            {
                if(_entities == null)
                {
                    _entities = new Stats[1];
                    _entities[0] = ent;
                }
                else
                {
                    Stats[] tmpEnt = _entities;
                    _entities = new Stats[tmpEnt.Length + 1];
                    for(int i = 0; i < tmpEnt.Length; i++)
                    {
                        _entities[i] = tmpEnt[i];
                    }
                    _entities[tmpEnt.Length] = ent;
                }
                
            }
            else
            {
                Rectangle entRect = new Rectangle((int)ent.Position.X - ((int)ent.Texture.Width / 2), (int)ent.Position.Y - ((int)ent.Texture.Height / 2), (int)ent.Texture.Width, (int)ent.Texture.Height);
                if(TopLeftQuad.FullQuad.Intersects(entRect))
                {
                    TopLeftQuad.PlaceEntity(ent);
                }
                if(TopRightQuad.FullQuad.Intersects(entRect))
                {
                    TopRightQuad.PlaceEntity(ent);
                }
                if(BottomLeftQuad.FullQuad.Intersects(entRect))
                {
                    BottomLeftQuad.PlaceEntity(ent);
                }
                if(BottomRightQuad.FullQuad.Intersects(entRect))
                {
                    BottomRightQuad.PlaceEntity(ent);
                }
            }
        }

        public void RemoveEntity(Stats ent)
        {
            if(TopLeftQuad == null)
            {
                if(_entities != null)
                {
                    bool entLivesHere = false;
                    for(int i = 0; i < _entities.Length; i++)
                    {
                        if(_entities[i] == ent)
                        {
                            entLivesHere = true;
                        }
                    }
                    if(!entLivesHere)
                    {
                        return;
                    }
                    if(_entities.Length > 1)
                    {

                        Stats[] tmpEnts = _entities;
                        _entities = new Stats[_entities.Length - 1];
                        int tmpX = 0;
                        for(int i = 0; i < tmpEnts.Length; i++)
                        {
                            if(tmpEnts[i] == ent)
                            {

                            }
                            else
                            {
                                _entities[tmpX] = tmpEnts[i];
                                tmpX++;
                            }
                        }
                    }
                    else
                    {
                        _entities = null;
                    }
                }
            }
            
            if(TopLeftQuad != null)
            {
                Rectangle entRect = new Rectangle((int)ent.Position.X - ((int)ent.Texture.Width /2), (int)ent.Position.Y - ((int)ent.Texture.Height / 2), (int)ent.Texture.Width, (int)ent.Texture.Height);
                if(TopLeftQuad.FullQuad.Intersects(entRect))
                {
                    TopLeftQuad.RemoveEntity(ent);
                }
                if(TopRightQuad.FullQuad.Intersects(entRect))
                {
                    TopRightQuad.RemoveEntity(ent);
                }
                if(BottomLeftQuad.FullQuad.Intersects(entRect))
                {
                    BottomLeftQuad.RemoveEntity(ent);
                }
                if(BottomRightQuad.FullQuad.Intersects(entRect))
                {
                    BottomRightQuad.RemoveEntity(ent);
                }
            }
        }

        public Stats[] GetAllCollidingEntities(Rectangle rect)
        {      
            if(TopLeftQuad == null)
            {
                return _entities;
            }
            else
            {
                Stats[] ents = new Stats[0];
                if(rect.Intersects(TopLeftQuad.FullQuad))
                {
                    Stats[] addedEnts = TopLeftQuad.GetAllCollidingEntities(rect);
                    if(ents.Length > 0)
                    {
                        if(addedEnts != null)
                        {
                            if(addedEnts.Length > 0)
                            {
                                int tmpIndex = ents.Length;
                                Array.Resize<Stats>(ref ents, ents.Length + addedEnts.Length);
                                addedEnts.CopyTo(ents, tmpIndex);
                            }
                        }
                    }
                    else if(addedEnts != null)
                    {
                        ents = addedEnts;
                    }  
                }
                if(rect.Intersects(TopRightQuad.FullQuad))
                {                                                                
                    Stats[] addedEnts = TopRightQuad.GetAllCollidingEntities(rect);
                    if(ents.Length > 0)
                    {
                        if(addedEnts != null)
                        {
                            if(addedEnts.Length > 0)
                            {
                                int tmpIndex = ents.Length;
                                Array.Resize<Stats>(ref ents, ents.Length + addedEnts.Length);
                                addedEnts.CopyTo(ents, tmpIndex);
                            }
                        }
                    }
                    else if(addedEnts != null)
                    {
                        ents = addedEnts;
                    }  
                }
                if(rect.Intersects(BottomLeftQuad.FullQuad))
                {                                                                   
                    Stats[] addedEnts = BottomLeftQuad.GetAllCollidingEntities(rect);
                    if(ents.Length > 0)
                    {
                        if(addedEnts != null)
                        {
                            if(addedEnts.Length > 0)
                            {
                                int tmpIndex = ents.Length;
                                Array.Resize<Stats>(ref ents, ents.Length + addedEnts.Length);
                                addedEnts.CopyTo(ents, tmpIndex);
                            }
                        }
                    }
                    else if(addedEnts != null)
                    {
                        ents = addedEnts;
                    }
                }
                if(rect.Intersects(BottomRightQuad.FullQuad))
                {                                                                     
                    Stats[] addedEnts = BottomRightQuad.GetAllCollidingEntities(rect);
                    if(ents.Length > 0)
                    {
                        if(addedEnts != null)
                        {
                            if(addedEnts.Length > 0)
                            {
                                int tmpIndex = ents.Length;
                                Array.Resize<Stats>(ref ents, ents.Length + addedEnts.Length);
                                addedEnts.CopyTo(ents, tmpIndex);
                            }
                        }
                    }
                    else if(addedEnts != null)
                    {
                        ents = addedEnts;
                    }  
                }
                return ents;
            } 
        }

        public int CountEntities(int count)
        {
            if(TopLeftQuad != null)
            {
                return TopLeftQuad.CountEntities(0) + TopRightQuad.CountEntities(0) + BottomLeftQuad.CountEntities(0) + BottomRightQuad.CountEntities(0);
            }
            return _entities.Length;
        }
    }
}

