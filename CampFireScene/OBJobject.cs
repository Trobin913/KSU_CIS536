using CampFireScene.Util;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;

namespace CampFireScene
{
    public class OBJobject
    {
        public int vertexBufferHandle { get; set; }

        public int vertexTexturBufferHandle { get; set; }

        public int vertexNormalBufferHandle { get; set; }

        public int imageTextureHandle { get; set; }

        public List<float> Vertices;
        public List<float> uvs;
        public List<float> normals;
        public List<faces> faces;
        public int shadersID;

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

            if (vertexAndTextureCoordinates || vertexTextureCoordinatesAndNormals)
            {
                for (int a = 0; a < vertexTextureBufferArray.Length; a += 2)
                    w.WriteLine("vt {0} {1}", vertexTextureBufferArray[a], vertexTextureBufferArray[a + 1]);
            }

            if (vertexTextureCoordinatesAndNormals)
            {
                for (int a = 0; a < normalBufferArray.Length; a += 3)
                    w.WriteLine("vn {0} {1} {2}", normalBufferArray[a], normalBufferArray[a + 1], normalBufferArray[a + 2]);
            }

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
                //GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferArray.Length * sizeof(float)), vertexBufferArray, BufferUsageHint.StaticDraw);
            }
        }

        public void Render()
        {
            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            //GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
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

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangleCount * 3);
            GL.PopClientAttrib();
        }

        private float[] cubeColors;

        public void RenderImediate()
        {
            GL.VertexPointer(3, VertexPointerType.Float, 0, Vertices.ToArray());

            if (cubeColors == null)
            {
                Random random = new Random();
                cubeColors = new float[faces.Count * 3];
                for (int i = 0; i < cubeColors.Length; i++)
                {
                    cubeColors[i] = (float)random.NextDouble();
                }
            }

            GL.ColorPointer(4, ColorPointerType.Float, 0, cubeColors);
            int[] triangles = new int[faces.Count * 3];
            for (int i = 0; i < faces.Count; i++)
            {
                triangles[i * 3] = (int)(faces[i].VertexIndex1 - 1);
                triangles[i * 3 + 1] = (int)(faces[i].VertexIndex2 - 1);
                triangles[i * 3 + 2] = (int)(faces[i].VertexIndex3 - 1);
            }

            GL.DrawElements(PrimitiveType.Triangles, triangles.Length, DrawElementsType.UnsignedInt, triangles);
        }
    }

    public class GLObject
    {
        /// <summary>
        /// The raw verticies making up the object.
        /// </summary>
        public Vector3List Verticies { get; private set; }

        /// <summary>
        /// The uv coordinates for the object.
        /// </summary>
        public Vector2List UVs { get; private set; }

        /// <summary>
        /// Vertex normals for the object.
        /// </summary>
        public Vector3List Normals { get; private set; }

        /// <summary>
        /// Faces defining the object.
        /// </summary>
        public List<faces> Faces { get; private set; }

        private int vertexBufferHandle;
        private int vertexTexturBufferHandle;
        private int vertexNormalBufferHandle;
        private int imageTextureHandle;

        public GLObject(float[] verticies, float[] uvs, float[] normals, faces[] faces)
        {
            Verticies = new Vector3List(verticies);
            UVs = new Vector2List(uvs);
            Normals = new Vector3List(normals);

            vertexBufferHandle = -1;
            vertexNormalBufferHandle = -1;
            vertexTexturBufferHandle = -1;
            imageTextureHandle = -1;
        }

        /// <summary>
        /// Loads the object onto the graphics card. This must be called before using Render()
        /// </summary>
        public void Load()
        {
            Vector3List vertexBufferArray = new Vector3List();

            foreach (faces face in Faces)
            {
                vertexBufferArray.Add(
                    face.VertexIndex1,
                    face.VertexIndex2,
                    face.VertexIndex3
                    );
            }

            vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferArray.Count * sizeof(float)), vertexBufferArray.ToArray(), BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Renders the object.
        /// </summary>
        public void Render()
        {
        }

        /// <summary>
        /// Renders the object imediately.
        /// </summary>
        public void RenderImediate()
        {
            GL.VertexPointer(3, VertexPointerType.Float, 0, Verticies.ToArray());

            float[] colors = new float[Faces.Count * 3];
            Random random = new Random(12345);
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = (float)random.NextDouble();
            }
            GL.ColorPointer(4, ColorPointerType.Float, 0, colors);

            int[] triangles = new int[Faces.Count * 3];
            for (int i = 0; i < Faces.Count; i++)
            {
                triangles[i * 3] = (int)(Faces[i].VertexIndex1 - 1);
                triangles[i * 3 + 1] = (int)(Faces[i].VertexIndex2 - 1);
                triangles[i * 3 + 2] = (int)(Faces[i].VertexIndex3 - 1);
            }

            //GL.DrawElements(PrimitiveType.Triangles, triangles.Length, DrawElementsType.UnsignedInt, triangles);
            GL.DrawArrays(PrimitiveType.Triangles, 0, triangles.Length);
        }
    }
}