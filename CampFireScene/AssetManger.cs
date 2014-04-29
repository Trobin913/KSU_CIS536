using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace CampFireScene
{
    public class AssetManger
    {
        private string _assetDirectory;

        public AssetManger(string assetDirectory)
        {
            _assetDirectory = assetDirectory;
        }

        public static List<OBJobject> LoadAssets()
        {
            //Load in all assets from the disk here.
            List<string> filesToBeParced = new List<string>();
            List<OBJobject> parcedFiles;
            filesToBeParced.Add(@"Objects\cube.obj");
            parcedFiles = ParceFiles(filesToBeParced);
            //parcedFiles[0].imageTextureHandle = loadImage(@"Images\water.jpg");
            return parcedFiles;
        }

        private static List<OBJobject> ParceFiles(List<String> files)
        {
            StreamReader sr;
            string line;
            OBJobject OBJ;
            int plyCount;
            Tuple<float, float, float> vec3;
            Tuple<float, float> vec2;
            List<OBJobject> listOfOBJS = new List<OBJobject>();
            for (int i = 0; i < files.Count; i++)
            {
                plyCount = 0;
                sr = new StreamReader(files[i]);
                OBJ = new OBJobject();
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    string[] str = line.Split(' ');
                    switch (str[0])
                    {
                        case "v":

                            for (int j = 1; j <= 3; j++)
                            {
                                OBJ.Vertices.Add(float.Parse(str[j]));
                            }
                            break;

                        case "vt":

                            for (int j = 1; j <= 2; j++)
                            {
                                OBJ.uvs.Add(float.Parse(str[j]));
                            }
                            break;

                        case "vn":

                            for (int j = 1; j <= 3; j++)
                            {
                                OBJ.normals.Add(float.Parse(str[j]));
                            }
                            break;

                        case "f":
                            

                            for (int j = 1; j < str.Length; j++)
                            {
                                string[] subStr = str[j].Split('/');
                                if (subStr.Length == 1);
                                else if (subStr.Length == 2) OBJ.VTC(true);
                                else if (subStr.Length == 3) OBJ.VTCN(true);
                                else throw new Exception("OBJ File is corrupted");
                                OBJ.faces.Add(int.Parse(subStr[j-1]));
                            }
                            break;
                        
                        default:
                            break;

                    }
                }

                int faceCount = OBJ.faces.Count;
                float[] vertexBufferArray = null;
                float[] normalBufferArray = null;
                float[] vertexTextureBufferArray = null;
                if(OBJ.vertexAndTextureCoordinates)
                {
                    vertexBufferArray = new float[(faceCount/2)*3];
                    vertexTextureBufferArray = new float[faceCount];
                    for (int j = 0; j < faceCount; j+=2)
                    {
                        int currentVert = OBJ.faces[j];
                        vertexBufferArray[j] = OBJ.Vertices[currentVert*3-3];
                        vertexBufferArray[j + 1] = OBJ.Vertices[currentVert*3-2];
                        vertexBufferArray[j + 2] = OBJ.Vertices[currentVert*3-1];
                        
                        int currentTexture = OBJ.faces[j+1];
                        vertexTextureBufferArray[j] = OBJ.uvs[currentTexture*2-2];
                        vertexTextureBufferArray[j+1] = OBJ.uvs[currentTexture*2-1];
                    }

                }
                else if(OBJ.vertexTextureCoordinatesAndNormals)
                {
                    vertexBufferArray = new float[faceCount];
                    vertexTextureBufferArray = new float[(faceCount/3)*2];
                    normalBufferArray = new float[faceCount];
                    for (int j = 0; j < faceCount; j+=3)
                    {
                        int currentVert = OBJ.faces[j];
                        vertexBufferArray[j] = OBJ.Vertices[currentVert*3-3];
                        vertexBufferArray[j + 1] = OBJ.Vertices[currentVert*3-2];
                        vertexBufferArray[j + 2] = OBJ.Vertices[currentVert*3-1];
                        
                        int currentTexture = OBJ.faces[j + 1];
                        int offset = j / 3;
                        vertexTextureBufferArray[j-offset] = OBJ.uvs[currentTexture * 2 - 2];
                        vertexTextureBufferArray[j-offset + 1] = OBJ.uvs[currentTexture * 2 - 1];
                        
                        int currentNormal = OBJ.faces[j + 2];
                        normalBufferArray[j] = OBJ.normals[currentNormal * 3 - 3];
                        normalBufferArray[j + 1] = OBJ.normals[currentNormal * 3 - 2];
                        normalBufferArray[j + 2] = OBJ.normals[currentNormal * 3 - 1];
                    }
                }
                else
                {
                    vertexBufferArray = new float[faceCount*3];
                    for (int j = 0; j < faceCount; j++)
                    {
                        int currentVert = OBJ.faces[j];
                        vertexBufferArray[j] = OBJ.Vertices[currentVert*3-3];
                        vertexBufferArray[j + 1] = OBJ.Vertices[currentVert*3-2];
                        vertexBufferArray[j + 2] = OBJ.Vertices[currentVert*3-1];
                    }
                }

#if DEBUG
                if(File.Exists(@"DEBUG"))
                    File.Delete(@"DEBUG");
                StreamWriter w = new StreamWriter(File.Open(@"DEBUG", FileMode.OpenOrCreate));

                for(int a = 0; a < vertexBufferArray.Length; a += 3)
                    w.WriteLine("v {0} {1} {2}", vertexBufferArray[a], vertexBufferArray[a+1], vertexBufferArray[a+2]);

                for(int a = 0; a < vertexTextureBufferArray.Length; a += 2)
                    w.WriteLine("vt {0} {1}", vertexTextureBufferArray[a], vertexTextureBufferArray[a+1]);

                for(int a = 0; a < normalBufferArray.Length; a += 3)
                    w.WriteLine("vn {0} {1} {2}", normalBufferArray[a], normalBufferArray[a+1], normalBufferArray[a+2]);

                w.Flush();
                w.Close();
#endif

                if(OBJ.vertexAndTextureCoordinates)
                {
                    OBJ.triangleCount = faceCount / 6;
                    OBJ.vertexBufferHandle = GL.GenBuffer();
                    OBJ.vertexTexturBufferHandle = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, OBJ.vertexBufferHandle);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferArray.Length * sizeof(float)), vertexBufferArray, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, OBJ.vertexTexturBufferHandle);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexTextureBufferArray.Length * sizeof(float)), vertexTextureBufferArray, BufferUsageHint.StaticDraw);
                }
                else if(OBJ.vertexTextureCoordinatesAndNormals)
                {
                    OBJ.triangleCount = faceCount / 9;
                    OBJ.vertexBufferHandle = GL.GenBuffer();
                    OBJ.vertexTexturBufferHandle = GL.GenBuffer();
                    OBJ.vertexNormalBufferHandle = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, OBJ.vertexBufferHandle);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferArray.Length * sizeof(float)), vertexBufferArray, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, OBJ.vertexTexturBufferHandle);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexTextureBufferArray.Length * sizeof(float)), vertexTextureBufferArray, BufferUsageHint.StaticDraw);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, OBJ.vertexNormalBufferHandle);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalBufferArray.Length * sizeof(float)), normalBufferArray, BufferUsageHint.StaticDraw);
                }
                else
                {
                    OBJ.triangleCount = faceCount / 3;
                    OBJ.vertexBufferHandle = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, OBJ.vertexBufferHandle);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferArray.Length * sizeof(float)), vertexBufferArray, BufferUsageHint.StaticDraw);
                }
                listOfOBJS.Add(OBJ);
            }

            return listOfOBJS;
        }

        private static int loadImage(string filePath)
        {
            Bitmap picture = new Bitmap(Image.FromFile(filePath));
            System.Drawing.Imaging.BitmapData data = picture.LockBits(new System.Drawing.Rectangle(0, 0, picture.Width, picture.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int textureHandle;
            GL.Enable(EnableCap.Texture2D);
            textureHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            picture.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return textureHandle;
        }
    }
}
