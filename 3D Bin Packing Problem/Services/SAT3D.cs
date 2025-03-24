using System;
using System.Numerics;

namespace _3D_Bin_Packing_Problem.Services
{
    /// <summary>
    /// Provides methods for detecting collisions in 3D rectangular cuboids using the Separating Axis Theorem (SAT).
    /// </summary>
    public class SAT3D
    {
        /// <summary>
        /// Determines if two 3D rectangular cuboids are colliding.
        /// </summary>
        /// <param name="cuboidA">The vertices of the first cuboid.</param>
        /// <param name="cuboidB">The vertices of the second cuboid.</param>
        /// <returns>True if the cuboids are colliding, otherwise false.</returns>
        public static bool IsColliding(Vector3[] cuboidA, Vector3[] cuboidB)
        {
            // Define the main axes for 3D rectangular cuboids: X, Y, and Z axes
            Vector3[] axes = new Vector3[] { Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ };

            // Check for collision on each axis (X, Y, Z)
            foreach (Vector3 axis in axes)
            {
                if (!OverlapOnAxis(cuboidA, cuboidB, axis))
                    return false; // No collision
            }

            return true; // Collision detected on all axes
        }

        /// <summary>
        /// Checks if two 3D cuboids overlap on a specific axis.
        /// </summary>
        /// <param name="cuboidA">The vertices of the first cuboid.</param>
        /// <param name="cuboidB">The vertices of the second cuboid.</param>
        /// <param name="axis">The axis to check for overlap.</param>
        /// <returns>True if the cuboids overlap on the axis, otherwise false.</returns>
        private static bool OverlapOnAxis(Vector3[] cuboidA, Vector3[] cuboidB, Vector3 axis)
        {
            bool samePointReachmentIsOverlapping = false;

            (float minA, float maxA) = ProjectCuboid(cuboidA, axis);
            (float minB, float maxB) = ProjectCuboid(cuboidB, axis);

            if (samePointReachmentIsOverlapping)
            {
                return !(maxB <= minA || maxA <= minB); // No overlap
            }
            else
            {
                return !(maxB < minA || maxA < minB); // No overlap
            }

        }

        /// <summary>
        /// Projects a cuboid onto an axis.
        /// </summary>
        /// <param name="cuboid">The vertices of the cuboid.</param>
        /// <param name="axis">The axis to project onto.</param>
        /// <returns>The minimum and maximum values of the projection.</returns>
        private static (float, float) ProjectCuboid(Vector3[] cuboid, Vector3 axis)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (Vector3 point in cuboid)
            {
                float projection = Vector3.Dot(point, axis);
                if (projection < min) min = projection;
                if (projection > max) max = projection;
            }

            return (min, max);
        }

        public static void Main()
        {
            Vector3[] cuboid1 = new Vector3[]
            {
                new(0,0,0), new(2,0,0), new(0,2,0),
                new(2,0,0), new(2,2,0), new(0,2,0),
                new(0,0,2), new(2,0,2), new(0,2,2),
                new(2,0,2), new(2,2,2), new(0,2,2),
                new(0,0,0), new(0,2,0), new(0,0,2),
                new(0,2,0), new(0,2,2), new(0,0,2),
                new(2,0,0), new(2,2,0), new(2,0,2),
                new(2,2,0), new(2,2,2), new(2,0,2),
            };

            Vector3[] cuboid2 = new Vector3[]
            {
                new(2,2,2), new(4,2,2), new(2,4,2),
                new(4,2,2), new(4,4,2), new(2,4,2),
                new(2,2,4), new(4,2,4), new(2,4,4),
                new(4,2,4), new(4,4,4), new(2,4,4),
                new(2,2,2), new(2,4,2), new(2,2,4),
                new(2,4,2), new(2,4,4), new(2,2,4),
                new(4,2,2), new(4,4,2), new(4,2,4),
                new(4,4,2), new(4,4,4), new(4,2,4),
            };

            Console.WriteLine(IsColliding(cuboid1, cuboid2)); // Check for collision
        }
    }
}
