using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CampFireScene.Particles
{
    public class Particle : IDisposable
    {
        private static Random r = new Random();

        private Vector3 _origPosition;
        private float _origLifeSpan;
        private Vector3 _position;
        private Vector3 _velocity;
        private Vector3 _acceleration;
        private float _lifeSpan;
        private int _vbo;

        public Particle(Vector3 position, float lifeSpan)
        {
            _position = position;
            _lifeSpan = lifeSpan;
            _velocity = new Vector3(
                ((float)r.NextDouble() - 0.5f) * 2, 
                0.3f,
                ((float)r.NextDouble() - 0.5f)) * 2;
            _acceleration = new Vector3(
                (float)r.NextDouble() - 0.5f,
                0,
                (float)r.NextDouble() - 0.5f);

            _origPosition = _position;
            _origLifeSpan = _lifeSpan;
        }

        public Particle(Vector3 position, Vector3 velocity, Vector3 acceleration, float lifeSpan)
        {
            _position = position;
            _lifeSpan = lifeSpan;
            _velocity = velocity;
            _acceleration = acceleration;
        }

        public bool IsAlive()
        {
            return _lifeSpan > 0;
        }

        public void Load()
        {
            GL.GenBuffers(1, out _vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        }

        public void Update(float time)
        {
            _velocity += time * _acceleration;
            _position += time * _velocity;
            _lifeSpan -= time;
            //_position = new Vector3(
            //    (float)Math.Sin(_lifeSpan),
            //    _position.Y + 1,
            //    (float)Math.Cos(_lifeSpan)
            //    );
        }

        public void Render()
        {
            GL.Vertex3(_position);
        }

        public void Dispose()
        {

        }

        public void Reset()
        {
            _position = _origPosition;
            _lifeSpan = _origLifeSpan;
            _velocity = new Vector3(
                ((float)r.NextDouble() - 0.5f) * 2,
                1,
                ((float)r.NextDouble() - 0.5f)) * 2;
            _acceleration = new Vector3(
                (float)r.NextDouble() - 0.5f,
                0,
                (float)r.NextDouble() - 0.5f);
        }
    }

    public class ParticleSystem
    {
        private static Random r = new Random();

        private List<Particle> _particles;
        private Vector3 _position;
        private int generationRate;
        private int _particleCount;

        public ParticleSystem(Vector3 position, int rate)
        {
            _position = position;
            generationRate = rate;
            _particles = new List<Particle>();
        }

        public void Update(double time)
        {
            Update((float)time);
        }

        public void Update(float time)
        {
            //Update all particles
            for (int i = 0; i < _particleCount; i++)
            {
                _particles[i].Update(time);
            }

            //Generate new particles.
            int numOfNew = (int)Math.Ceiling(generationRate * time);
            for (int i = _particleCount; i < _particleCount + numOfNew; i++)
            {
                if (i >= _particles.Count)
                    _particles.Add(createNewParticle());
                else
                    _particles[i].Reset();
            }

            //Remove dead particles
            _particles = _particles.OrderBy((p) => !p.IsAlive()).ToList();
            _particleCount = _particles.Count((p) => p.IsAlive());

            Console.Out.Write(string.Format("{0}:{1}\r", _particles.Count, _particleCount));
        }

        public void Render()
        {
            GL.UseProgram(0);
            GL.PointSize(5f);
            GL.Begin(PrimitiveType.Points);
            foreach (Particle p in _particles.GetRange(0, _particleCount))
            {
                p.Render();
            }
            GL.End();
        }

        public void Dispose()
        {
            foreach (Particle p in _particles)
            {
                p.Dispose();
            }
        }

        protected virtual Particle createNewParticle()
        {
            return new Particle(new Vector3(1), r.Next(100) + 1);
        }
    }
}