using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CampFireScene
{
    /// <summary>
    /// 
    /// </summary>
    //public class Renderer
    //{
    //    public void RenderFrame(object sender, FrameEventArgs e)
    //    {
    //        GameWindow game = sender as GameWindow;
    //        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

    //        GL.MatrixMode(MatrixMode.Projection);
    //        GL.LoadIdentity();
    //        GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

    //        GL.Begin(PrimitiveType.Triangles);

    //        GL.Color3(Color.MidnightBlue);
    //        GL.Vertex2(-1.0f, 1.0f);
    //        GL.Color3(Color.SpringGreen);
    //        GL.Vertex2(0.0f, -1.0f);
    //        GL.Color3(Color.Ivory);
    //        GL.Vertex2(1.0f, 1.0f);

    //        GL.End();

    //        game.SwapBuffers();
    //    }
    //}
}
