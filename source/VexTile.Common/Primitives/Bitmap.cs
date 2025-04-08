using VexTile.Common.Interfaces;

namespace VexTile.Common.Primitives;

public class Bitmap : IBitmap
{
    public Bitmap(byte[] binary)
    {
        Binary = binary;
    }

    public object? Native { get; set; }

    public byte[] Binary { get; }
}
