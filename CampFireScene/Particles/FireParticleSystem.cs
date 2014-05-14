using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CampFireScene.Particles
{
    internal class FireParticle : Particle
    {
        public FireParticle(Vector3 position, float life)
            : base(position, life)
        { }
    }

    internal class FireParticleSystem : ParticleSystem
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