using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Bin_Packing_Problem.Comparer
{
    public class Vector3EqualityComparer : IEqualityComparer<Vector3>
    {
        private const float Epsilon = 1e-5f;

        public bool Equals(Vector3 v1, Vector3 v2)
        {
            return Math.Abs(v1.X - v2.X) < Epsilon &&
                   Math.Abs(v1.Y - v2.Y) < Epsilon &&
                   Math.Abs(v1.Z - v2.Z) < Epsilon;
        }

        public int GetHashCode(Vector3 v)
        {
            return v.X.GetHashCode() ^ v.Y.GetHashCode() ^ v.Z.GetHashCode();
        }
    }
}
