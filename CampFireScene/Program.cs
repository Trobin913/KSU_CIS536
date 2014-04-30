using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        CameraController cameraController;
        List<OBJobject> loadedAssets;
        int programId;
        int matrixId;

        public Program()
        {
            cameraController = new CameraController(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            
            //Load shaders
            programId = ShaderUtil.LoadProgram(
                @"Shaders\TextureFragmentShader.fragmentshader",
                @"Shaders\TransformVertexShader.vertexshader"
                );
            matrixId = GL.GetUniformLocation(programId, "MVP");

            //Load assets
            loadedAssets = AssetManger.LoadAssets();
            Console.Out.WriteLine("Loaded " + loadedAssets.Count + " obj");
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
            if (Focused)
            {
                ResetCursor();
            }

            if (Keyboard[Key.Escape])
            {
                Exit();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            try
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                //renderTestCube();
                GL.UseProgram(programId);
                Matrix4 MVP = cameraController.ViewMatrix;
                GL.UniformMatrix4(matrixId, false, ref MVP);
                foreach (OBJobject obj in loadedAssets)
                {
                    obj.Render();
                }

                SwapBuffers();
            }
            catch (Exception ex)
            {
                PrintError();
                Console.Out.WriteLine(ex.ToString());
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void PrintError()
        {
            ErrorCode ec = GL.GetError();
            if (ec != 0)
            {
                Console.Out.WriteLine(ec.ToString());
            }
        }

#if DEBUG

        float[] cubeColors = {
			1.0f, 0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f, 1.0f,
			0.0f, 1.0f, 1.0f, 1.0f,
			1.0f, 0.0f, 0.0f, 1.0f,
			0.0f, 1.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 1.0f, 1.0f,
			0.0f, 1.0f, 1.0f, 1.0f,
		};

        byte[] triangles =
		{
			1, 0, 2, // front
			3, 2, 0,
			6, 4, 5, // back
			4, 6, 7,
			4, 7, 0, // left
			7, 3, 0,
			1, 2, 5, //right
			2, 6, 5,
			0, 1, 5, // top
			0, 5, 4,
			2, 3, 6, // bottom
			3, 7, 6,
		};

        float[] cube = {
			-0.5f,  0.5f,  0.5f, // vertex[0]
			 0.5f,  0.5f,  0.5f, // vertex[1]
			 0.5f, -0.5f,  0.5f, // vertex[2]
			-0.5f, -0.5f,  0.5f, // vertex[3]
			-0.5f,  0.5f, -0.5f, // vertex[4]
			 0.5f,  0.5f, -0.5f, // vertex[5]
			 0.5f, -0.5f, -0.5f, // vertex[6]
			-0.5f, -0.5f, -0.5f, // vertex[7]
		};

        private void renderTestCube()
        {
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref cameraController.ProjectionMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref cameraController.ViewMatrix);

            GL.VertexPointer(3, VertexPointerType.Float, 0, cube);
            GL.ColorPointer(4, ColorPointerType.Float, 0, cubeColors);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedByte, triangles);
        }
#endif
        /// <summary>
        /// Resets the mouse cursor to the center of the window.
        /// </summary>
        public void ResetCursor()
        {
            OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
        }
    }
}
