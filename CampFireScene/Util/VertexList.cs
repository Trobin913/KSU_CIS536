using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampFireScene.Util
{
    public class VertexList : List<float>
    {
        public void AddVertex(float x, float y, float z)
        {
            this.Add(x);
            this.Add(y);
            this.Add(z);
        }

        public void AddVertex(Vector3 vertex)
        {
            AddVertex(vertex.X, vertex.Y, vertex.Z);
        }

        public void RemoveVertex(int index)
        {
            this.RemoveAt(index);
            this.RemoveAt(index);
            this.RemoveAt(index);
        }

        public Vector3 GetVertex(int index)
        {
            int startIndex = index * 3;
            if (startIndex >= this.Count
                || startIndex + 1 >= this.Count
                || startIndex + 2 >= this.Count)
            {
                throw new ArgumentOutOfRangeException("index", index, "Cannot access vertex with given index");
            }

            return new Vector3(
                this[startIndex],
                this[startIndex + 1],
                this[startIndex + 2]
                );
        }

        public int GetVertexCount()
        {
            return this.Count / 3;
        }
    }
}
