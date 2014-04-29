using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.IO;

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

        public void Load()
        {
            int faceCount = faces.Count;
            float[] vertexBufferArray = null;
            float[] normalBufferArray = null;
            float[] vertexTextureBufferArray = null;
            if (vertexAndTextureCoordinates)
            {
                vertexBufferArray = new float[(faceCount) * 9];
                vertexTextureBufferArray = new float[faceCount * 6];
                for (int j = 0; j < faceCount; j++)
                {
                    int currentVert = faces[j].VertexIndex1;
                    vertexBufferArray[j * 9] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 1] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 2] = Vertices[currentVert * 3 - 1];
                    currentVert = faces[j].VertexIndex2;
                    vertexBufferArray[j * 9 + 3] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 4] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 5] = Vertices[currentVert * 3 - 1];
                    currentVert = faces[j].VertexIndex3;
                    vertexBufferArray[j * 9 + 6] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 7] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 8] = Vertices[currentVert * 3 - 1];
                    int currentTexture = faces[j].textureIndex1;
                    vertexTextureBufferArray[j * 9] = uvs[currentTexture * 2 - 2];
                    vertexTextureBufferArray[j * 9 + 1] = uvs[currentTexture * 2 - 1];
                    currentTexture = faces[j].textureIndex2;
                    vertexTextureBufferArray[j * 9 + 2] = uvs[currentTexture * 2 - 2];
                    vertexTextureBufferArray[j * 9 + 3] = uvs[currentTexture * 2 - 1];
                    currentTexture = faces[j].textureIndex3;
                    vertexTextureBufferArray[j * 9 + 4] = uvs[currentTexture * 2 - 2];
                    vertexTextureBufferArray[j * 9 + 5] = uvs[currentTexture * 2 - 1];
                }

            }
            else if (vertexTextureCoordinatesAndNormals)
            {
                vertexBufferArray = new float[faceCount * 9];
                vertexTextureBufferArray = new float[(faceCount) * 6];
                normalBufferArray = new float[faceCount * 9];
                for (int j = 0; j < faceCount; j++)
                {
                    int currentVert = faces[j].VertexIndex1;
                    vertexBufferArray[j * 9] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 1] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 2] = Vertices[currentVert * 3 - 1];
                    currentVert = faces[j].VertexIndex2;
                    vertexBufferArray[j * 9 + 3] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 4] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 5] = Vertices[currentVert * 3 - 1];
                    currentVert = faces[j].VertexIndex3;
                    vertexBufferArray[j * 9 + 6] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 7] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 8] = Vertices[currentVert * 3 - 1];
                    int currentTexture = faces[j].textureIndex1;
                    vertexTextureBufferArray[j * 6] = uvs[currentTexture * 2 - 2];
                    vertexTextureBufferArray[j * 6 + 1] = uvs[currentTexture * 2 - 1];
                    currentTexture = faces[j].textureIndex2;
                    vertexTextureBufferArray[j * 6 + 2] = uvs[currentTexture * 2 - 2];
                    vertexTextureBufferArray[j * 6 + 3] = uvs[currentTexture * 2 - 1];
                    currentTexture = faces[j].textureIndex3;
                    vertexTextureBufferArray[j * 6 + 4] = uvs[currentTexture * 2 - 2];
                    vertexTextureBufferArray[j * 6 + 5] = uvs[currentTexture * 2 - 1];
                    int currentNormal = faces[j].normalIndex1;
                    normalBufferArray[j * 9] = normals[currentNormal * 3 - 3];
                    normalBufferArray[j * 9 + 1] = normals[currentNormal * 3 - 3];
                    normalBufferArray[j * 9 + 2] = normals[currentNormal * 3 - 3];
                    currentNormal = faces[j].normalIndex2;
                    normalBufferArray[j * 9 + 3] = normals[currentNormal * 3 - 3];
                    normalBufferArray[j * 9 + 4] = normals[currentNormal * 3 - 3];
                    normalBufferArray[j * 9 + 5] = normals[currentNormal * 3 - 3];
                    currentNormal = faces[j].normalIndex3;
                    normalBufferArray[j * 9 + 6] = normals[currentNormal * 3 - 3];
                    normalBufferArray[j * 9 + 7] = normals[currentNormal * 3 - 3];
                    normalBufferArray[j * 9 + 8] = normals[currentNormal * 3 - 3];

                }
            }
            else
            {
                vertexBufferArray = new float[faceCount * 9];
                for (int j = 0; j < faceCount; j++)
                {
                    int currentVert = faces[j].VertexIndex1;
                    vertexBufferArray[j * 9] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 1] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 2] = Vertices[currentVert * 3 - 1];
                    currentVert = faces[j].VertexIndex2;
                    vertexBufferArray[j * 9 + 3] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 4] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 5] = Vertices[currentVert * 3 - 1];
                    currentVert = faces[j].VertexIndex3;
                    vertexBufferArray[j * 9 + 6] = Vertices[currentVert * 3 - 3];
                    vertexBufferArray[j * 9 + 7] = Vertices[currentVert * 3 - 2];
                    vertexBufferArray[j * 9 + 8] = Vertices[currentVert * 3 - 1];
                }
            }
