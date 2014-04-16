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

                game.Run(60.0);
            }
        }

        //Load external assests here.
        static void game_Load(object sender, EventArgs e)
        {
            GameWindow game = sender as GameWindow;
            game.VSync = VSyncMode.On;
            loadedAssets = AssetManger.LoadAssets();
        }

        static void game_Resize(object sender, EventArgs e)
        {
            GameWindow game = sender as GameWindow;
            GL.Viewport(0, 0, game.Width, game.Height);
        }

        //Update logic here.
        static void game_UpdateFrame(object sender, EventArgs e)
        {
            GameWindow game = sender as GameWindow;
            if (game.Keyboard[Key.Escape])
            {
                game.Exit();
            }
        }

        //Draw game objects here.
        static void game_RenderFrame(object sender, EventArgs e)
        {
            GameWindow game = sender as GameWindow;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex2(-1.0f, 1.0f);
            GL.Color3(Color.SpringGreen);
            GL.Vertex2(0.0f, -1.0f);
            GL.Color3(Color.Ivory);
            GL.Vertex2(1.0f, 1.0f);

            GL.End();
            foreach (OBJobject OBJ in loadedAssets)
            {
                OBJ.Render();
            }
            game.SwapBuffers();
        }
    }
}
