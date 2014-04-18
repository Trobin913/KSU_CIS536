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
        private static ShaderType getShaderTypeFromExtension(string shaderFilePath)
        {
            string ext = Path.GetExtension(shaderFilePath).ToLower();
            switch (ext)
            {
                case ".vertexshader":
                    return ShaderType.VertexShader;
                case ".fragmentshader":
                    return ShaderType.FragmentShader;
                case ".computeshader":
                    return ShaderType.ComputeShader;
                case ".geometryshader":
                    return ShaderType.GeometryShader;
                case ".geometryshaderext":
                    return ShaderType.GeometryShaderExt;
                case ".tesscontrolshader":
                    return ShaderType.TessControlShader;
                case ".tessevaluationshader":
                    return ShaderType.TessEvaluationShader;
                default:
                    throw new Exception("Unknown shader extention: " + ext);
            }
        }

        public static int LoadProgram(params string[] shaders)
        {
            int[] shaderIds = new int[shaders.Length];
            for (int i = 0; i < shaders.Length; i++)
            {
                shaderIds[i] = LoadShader(shaders[i]);
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

        public static int LoadShader(string shaderFilePath)
        {
            int shaderId = GL.CreateShader(getShaderTypeFromExtension(shaderFilePath));

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
