using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;

namespace d4lilah.Data
{
    public class ShellTexture
    {
        [MoonSharpHidden]
        private Texture2D _texture;

        public string Name
        {
            get
            {
                return _texture.Name;
            }
        }

        public float Width
        {
            get
            {
                if(_texture != null)
                {
                    return _texture.Width;
                }
                return 0;
            }
        }

        public float Height
        {
            get
            {
                if(_texture != null)
                {
                    return _texture.Height;
                }
                return 0;
            }
        }

        [MoonSharpHidden]
        public Texture2D Texture2D
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }
            

        [MoonSharpHidden]
        public ShellTexture(Texture2D tex)
        {
            _texture = tex;
        }
    }
}
