using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampFireScene.Util
{
    public class Vector3List : List<float>
    {
        public Vector3List()
            : base()
        { }

        public Vector3List(int size)
            : base(size * 3)
        { }

        public Vector3List(IEnumerable<float> c)
            : base(c)
        { }

        public void Add(float x, float y, float z)
        {
            this.Add(x);
            this.Add(y);
            this.Add(z);
        }

        public void Add(Vector3 vertex)
        {
            Add(vertex.X, vertex.Y, vertex.Z);
        }

        public void RemoveVector(int index)
        {
            this.RemoveAt(index);
            this.RemoveAt(index);
            this.RemoveAt(index);
        }

        public Vector3 GetVector(int index)
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

        public int GetVectorCount()
        {
            return this.Count / 3;
        }
    }
}
