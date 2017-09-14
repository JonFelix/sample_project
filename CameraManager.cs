using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d4lilah
{
    public class CameraManager
    {
        public float Zoom;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Origin;

        private Game1 _game;

        public CameraManager(Game1 game)
        {
            _game = game;
            Zoom = 1;
            Position = Vector2.Zero;
            Rotation = 0;
            Origin = Vector2.Zero;   

        }



        public int SetCameraPosition(Table position, float zoom, float rotation)
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
            Position = new Vector2(x, y);
            Zoom = zoom;
            Rotation = rotation;
            return 1;
        }

        public int SetCameraOrigin(Table origin)
        {
            float x = 0, y = 0;
            foreach(TablePair pair in origin.Pairs)
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
            Origin = new Vector2(x, y);
            return 1;
        }

        public Matrix GetTransform()
        {
            var translationMatrix = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0));
            var rotationMatrix = Matrix.CreateRotationZ(Rotation);
            var scaleMatrix = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            var originMatrix = Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));

            return translationMatrix * rotationMatrix * scaleMatrix * originMatrix;
        }

        public Vector3 GetScreenScale()
        {
            float scaleX = _game.GraphicsDevice.Viewport.Width / _game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            float scaleY = _game.GraphicsDevice.Viewport.Height / _game.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            return new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
