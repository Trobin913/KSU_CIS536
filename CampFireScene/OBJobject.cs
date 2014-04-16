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
        public List<int> faces;
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
            faces = new List<int>();
            imageTextureHandle = -1;
            vertexAndTextureCoordinates = false;
            vertexTextureCoordinatesAndNormals = false;
            triangleCount = 0;
        }
        public void Render()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexNormalBufferHandle);
            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
            if (vertexAndTextureCoordinates)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexTexturBufferHandle);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);
            }
            else if (vertexTextureCoordinatesAndNormals)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexTexturBufferHandle);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexNormalBufferHandle);
                GL.NormalPointer(NormalPointerType.Float, 0, 0);
            }
            if (imageTextureHandle != -1)
                GL.BindTexture(TextureTarget.Texture2D, imageTextureHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangleCount * 3);
        }
    }
}
