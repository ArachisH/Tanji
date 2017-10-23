namespace Sulakore.Habbo
{
    /// <summary>
    /// Represents a floor object's in-game position relative to the map's three-dimensional plane.
    /// </summary>
    public class HPoint
    {
        /// <summary>
        /// Gets or sets the x-coordinate of the <see cref="HPoint"/>.
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Gets or sets the y-coordinate of the <see cref="HPoint"/>.
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Gets or sets the z-coordinate of the <see cref="HPoint"/>.
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HPoint"/> class with the specified floor object coordinates.
        /// </summary>
        /// <param name="x">The horizontal position of the floor object.</param>
        /// <param name="y">The vertical position of the floor object.</param>
        public HPoint(int x, int y)
            : this(x, y, 0.0)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="HPoint"/> class with the specified floor object coordinates.
        /// </summary>
        /// <param name="x">The horizontal position of the floor object.</param>
        /// <param name="y">The vertical position of the floor object.</param>
        /// <param name="z">The elevated position of the floor object.</param>
        public HPoint(int x, int y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HPoint"/> class with the specified floor object coordinates.
        /// </summary>
        /// <param name="x">The horizontal position of the floor object.</param>
        /// <param name="y">The vertical position of the floor object.</param>
        /// <param name="level">The UTF-16 character that represent a floor level in the room map.</param>
        public HPoint(int x, int y, char level)
            : this(x, y, ToZ(level))
        { }

        public static double ToZ(char level)
        {
            if (level >= '0' && level <= '9')
            {
                return (level - 48);
            }
            else if (level >= 'a' && level <= 't')
            {
                return (level - 87);
            }
            return 0;
        }
        public static char ToLevel(double z)
        {
            char level = 'x';
            if (z >= 0 && z <= 9)
            {
                level = (char)(z + 48);
            }
            else if (z >= 10 && z <= 29)
            {
                level = (char)(z + 87);
            }
            return level;
        }

        /// <summary>
        /// Converts the <see cref="HPoint"/> to a human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"X: {X}, Y: {Y}, Z: {Z}";
    }
}