using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace d4lilah.Data
{
    public class ShellVector
    {
        [MoonSharpHidden]
        private Vector2 _position;

        public float X
        {
            get
            {
                return _position.X;
            }
            set
            {
                _position.X = value;
            }
        }

        public float Y
        {
            get
            {
                return _position.Y;
            }
            set
            {
                _position.Y = value;
            }
        }

        [MoonSharpHidden]
        public Vector2 Vector
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        [MoonSharpHidden]
        public ShellVector(Vector2 pos)
        {
            _position = pos;
        }
        
    }
}
