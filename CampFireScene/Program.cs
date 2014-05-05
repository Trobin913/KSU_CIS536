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
        int waterProgramID;
        int skyBoxProgramID;
        int lightProgramID;
        int waterLightProgramID;
        double time;
        float temp = 5;
        int vecId;
        int timeId;
        Vector3 vec = new Vector3(0.0f, 0.0f, 0.0f);
        public Program()
        {
            cameraController = new CameraController(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.CornflowerBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.NormalArray);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            //GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.Texture2D);
            //Load shaders
            programId = ShaderUtil.LoadProgram(
                @"Shaders\TextureFragmentShader.fragmentshader",
                @"Shaders\TransformVertexShader.vertexshader"
                );
            //waterProgramID = ShaderUtil.LoadProgram(
            //    @"Shaders\WaterFragmentShader.fragmentshader",
            //    @"Shaders\WaterVertexShader.vertexshader"
            //);
            skyBoxProgramID = ShaderUtil.LoadProgram(
                @"Shaders\SkyBoxTextureFragmentShader.fragmentshader",
                @"Shaders\SkyBoxTextureVertexShader.vertexshader"
            );
            lightProgramID = ShaderUtil.LoadProgram(
                @"Shaders\LightFragmentShader.fragmentshader",
                @"Shaders\LightVertexShader.vertexshader"
            );
            waterLightProgramID = ShaderUtil.LoadProgram(
                @"Shaders\WaterLightFragmentShader.fragmentshader",
                @"Shaders\WaterLightVertexShader.vertexshader"
            );

            GL.UseProgram(waterLightProgramID);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, new float[] { 4.0f, 4.0f, 4.0f, 1.0f });
            //GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 0.1f);
            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, new float[] { 0.4f, 0.4f, 0.4f, 1.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new float[] { .4f, .4f, 0.4f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.0f, 1.0f, 0.0f, 1.0f });


            //Load assets
            loadedAssets = AssetManger.LoadAssets();
            loadedAssets[0].shadersID = skyBoxProgramID;
            loadedAssets[1].shadersID = waterLightProgramID;
            loadedAssets[2].shadersID = lightProgramID;

            GL.UseProgram(waterLightProgramID);

            vecId = GL.GetUniformLocation(waterLightProgramID, "originPoint");
            timeId = GL.GetUniformLocation(waterLightProgramID, "Wavetime");

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
            time += e.Time;
            
            try
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                //renderTestCube();

                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref cameraController.ProjectionMatrix);
                GL.MultMatrix(ref cameraController.ViewMatrix);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                
                foreach (OBJobject obj in loadedAssets)
                {
                    if (obj.shadersID != 0) GL.UseProgram(obj.shadersID);
                    else GL.UseProgram(programId);
                    if (obj.shadersID == waterLightProgramID)
                    {
                        GL.Uniform3(vecId, vec);
                        GL.Uniform1(timeId, (float)time);
                    }
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
        private void setUniforms()
        {

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
