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
