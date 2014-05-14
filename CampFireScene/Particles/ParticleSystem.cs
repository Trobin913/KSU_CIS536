using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CampFireScene.Particles
{
    public class Particle : IDisposable
    {
        public Vector3 Position;
        private static Random r = new Random();

        private Vector3 _acceleration;
        private float _lifeSpan;
        private float _origLifeSpan;
        private Vector3 _origPosition;
        private Vector3 _velocity;

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

        public void Dispose()
        {
        }

        public bool IsAlive()
        {
            return _lifeSpan > 0;
        }

        public void Render()
        {
            GL.Vertex3(Position);
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

        public void Update(float time)
        {
            _lifeSpan -= time;
            float t = _origLifeSpan - _lifeSpan;
            Position += time * (_velocity + t * _acceleration);
        }
    }

    public class ParticleSystem
    {
        protected int shaderProgramId;
        private static Random r = new Random();

        private int _particleCount;
        private List<Particle> _particles;
        private Vector3 _position;
        private int generationRate;
        private int vbo;

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

        public void Dispose()
        {
            foreach (Particle p in _particles)
            {
                p.Dispose();
            }
        }

        public void Render()
        {
            //GL.UseProgram(shaderProgramId);
            //float[] particlePositions = getParticlePositions();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            //GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(particlePositions.Length * sizeof(float)), particlePositions, BufferUsageHint.StaticDraw);
            //GL.DrawArrays(PrimitiveType.Points, 0, particlePositions.Length);

            GL.UseProgram(shaderProgramId);
            GL.PointSize(5f);
            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < _particleCount; i++)
            {
                _particles[i].Render();
            }
            GL.End();
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

        private float[] getParticlePositions()
        {
            float[] particlePositions = new float[_particleCount * 3];
            for (int i = 0; i < _particleCount; i++)
            {
                particlePositions[(i * 3)] = _particles[i].Position.X;
                particlePositions[(i * 3) + 1] = _particles[i].Position.Y;
                particlePositions[(i * 3) + 2] = _particles[i].Position.Z;
            }
            return particlePositions;
        }
    }
}