using d4lilah.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d4lilah
{
    public class DecalManager
    {
        private Game1 _game;
        private List<DecalInfo> _decals = new List<DecalInfo>();

        public DecalManager(Game1 game)
        {
            _game = game;
        }

        public void Update()
        {
            _game.Performance.DecalCount = _decals.Count;
            if(_game.ClientSettings.DecalCount > 0)
            {
                while(_decals.Count > _game.ClientSettings.DecalCount)
                {
                    _decals.RemoveAt(0);
                }
            }
        }

        public void Draw()
        {
            for(int i = 0; i < _decals.Count; i++)
            {
                _game.SpriteBatch.Draw(_decals[i].Texture, 
                    new Vector2(_decals[i].Position.X, _decals[i].Position.Y),
                    null,
                    _decals[i].Color, 
                    _decals[i].Rotation,
                    new Vector2(_decals[i].Texture.Width / 2f, _decals[i].Texture.Height / 2f),
                    _decals[i].Scale,
                    SpriteEffects.None,
                    _decals[i].Depth);
            }
        }

        public void CleanUp()
        {
            _decals.Clear();   
        }

        public int AddDecal(string texture, Table position, Table color, float rotation, float scale, float depth = 0)
        {
            DecalInfo decalinfo = new DecalInfo();
            decalinfo.Texture = _game.Sprites.GetSprite(texture);
            decalinfo.Position = TranslateVector(position);
            decalinfo.Color = TranslateColor(color);
            decalinfo.Rotation = rotation;
            decalinfo.Scale = scale;
            decalinfo.Depth = depth;
            _decals.Add(decalinfo);
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
    }
}
