using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace BadAddons.Common
{
    // I would NEVER over-engineer my circle struct with functions i will never use!
    /// <summary>
    /// Primitive circle struct
    /// </summary>
    public struct Circle
    {
        public static readonly Circle Empty;

        private Vector2 center;
        private float radius;

        /// <summary>
        /// The center position of this circle
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        /// <summary>
        /// Distance from <see cref="Center"/> to the edge
        /// </summary>
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        /// <summary>
        /// The total perimeter around the circle
        /// </summary>
        public float Circumference => MathF.Tau * radius; // Tau = 2pi

        /// <summary>
        /// Total area of the circle
        /// </summary>
        public float Area => MathF.PI * radius * radius;

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Gets a unit tangent vector at a given angle on the circle.
        /// </summary>
        /// <param name="angle">The angle from the <see cref="center"/> in radians</param>
        /// <param name="clockwise">If the tangent should be rotated clockwise or counterclockwise</param>
        /// <returns>[Position of tangent, Direction of tangent]</returns>
        public Vector2[] Tangent(float angle, bool clockwise = true)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            // Compute position directly
            Vector2 offset = new Vector2(cos, sin) * radius;
            Vector2 position = center + offset;

            // Compute tangent direction
            Vector2 direction = clockwise ? new Vector2(sin, -cos).RotatedBy(MathHelper.PiOver2) : new Vector2(-sin, cos).RotatedBy(-MathHelper.PiOver2);

            return [position, direction];
        }

        /// <summary>
        /// Returns true when a given position lies inside the circle
        /// </summary>
        public bool Contains(Vector2 position)
        {
            return Contains(position.X, position.Y);
        }

        /// <summary>
        /// Returns true when a given position lies inside the circle
        /// </summary>
        public bool Contains(float x, float y)
        {
            float dx = x - center.X;
            float dy = y - center.Y;
            return (dx * dx + dy * dy) < radius * radius;
        }

        /// <summary>
        /// Gets the closest position on the circumference of the circle to another position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 ClosestPointOnEdge(Vector2 position)
        {
            float dx = position.X - center.X;
            float dy = position.Y - center.Y;
            float invLen = MathF.ReciprocalSqrtEstimate(dx * dx + dy * dy); // Fast inverse sqrt, like in that one video, you know the one
            return center + new Vector2(dx * invLen, dy * invLen) * radius;
        }

        /// <summary>
        /// Returns true when the line between two points intersects this circle
        /// </summary>
        /// <param name="a">point a</param>
        /// <param name="b">point b</param>
        public bool IntersectsLine(Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            Vector2 ac = center - a;

            float abLenSq = ab.X * ab.X + ab.Y * ab.Y; // LengthSquared manually
            float t = MathF.Max(0, MathF.Min(1, (ac.X * ab.X + ac.Y * ab.Y) / abLenSq));

            float closestX = a.X + t * ab.X;
            float closestY = a.Y + t * ab.Y;

            float dx = closestX - center.X;
            float dy = closestY - center.Y;

            return (dx * dx + dy * dy) <= (radius * radius);
        }

        /// <summary>
        /// Returns true when this circle intersects the given circle
        /// </summary>
        public bool IntersectsCircle(Circle other)
        {
            float combinedRadius = radius + other.radius;
            return Vector2.DistanceSquared(center, other.center) <= combinedRadius * combinedRadius;
        }

        /// <summary>
        /// Gets a uniformly distributed position within the bounds of the circle <br/>
        /// </summary>
        public Vector2 RandomPointInside()
        {
            // Rejection sampling. Somehow the most optimal choice
            float x;
            float y;
            do
            {
                x = center.X + Main.rand.NextFloat(-radius, radius);
                y = center.Y + Main.rand.NextFloat(-radius, radius);
                if (Contains(x,y))
                {
                    return new Vector2(x,y);
                }
            }
            while (true);
        }
    }
}
