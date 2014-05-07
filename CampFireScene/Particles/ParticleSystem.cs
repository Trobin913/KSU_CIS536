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
        public Vector3 Position;
        private Vector3 _velocity;
        private Vector3 _acceleration;
        private float _lifeSpan;

        public Particle(Vector3 position, float lifeSpan)
        {
            Position = position;
            _lifeSpan = lifeSpan;
            _velocity = new Vector3(
                ((float)r.NextDouble() - 0.5f), 
                1,
                ((float)r.NextDouble() - 0.5f));
            _acceleration = Vector3.Zero;

            _origPosition = Position;
            _origLifeSpan = _lifeSpan;
        }

        public Particle(Vector3 position, Vector3 velocity, Vector3 acceleration, float lifeSpan)
        {
            Position = position;
            _lifeSpan = lifeSpan;
            _velocity = velocity;
            _acceleration = acceleration;
        }

        public bool IsAlive()
        {
            return _lifeSpan > 0;
        }

        public void Update(float time)
        {
            _lifeSpan -= time;
            float t = _origLifeSpan - _lifeSpan;
            Position += time * (_velocity + t * _acceleration);
        }

        public void Render()
        {
            GL.Vertex3(Position);
        }

        public void Dispose()
        {

        }

        public void Reset()
        {
            Position = _origPosition;
            _lifeSpan = _origLifeSpan;
            _velocity = new Vector3(
                ((float)r.NextDouble() - 0.5f),
                1,
                ((float)r.NextDouble() - 0.5f));
            _acceleration = Vector3.Zero;
        }
    }

    public class ParticleSystem
    {
        private static Random r = new Random();

        private List<Particle> _particles;
        private Vector3 _position;
        private int generationRate;
        private int _particleCount;
        private int vbo;

        protected int shaderProgramId;

        public ParticleSystem(Vector3 position, int rate)
        {
            _position = position;
            generationRate = rate;
            _particles = new List<Particle>();
            vbo = GL.GenBuffer();
            shaderProgramId = ShaderUtil.LoadProgram(
                @"Shaders\FireVertexShader.vertexshader",
                @"Shaders\FireFragmentShader.fragmentshader");
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
            //GL.UseProgram(shaderProgramId);
            //float[] particlePositions = getParticlePositions();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            //GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(particlePositions.Length * sizeof(float)), particlePositions, BufferUsageHint.StaticDraw);
            //GL.DrawArrays(PrimitiveType.Points, 0, particlePositions.Length);
            
            //GL.PointSize(5f);
            //GL.Begin(PrimitiveType.Points);
            //for (int i = 0; i < _particleCount; i++)
            //{
            //    _particles[i].Render();
            //}
            //GL.End();
        }

        private float[] getParticlePositions()
        {
            float[] particlePositions = new float[_particleCount * 3];
            for (int i = 0; i < _particleCount; i++)
            {
                particlePositions[(i*3)] = _particles[i].Position.X;
                particlePositions[(i * 3) + 1] = _particles[i].Position.Y;
                particlePositions[(i*3) + 2] = _particles[i].Position.Z;
            }
            return particlePositions;
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
            return new Particle(
                new Vector3(
                    (float)(_position.X + r.NextDouble() - 0.5),
                    1,
                    (float)(_position.Z + r.NextDouble() - 0.5)), 
                (float)(r.NextDouble() * 3 + 0.1));
        }
    }
}