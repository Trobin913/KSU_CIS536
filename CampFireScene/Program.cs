using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;

namespace CampFireScene
{
    class Program
    {
        static int programID;
        static int matrixId;
        static CameraController cameraController;
        static List<OBJobject> loadedAssets;
        [STAThread]
        static void Main(string[] args)
        {
            using (GameWindow game = new GameWindow())
            {
                game.Resize += game_Resize;
                game.Load += game_Load; //Initialize
                game.UpdateFrame += game_UpdateFrame; //Update
                game.RenderFrame += game_RenderFrame; //Draw

                cameraController = new CameraController(game);

                game.Run(60.0);
            }
        }

        //Load external assests here.
        static void game_Load(object sender, EventArgs e)
        {
            GameWindow game = sender as GameWindow;
            game.VSync = VSyncMode.On;
            loadedAssets = AssetManger.LoadAssets();
            programID = ShaderUtil.LoadProgram(
                    @"Shaders\TransformVertexShader.vertexshader", 
                    @"Shaders\TextureFragmentShader.fragmentshader"
                    );
            matrixId = GL.GetUniformLocation(programID, "MVP");
        }

        static void game_Resize(object sender, EventArgs e)
        {
            GameWindow game = sender as GameWindow;
            GL.Viewport(0, 0, game.Width, game.Height);
        }

        //Update logic here.
        static void game_UpdateFrame(object sender, FrameEventArgs e)
        {
            GameWindow game = sender as GameWindow;
            cameraController.Update(e.Time);
            if (game.Keyboard[Key.Escape])
            {
                game.Exit();
            }
        }

        //Draw game objects here.
        static void game_RenderFrame(object sender, EventArgs e)
        {
            GameWindow game = sender as GameWindow;
            if (game.Keyboard[Key.Space])
            {
                GL.UseProgram(0);
            }
            else
            {
                GL.UseProgram(programID);
            }

            GL.Disable(EnableCap.CullFace);
            Matrix4 MVP = cameraController.ProjectionMatrix * cameraController.ViewMatrix * new Matrix4(1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.UniformMatrix4(matrixId, true, ref MVP);

            GL.Begin(PrimitiveType.Triangles);

            GL.Vertex2(-1.0f, 1.0f);
            GL.Vertex2(0.0f, -1.0f);
            GL.Vertex2(1.0f, 1.0f);

            GL.End();
            /*
            foreach (OBJobject OBJ in loadedAssets)
            {
                OBJ.Render();
            }
             */
            game.SwapBuffers();
        }
    }
}
