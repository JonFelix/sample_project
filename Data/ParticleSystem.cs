using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace d4lilah
{
    public class ParticleSystem
    {
        private List<Particle> _particles = new List<Particle>();
        public float Depth = 0;
        public bool Permanent = false;

        public List<Particle> Particles
        {
            get
            {
                return _particles;
            }
        }

        public ParticleSystem(Vector2 position, int amountMin, int amountMax, float directionMax, float directionMin, float speedMin, float speedMax, Texture2D[] sprites, float scaleMax, float scaleMin, float rotationSpeedMin, float rotationSpeedMax, float lifeMin, float lifeMax, Color[] colors, float depth, float dragMin, float dragMax)
        {
            Random rand = new Random();
            Depth = depth;
            int amount = Constants.rand.Next(amountMin, amountMax);
            if(lifeMax == 0 && lifeMin == 0)
            {
                Permanent = true;
            }
            for(int i = 0; i < amount; i++)
            {
                Particle part = new Particle();
                part.Texture = sprites[Constants.rand.Next(0, sprites.Length)];
                part.Position = position;
                part.Speed = Math.Max(speedMin, (float)Constants.rand.NextDouble() * speedMax);
                part.Direction = Math.Max(directionMin, (float)Constants.rand.NextDouble() * directionMax) + (float)(Math.PI / 2f);
                part.RotationSpeed = Math.Max(rotationSpeedMin, (float)Constants.rand.NextDouble() * rotationSpeedMax);
                part.Scale = Math.Max(scaleMin, (float)rand.NextDouble() * scaleMax);
                if(!Permanent)
                {
                    part.TimeToLive = DateTime.Now.AddSeconds(Math.Max(lifeMin, (float)Constants.rand.NextDouble() * lifeMax));
                }
                part.Drag = Math.Max(dragMin, (float)Constants.rand.NextDouble() * dragMax);
                part.Color = colors[Constants.rand.Next(0, colors.Length)];
                part.Rotation = 0f;
                _particles.Add(part);
            }
        }
    }

    public class Particle
    {
        public Texture2D Texture;
        public Vector2 Position;
        public float Direction;
        public float Speed;
        public float Rotation;
        public float RotationSpeed;
        public float Scale;
        public DateTime TimeToLive;
        public float Drag;
        public Color Color;   
    }
}
