using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace CampFireScene
{
    class ShaderUtil
    {
        public static int LoadProgram(string[] shaders, ShaderType[] types)
        {
            int[] shaderIds = new int[shaders.Length];
            for (int i = 0; i < shaders.Length; i++)
            {
                shaderIds[i] = LoadShader(shaders[i], types[i]);
            }

            Console.Out.WriteLine("Linking program...");
            int programId = GL.CreateProgram();
            foreach (int shaderId in shaderIds)
                GL.AttachShader(programId, shaderId);

            GL.LinkProgram(programId);

            //Check program.

            foreach (int shaderId in shaderIds)
                GL.DeleteShader(shaderId);

            return programId;
        }

        public static int LoadShader(string shaderFilePath, ShaderType type)
        {
            int shaderId = GL.CreateShader(type);

            string shaderCode = string.Empty;
            try
            {
                shaderCode = string.Join("\n", File.ReadAllLines(shaderFilePath));
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Error reading shader source: " + shaderFilePath + "\n" + e.ToString());
                return -1;
            }

            Console.Out.WriteLine("Compiling shader: " + shaderFilePath);
            GL.ShaderSource(shaderId, shaderCode);
            GL.CompileShader(shaderId);

            //Check shader.

            return shaderId;
        }
    }
}
