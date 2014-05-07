using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampFireScene.Particles
{
    class FireParticle : Particle
    {
        public FireParticle(Vector3 position, float life)
            : base(position, life)
        { }
    }

    class FireParticleSystem : ParticleSystem
    {
        private int lifeId;

        public FireParticleSystem(Vector3 position, int rate)
            : base(position, rate)
        {
            shaderProgramId = ShaderUtil.LoadProgram(
                @"Shaders\FireFragmentShader.fragmentshader",
                @"Shaders\FireVertexShader.vertexshader");
            lifeId = GL.GetUniformLocation(shaderProgramId, "life");
        }
    }
}
