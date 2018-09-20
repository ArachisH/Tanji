using System;
using System.Drawing;
using System.Diagnostics;

namespace Sulakore.Habbo
{
    [DebuggerDisplay(@"\{X = {X} Y = {Y} Z = {Z}\}")]
    public struct HPoint : IEquatable<HPoint>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Z { get; set; }
        public bool IsEmpty => (Equals(Empty));

        public static readonly HPoint Empty;

        public static implicit operator HPoint((int x, int y) point) => new HPoint(point.x, point.y);
        public static implicit operator HPoint((int x, int y, double z) point) => new HPoint(point.x, point.y, point.z);

        public static implicit operator (int x, int y) (HPoint point) => (point.X, point.Y);
        public static implicit operator (int x, int y, double z) (HPoint point) => (point.X, point.Y, point.Z);

        public static implicit operator Point(HPoint point) => new Point(point.X, point.Y);
        public static implicit operator HPoint(Point point) => new HPoint(point.X, point.Y);

        public static bool operator !=(HPoint left, HPoint right) => !(left == right);
        public static bool operator ==(HPoint left, HPoint right) => left.Equals(right);

        public HPoint(int x, int y)
            : this(x, y, 0)
        { }
        public HPoint(int x, int y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public HPoint(int x, int y, char level)
            : this(x, y, ToZ(level))
        { }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = (hashCode * -1521134295 + base.GetHashCode());
            hashCode = (hashCode * -1521134295 + X.GetHashCode());
            hashCode = (hashCode * -1521134295 + Y.GetHashCode());
            return hashCode;
        }
        public override string ToString() => $"{{X={X},Y={Y},Z={Z}}}";

        public override bool Equals(object obj)
        {
            if (obj is HPoint point)
            {
                return Equals(point);
            }
            return false;
        }
        public bool Equals(HPoint point) => (X == point.X && Y == point.Y);

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
    }
}