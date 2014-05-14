using OpenTK;
using System;
using System.Collections.Generic;

namespace CampFireScene.Util
{
    public class Vector2List : List<float>
    {
        public Vector2List()
            : base()
        { }

        public Vector2List(int size)
            : base(size * 2)
        { }

        public Vector2List(IEnumerable<float> c)
            : base(c)
        { }

        public void Add(float x, float y)
        {
            this.Add(x);
            this.Add(y);
        }

        public void Add(Vector2 vertex)
        {
            Add(vertex.X, vertex.Y);
        }

        public Vector2 GetVector(int index)
        {
            int startIndex = index * 3;
            if (startIndex >= this.Count
                || startIndex + 1 >= this.Count)
            {
                throw new ArgumentOutOfRangeException("index", index, "Cannot access vertex with given index");
            }

            return new Vector2(
                this[startIndex],
                this[startIndex + 1]
                );
        }

        public int GetVectorCount()
        {
            return this.Count / 2;
        }

        public void RemoveVector(int index)
        {
            this.RemoveAt(index);
            this.RemoveAt(index);
        }
    }
}