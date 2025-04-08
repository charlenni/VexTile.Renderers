namespace VexTile.Common.Interfaces;

/// <summary>
/// Data source that is used as one time source
/// </summary>
public interface IDataSource
{
    Task<byte[]?> GetBytesAsync(string source);
}
