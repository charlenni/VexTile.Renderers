namespace VexTile.Common.Primitives
{
    public class Color
    {
        public Color(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        // Default colors
        public static Color Empty = new(0, 0, 0, 0);
        public static Color Black = new(0, 0, 0, 0);
    }
}
