using _3D_Bin_Packing_Problem.Comparer;
using System.Collections.Generic;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Services
{
    /// <summary>
    /// Provides methods for detecting collisions and point containment in 3D objects using the Separating Axis Theorem (SAT).
    /// </summary>
    public class SAT3D
    {
        /// <summary>
        /// Determines if two 3D objects are colliding.
        /// </summary>
        /// <param name="polyA">The vertices of the first 3D object.</param>
        /// <param name="polyB">The vertices of the second 3D object.</param>
        /// <returns>True if the objects are colliding, otherwise false.</returns>
        public static bool IsColliding(Vector3[] polyA, Vector3[] polyB)
        {
            List<Vector3> axes = new List<Vector3>();

            // Extract axes from the face normals of both objects
            axes.AddRange(GetFaceNormals(polyA));
            axes.AddRange(GetFaceNormals(polyB));

            // Also consider the cross products of the edges of both objects as axes
            axes.AddRange(GetEdgeCrossProducts(polyA, polyB));

            // Check for collision on each axis
            foreach (Vector3 axis in axes)
            {
                if (axis.LengthSquared() < 1e-6) continue; // Skip axes with zero length

                if (!OverlapOnAxis(polyA, polyB, axis))
                    return false; // No collision
            }

            return true; // Collision detected on all axes
        }

        /// <summary>
        /// Gets the face normals for a 3D object.
        /// </summary>
        /// <param name="poly">The vertices of the 3D object.</param>
        /// <returns>A list of face normals.</returns>
        private static List<Vector3> GetFaceNormals(Vector3[] poly)
        {
            HashSet<Vector3> uniqueNormals = new HashSet<Vector3>(new Vector3EqualityComparer());

            for (int i = 0; i < poly.Length; i += 3)
            {
                // Select a triangular face
                Vector3 p1 = poly[i];
                Vector3 p2 = poly[(i + 1) % poly.Length];
                Vector3 p3 = poly[(i + 2) % poly.Length];

                // Calculate the face normal
                Vector3 edge1 = p2 - p1;
                Vector3 edge2 = p3 - p1;
                Vector3 normal = Vector3.Cross(edge1, edge2);
                normal = Vector3.Normalize(normal);

                uniqueNormals.Add(normal);
            }
            return [.. uniqueNormals];

        }

        /// <summary>
        /// Gets the axes from the cross products of the edges of two 3D objects.
        /// </summary>
        /// <param name="polyA">The vertices of the first 3D object.</param>
        /// <param name="polyB">The vertices of the second 3D object.</param>
        /// <returns>A list of axes.</returns>
        private static List<Vector3> GetEdgeCrossProducts(Vector3[] polyA, Vector3[] polyB)
        {
            List<Vector3> axes = new List<Vector3>();

            for (int i = 0; i < polyA.Length; i++)
            {
                Vector3 edgeA = polyA[(i + 1) % polyA.Length] - polyA[i];

                for (int j = 0; j < polyB.Length; j++)
                {
                    Vector3 edgeB = polyB[(j + 1) % polyB.Length] - polyB[j];

                    Vector3 axis = Vector3.Cross(edgeA, edgeB);
                    if (axis.LengthSquared() > 1e-6) // Skip axes with zero length
                        axes.Add(Vector3.Normalize(axis));
                }
            }

            return axes;
        }

        /// <summary>
        /// Checks if two 3D objects overlap on a specific axis.
        /// </summary>
        /// <param name="polyA">The vertices of the first 3D object.</param>
        /// <param name="polyB">The vertices of the second 3D object.</param>
        /// <param name="axis">The axis to check for overlap.</param>
        /// <returns>True if the objects overlap on the axis, otherwise false.</returns>
        private static bool OverlapOnAxis(Vector3[] polyA, Vector3[] polyB, Vector3 axis)
        {
            (float minA, float maxA) = ProjectPolygon(polyA, axis);
            (float minB, float maxB) = ProjectPolygon(polyB, axis);

            return !(maxA < minB || maxB < minA); // No overlap
        }

        /// <summary>
        /// Projects a 3D object onto an axis.
        /// </summary>
        /// <param name="poly">The vertices of the 3D object.</param>
        /// <param name="axis">The axis to project onto.</param>
        /// <returns>The minimum and maximum values of the projection.</returns>
        private static (float, float) ProjectPolygon(Vector3[] poly, Vector3 axis)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (Vector3 point in poly)
            {
                float projection = Vector3.Dot(point, axis);
                if (projection < min) min = projection;
                if (projection > max) max = projection;
            }

            return (min, max);
        }

        public static void Main()
        {
            Vector3[] poly1 = new Vector3[]
            {
                   // Face 1 (Bottom)
                   new(0,0,0), new(2,0,0), new(0,2,0),
                   new(2,0,0), new(2,2,0), new(0,2,0),

                   // Face 2 (Top)
                   new(0,0,2), new(2,0,2), new(0,2,2),
                   new(2,0,2), new(2,2,2), new(0,2,2),

                   // Face 3 (Front)
                   new(0,0,0), new(2,0,0), new(0,0,2),
                   new(2,0,0), new(2,0,2), new(0,0,2),

                   // Face 4 (Back)
                   new(0,2,0), new(2,2,0), new(0,2,2),
                   new(2,2,0), new(2,2,2), new(0,2,2),

                   // Face 5 (Left)
                   new(0,0,0), new(0,2,0), new(0,0,2),
                   new(0,2,0), new(0,2,2), new(0,0,2),

                   // Face 6 (Right)
                   new(2,0,0), new(2,2,0), new(2,0,2),
                   new(2,2,0), new(2,2,2), new(2,0,2),
            };
            Vector3[] poly2 = new Vector3[]
            {
                    // Face 1 (Bottom)
                   new(2,2,2), new(4,2,2), new(2,4,2),
                   new(4,2,2), new(4,4,2), new(2,4,2),

                   // Face 4 (Top)
                   new(2,2,4), new(4,2,4), new(2,4,4),
                   new(4,2,4), new(4,4,4), new(2,4,4),

                   // Face 3 (Front)
                   new(2,2,2), new(4,2,2), new(2,2,4),
                   new(4,2,2), new(4,2,4), new(2,2,4),

                   // Face 4 (Back)
                   new(2,4,2), new(4,4,2), new(2,4,4),
                   new(4,4,2), new(4,4,4), new(2,4,4),

                   // Face 5 (Left)
                   new(2,2,2), new(2,4,2), new(2,2,4),
                   new(2,4,2), new(2,4,4), new(2,2,4),

                   // Face 6 (Right)
                   new(4,2,2), new(4,4,2), new(4,2,4),
                   new(4,4,2), new(4,4,4), new(4,2,4),
            };
            Console.WriteLine(IsColliding(poly1, poly2));
        }
    }
}
}