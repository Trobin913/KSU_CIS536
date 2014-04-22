using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampFireScene
{
    class Program : GameWindow
    {
        public static void Main(string[] args)
        {
            using (Program p = new Program())
                p.Run(60.0);
        }

        int programID;
        int matrixId;
        CameraController cameraController;
        List<OBJobject> loadedAssets;

        public Program()
        {
            cameraController = new CameraController(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            VSync = VSyncMode.On;
            loadedAssets = AssetManger.LoadAssets();
            programID = ShaderUtil.LoadProgram(
                    @"Shaders\TransformVertexShader.vertexshader",
                    @"Shaders\TextureFragmentShader.fragmentshader"
                    );
            matrixId = GL.GetUniformLocation(programID, "MVP");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            cameraController.Update(e.Time);
            if (Keyboard[Key.Escape])
            {
                Exit();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.UseProgram(programID);
            GL.Disable(EnableCap.CullFace);
            Matrix4 MVP = cameraController.ProjectionMatrix * cameraController.ViewMatrix * cameraController.ModelMatrix;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.UniformMatrix4(matrixId, true, ref MVP);

            GL.Begin(PrimitiveType.Triangles);

            for (int i = 0; i < 36; i += 3)
            {
                GL.Vertex3(
                    AssetManger.CubeVertexBufferData[i],
                    AssetManger.CubeVertexBufferData[i + 1],
                    AssetManger.CubeVertexBufferData[i + 2]
                    );
            }

            GL.End();

            SwapBuffers();
        }
    }
}
