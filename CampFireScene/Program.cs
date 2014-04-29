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
           using (Program p = new Program()) p.Run(60.0);
        }
        private const float ASPECT = 4.0f / 3.0f;
        private const float NEAR_CLIP = 0.1f;
        private const float FAR_CLIP = 10000.0f;
        private const float FoV = 0.5f;
        int programID;
        int programIDOBJ;
        int matrixId;
        Camera cameraController;
        List<OBJobject> loadedAssets;

        public Program()
        {
            InputManager.InitializeKeyboard(this.Keyboard);
            cameraController = new Camera();
            cameraController.PositionSpeed = 1;
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
            programIDOBJ = ShaderUtil.LoadProgram(
                    @"Shaders\OBJObjectFragmentShader.fragmentshader",
                    @"Shaders\OBJObjectVertexShader.vertexshader"
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
            //cameraController.Update(e.Time);
            InputManager.Update();
            cameraController.CameraControls();
            if (Keyboard[Key.Escape])
            {
                Exit();
            }
        }
        double temp = 0;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            //double t = e.Time;
            
            GL.UseProgram(programID);
            GL.Disable(EnableCap.CullFace);
            Matrix4 ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FoV, ASPECT, NEAR_CLIP, FAR_CLIP);
            Matrix4 viewMatrix = cameraController.GetMatrix();
            Matrix4 MVP = ProjectionMatrix * viewMatrix;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref ProjectionMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMatrix);

            //Matrix4 MVP = cameraController.ProjectionMatrix * cameraController.ViewMatrix * cameraController.ModelMatrix;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UniformMatrix4(matrixId, true, ref MVP);

            //GL.Begin(PrimitiveType.Triangles);

            //for (int i = 0; i < 36; i += 3)
            //{
            //    GL.Vertex3(
            //        AssetManger.CubeVertexBufferData[i],
            //        AssetManger.CubeVertexBufferData[i + 1],
            //        AssetManger.CubeVertexBufferData[i + 2]
            //        );
            //}

            //GL.End();
            //GL.Begin(PrimitiveType.Triangles);
            //temp += .01f;
            //GL.Vertex3(0, 1, 0);
            //GL.Vertex3(1, -1, 0);
            //GL.Vertex3(1, 0, 0);
            //GL.End();
            //GL.UseProgram(programIDOBJ);
            foreach (OBJobject obj in loadedAssets)
            {
                obj.Render();
            }

            SwapBuffers();
        }
    }
}
