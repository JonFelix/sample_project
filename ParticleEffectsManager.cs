using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;

namespace d4lilah
{
    public class ParticleEffectsManager
    {
        private Game1 _game;
        private List<ParticleSystem> _parts = new List<ParticleSystem>();
        private List<ParticleSystem> _permParts = new List<ParticleSystem>();

        public ParticleEffectsManager(Game1 game)
        {
            _game = game;
        }

        public void Update()
        {       
            
            for(int i = 0; i < _parts.Count; i++)
            {
                if(!_parts[i].Permanent)
                {
                    if(_parts[i].Particles.Count == 0)
                    {
                        _parts.RemoveAt(i);
                        continue;
                    }
                }
                bool transferToPerm = true;
                for(int x = 0; x < _parts[i].Particles.Count; x++)
                {
                    if(transferToPerm && _parts[i].Particles[x].Speed != 0)
                    {
                        transferToPerm = false;
                    }
                    if(_parts[i].Particles[x].TimeToLive > DateTime.Now || _parts[i].Permanent)
                    {        
                        _parts[i].Particles[x].Position += new Vector2((float)Math.Cos(_parts[i].Particles[x].Direction - (Math.PI / 2f)), (float)Math.Sin(_parts[i].Particles[x].Direction - (Math.PI / 2f))) * _parts[i].Particles[x].Speed * _game.Performance.DeltaTime;
                        _parts[i].Particles[x].Rotation += (int)_parts[i].Particles[x].RotationSpeed * _game.Performance.DeltaTime;
                        _parts[i].Particles[x].Speed = Math.Max(0, _parts[i].Particles[x].Speed - _parts[i].Particles[x].Drag);
                    }
                    else
                    {
                        _parts[i].Particles.RemoveAt(x);
                    }
                    
                }
                if(transferToPerm)
                {
                    _permParts.Add(_parts[i]);
                    _parts.RemoveAt(i);
                }
            }    
        }

        public void Draw()
        {
            ulong partCount = 0;
            for(int i = 0; i < _parts.Count; i++)
            {
                for(int x = 0; x < _parts[i].Particles.Count; x++)
                {
                    _game.SpriteBatch.Draw(_parts[i].Particles[x].Texture,
                        new Rectangle(
                            (int)_parts[i].Particles[x].Position.X - ((int)(_parts[i].Particles[x].Texture.Width * _parts[i].Particles[x].Scale) / 2),
                            (int)_parts[i].Particles[x].Position.Y - ((int)(_parts[i].Particles[x].Texture.Height * _parts[i].Particles[x].Scale) / 2),
                            (int)(_parts[i].Particles[x].Texture.Width * _parts[i].Particles[x].Scale),
                            (int)(_parts[i].Particles[x].Texture.Height * _parts[i].Particles[x].Scale)),
                        null, 
                        _parts[i].Particles[x].Color,
                        (int)_parts[i].Particles[x].Rotation,
                        new Vector2((int)(_parts[i].Particles[x].Texture.Width * _parts[i].Particles[x].Scale) / 2, (int)(_parts[i].Particles[x].Texture.Height * _parts[i].Particles[x].Scale) / 2),
                        SpriteEffects.None, _parts[i].Depth);
                    partCount++;
                }
            }
            for(int i = 0; i < _permParts.Count; i++)
            {
                for(int x = 0; x < _permParts[i].Particles.Count; x++)
                {
                    _game.SpriteBatch.Draw(_permParts[i].Particles[x].Texture,
                        new Rectangle(
                            (int)_permParts[i].Particles[x].Position.X - ((int)(_permParts[i].Particles[x].Texture.Width * _permParts[i].Particles[x].Scale) / 2),
                            (int)_permParts[i].Particles[x].Position.Y - ((int)(_permParts[i].Particles[x].Texture.Height * _permParts[i].Particles[x].Scale) / 2),
                            (int)(_permParts[i].Particles[x].Texture.Width * _permParts[i].Particles[x].Scale),
                            (int)(_permParts[i].Particles[x].Texture.Height * _permParts[i].Particles[x].Scale)),
                        null,
                        _permParts[i].Particles[x].Color,
                        (int)_permParts[i].Particles[x].Rotation,
                        new Vector2((int)(_permParts[i].Particles[x].Texture.Width * _permParts[i].Particles[x].Scale) / 2, (int)(_permParts[i].Particles[x].Texture.Height * _permParts[i].Particles[x].Scale) / 2),
                        SpriteEffects.None, _permParts[i].Depth);
                    partCount++;
                }
            }
            _game.Performance.ParticleCount = partCount;
        }

        public int AddParticles(Table table)
        {     
            Vector2 position = Vector2.Zero;
            float depth = 0;
            int amountMin = 0, amountMax = 0;
            float directionMax = 0f, directionMin = 0f;
            float speedMin = 0f, speedMax = 0f;
            List<Texture2D> sprites = new List<Texture2D>();
            float scaleMax = 0f, scaleMin = 0f;
            float rotationSpeedMin = 0f, rotationSpeedMax = 0f;
            float lifeMin = 0f, lifeMax = 0f;
            float dragMin = 0f, dragMax = 0f;
            List<Color> colors = new List<Color>();

            foreach(TablePair pair in table.Pairs)
            {
                if(pair.Key.String.ToLower() == "position")
                {
                    position = TranslateVector(pair.Value.Table);       
                }
                if(pair.Key.String.ToLower() == "amountminimum")
                {
                    amountMin = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "amountmaximum")
                {
                    amountMax = (int)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "directionminimum")
                {
                    directionMin = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "directionmaximum")
                {
                    directionMax = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "speedminimum")
                {
                    speedMin = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "speedmaximum")
                {
                    speedMax = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "scaleminimum")
                {
                    scaleMin = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "scalemaximum")
                {
                    scaleMax = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "rotationspeedmaximum")
                {
                    rotationSpeedMax = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "rotationspeedminimum")
                {
                    rotationSpeedMin = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "dragminimum")
                {
                    dragMin = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "dragmaximum")
                {
                    dragMax = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "lifeminimum")
                {
                    lifeMin = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "lifemaximum")
                {
                    lifeMax = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "depth")
                {
                    depth = (float)pair.Value.Number;
                }
                if(pair.Key.String.ToLower() == "sprites")
                {
                    foreach(TablePair spritepair in pair.Value.Table.Pairs)
                    {
                        sprites.Add(_game.Sprites.GetSprite(spritepair.Value.String));
                    }
                }    
                if(pair.Key.String.ToLower() == "colors")
                {
                    foreach(TablePair colorpair in pair.Value.Table.Pairs)
                    {
                        colors.Add(TranslateColor(colorpair.Value.Table));
                    }
                }
            }

            _parts.Add(new ParticleSystem(position,
                amountMin, amountMax,
                directionMax, directionMin,
                speedMin, speedMax,
                sprites.ToArray(),
                scaleMax, scaleMin,
                rotationSpeedMin, rotationSpeedMax,
                lifeMin, lifeMax,
                colors.ToArray(),
                depth,
                dragMin, dragMax));
            return 1;
        }

        public void ClearParticles()
        {
            _parts.Clear();
            _permParts.Clear();
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
    }
}
