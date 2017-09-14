using d4lilah.Data;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d4lilah
{
    public class SpriteManager
    {
        private Game1 _game;
        private List<SpriteInfo> _sprites = new List<SpriteInfo>();
        private List<FontInfo> _fonts = new List<FontInfo>();
        private List<SVGInfo> _svgs = new List<SVGInfo>();

        public SpriteManager(Game1 game)
        {
            _game = game;     
            foreach(string i in _game.Files.FontLocations)
            {                                                                      
                FontInfo fontinfo = new FontInfo();
                fontinfo.File = new System.IO.FileInfo(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/" + i + ".xnb");
                fontinfo.Name = i.Replace('\\', '/');
                fontinfo.Font = _game.Content.Load<SpriteFont>(i);
                fontinfo.MD5 = _game.Files.GetChecksum(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/" + i + ".xnb");
                _game.Log.Write(Debug.ConsoleColorCoding.Variable + fontinfo.Name + Debug.ConsoleColorCoding.Normal + "[" + Debug.ConsoleColorCoding.Numeric + fontinfo.MD5 + Debug.ConsoleColorCoding.Normal + "]");
                _fonts.Add(fontinfo);
            }
            foreach(string i in _game.Files.SpriteLocations)
            {
                SpriteInfo spriteinfo = new SpriteInfo();
                spriteinfo.File = new System.IO.FileInfo(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/" + i + ".png");
                spriteinfo.Name = i.Replace('\\', '/');
                spriteinfo.Sprite = _game.Content.Load<Texture2D>(i);
                spriteinfo.MD5 = _game.Files.GetChecksum(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/" + i + ".png");
                _game.Log.Write(Debug.ConsoleColorCoding.Variable + spriteinfo.Name + Debug.ConsoleColorCoding.Normal + "[" + Debug.ConsoleColorCoding.Numeric + spriteinfo.MD5 + Debug.ConsoleColorCoding.Normal + "]");
                _sprites.Add(spriteinfo);
            }
            foreach(string i in _game.Files.SVGLocations)
            {
                SVGInfo svg = new SVGInfo();
                svg.File = new System.IO.FileInfo(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/" + i + ".svg");
                //svg.Stream = File.ReadAllBytes(svg.File.FullName);
                svg.Name = i.Replace('\\', '/');
                svg.MD5 = _game.Files.GetChecksum(System.Environment.CurrentDirectory + @"/" + _game.Content.RootDirectory + @"/" + i + ".svg");
                _game.Log.Write(Debug.ConsoleColorCoding.Variable + svg.Name + Debug.ConsoleColorCoding.Normal + "[" + Debug.ConsoleColorCoding.Numeric + svg.MD5 + Debug.ConsoleColorCoding.Normal + "]");
                _svgs.Add(svg);
            }
        }

        public SpriteInfo[] GetAllSprites
        {
            get
            {
                return _sprites.ToArray();
            }
        }

        public FontInfo[] GetAllFonts
        {
            get
            {
                return _fonts.ToArray();
            }
        }

        public Texture2D GetSprite(string name)
        {                                       
            for(int i = 0; i < _sprites.Count; i++)
            {
                if(name.ToLower() == _sprites[i].Name.ToLower())
                {
                    return _sprites[i].Sprite;
                }
            }
            _game.Log.Write(Debug.ConsoleColorCoding.Error + "Sprite '" + name + "' was not found!");
            return null;
        }

        public SpriteFont GetFont(string name)
        {
            for(int i = 0; i < _fonts.Count; i++)
            {
                if(name.ToLower() == _fonts[i].Name.ToLower())
                {
                    return _fonts[i].Font;
                }
            }
            _game.Log.Write(Debug.ConsoleColorCoding.Error + "Font '" + name + "' was not found!");
            return null;
        }

        public string GetSVG(string name)
        {
            for(int i = 0; i < _svgs.Count; i++)
            {
                if(name.ToLower() == _svgs[i].Name.ToLower())
                {
                    return _svgs[i].File.FullName;
                }
            }
            _game.Log.Write(Debug.ConsoleColorCoding.Error + "SVG '" + name + "' was not found!");
            return null;
        }
    }
}
