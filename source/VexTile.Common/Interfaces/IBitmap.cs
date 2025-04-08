namespace VexTile.Common.Interfaces;

public interface IBitmap
{
    byte[] Binary { get; }
    object? Native { get; set; }
}
