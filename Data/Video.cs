using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace d4lilah.Data
{
    public class Video
    {
        [MoonSharpHidden]
        private Game1 _game;

        public int[][] SupportedResolutions
        {
            get
            {
                List<int[]> res = new List<int[]>();
                foreach(DisplayMode i in _game.Resolutions)
                {
                    res.Add(new[] { i.Width, i.Height });
                }
                return res.ToArray();
            }
        }

        [MoonSharpHidden]
        public Video(Game1 game)
        {
            _game = game;
        }
    }
}
