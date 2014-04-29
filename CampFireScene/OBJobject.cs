using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace CampFireScene
{

    public class OBJobject
    {
        public int vertexBufferHandle{get; set;}
        public int vertexTexturBufferHandle { get; set; }
        public int vertexNormalBufferHandle { get; set; }
        public int imageTextureHandle { get; set; }
        public List<float> Vertices;
        public List<float> uvs;
        public List<float> normals;
        public List<faces> faces;
        public int[] indicies;
        public bool vertexAndTextureCoordinates;
        public bool vertexTextureCoordinatesAndNormals;
        public int triangleCount;
        public void VTC(bool val)
        {
            vertexAndTextureCoordinates = val;
        }

        public void VTCN(bool val)
        {
            vertexTextureCoordinatesAndNormals = val;
        }

        public OBJobject()
        {
            Vertices = new List<float>();
            uvs = new List<float>();
            normals = new List<float>();
            faces = new List<faces>();
            imageTextureHandle = -1;
            vertexAndTextureCoordinates = false;
            vertexTextureCoordinatesAndNormals = false;
            triangleCount = 0;
        }
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

        public float[] cube = {
			-0.5f,  0.5f,  0.5f, // vertex[0]
			 0.5f,  0.5f,  0.5f, // vertex[1]
			 0.5f, -0.5f,  0.5f, // vertex[2]
			-0.5f, -0.5f,  0.5f, // vertex[3]
			-0.5f,  0.5f, -0.5f, // vertex[4]
			 0.5f,  0.5f, -0.5f, // vertex[5]
			 0.5f, -0.5f, -0.5f, // vertex[6]
			-0.5f, -0.5f, -0.5f, // vertex[7]
		};
        public void Render()
        {
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref cameraController.ProjectionMatrix);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.VertexPointer(3, VertexPointerType.Float, 0, cube);
            GL.ColorPointer(4, ColorPointerType.Float, 0, cubeColors);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedByte, triangles);
        
            //GL.EnableVertexAttribArray(vertexBufferHandle);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            //GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
            //if (vertexAndTextureCoordinates)
            //{
            //    GL.EnableVertexAttribArray(vertexTexturBufferHandle);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexTexturBufferHandle);
            //    GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);
            //}
            //else if (vertexTextureCoordinatesAndNormals)
            //{
            //    GL.EnableVertexAttribArray(vertexTexturBufferHandle);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexTexturBufferHandle);
            //    GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);
            //    GL.EnableVertexAttribArray(vertexNormalBufferHandle);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexNormalBufferHandle);
            //    GL.NormalPointer(NormalPointerType.Float, 0, 0);
            //}
            //if (imageTextureHandle != -1)
            //    GL.BindTexture(TextureTarget.Texture2D, imageTextureHandle);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, triangleCount * 3);
            //GL.DisableVertexAttribArray(vertexBufferHandle);
            //GL.DisableVertexAttribArray(vertexTexturBufferHandle);
            //GL.DisableVertexAttribArray(vertexNormalBufferHandle);
        }

    }
}