#if DEBUG
            if (File.Exists(@"DEBUG"))
                File.Delete(@"DEBUG");
            StreamWriter w = new StreamWriter(File.Open(@"DEBUG", FileMode.OpenOrCreate));

            for (int a = 0; a < vertexBufferArray.Length; a += 3)
                w.WriteLine("v {0} {1} {2}", vertexBufferArray[a], vertexBufferArray[a + 1], vertexBufferArray[a + 2]);

            for (int a = 0; a < vertexTextureBufferArray.Length; a += 2)
                w.WriteLine("vt {0} {1}", vertexTextureBufferArray[a], vertexTextureBufferArray[a + 1]);

            for (int a = 0; a < normalBufferArray.Length; a += 3)
                w.WriteLine("vn {0} {1} {2}", normalBufferArray[a], normalBufferArray[a + 1], normalBufferArray[a + 2]);

            w.Flush();
            w.Close();
#endif
            if (vertexAndTextureCoordinates)
            {
                triangleCount = faceCount;
                vertexBufferHandle = GL.GenBuffer();
                vertexTexturBufferHandle = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferArray.Length * sizeof(float)), vertexBufferArray, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexTexturBufferHandle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexTextureBufferArray.Length * sizeof(float)), vertexTextureBufferArray, BufferUsageHint.StaticDraw);
            }
            else if (vertexTextureCoordinatesAndNormals)
            {
                triangleCount = faceCount;
                vertexBufferHandle = GL.GenBuffer();
                vertexTexturBufferHandle = GL.GenBuffer();
                vertexNormalBufferHandle = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferArray.Length * sizeof(float)), vertexBufferArray, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexTexturBufferHandle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexTextureBufferArray.Length * sizeof(float)), vertexTextureBufferArray, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexNormalBufferHandle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalBufferArray.Length * sizeof(float)), normalBufferArray, BufferUsageHint.StaticDraw);
            }
            else
            {
                triangleCount = faceCount;
                vertexBufferHandle = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferArray.Length * sizeof(float)), vertexBufferArray, BufferUsageHint.StaticDraw);
            }
        }

        public void Render()
        {
            GL.EnableVertexAttribArray(vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
            if (vertexAndTextureCoordinates)
            {
                GL.EnableVertexAttribArray(vertexTexturBufferHandle);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexTexturBufferHandle);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);
            }
            else if (vertexTextureCoordinatesAndNormals)
            {
                GL.EnableVertexAttribArray(vertexTexturBufferHandle);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexTexturBufferHandle);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);
                GL.EnableVertexAttribArray(vertexNormalBufferHandle);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexNormalBufferHandle);
                GL.NormalPointer(NormalPointerType.Float, 0, 0);
            }
            if (imageTextureHandle != -1)
                GL.BindTexture(TextureTarget.Texture2D, imageTextureHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangleCount * 3);
            GL.DisableVertexAttribArray(vertexBufferHandle);
            GL.DisableVertexAttribArray(vertexTexturBufferHandle);
            GL.DisableVertexAttribArray(vertexNormalBufferHandle);
        }
    }
}
