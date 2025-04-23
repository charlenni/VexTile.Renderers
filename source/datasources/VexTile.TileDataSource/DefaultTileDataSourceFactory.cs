using VexTile.Common.Interfaces;

namespace VexTile.TileDataSource;

public static class DefaultTileDataSourceFactory
{
    public const string FileScheme = "file";
    public const string MbTilesScheme = "mbtiles";
    public const string HttpScheme = "http";
    public const string HttpsScheme = "https";

    public static ITileDataSource? CreateTileDataSource(string[] sources)
    {
        if (sources == null || sources.Length == 0)
            return null;

        // Allways use the first one, even if there are more than one
        var source = sources[0];

        // Uri has a limitation of ~2000 bytes for URLs, so extract the scheme by hand
        var scheme = source.Substring(0, source.IndexOf(':'));

        return scheme switch
        {
            MbTilesScheme => CreateMbTilesSource(source.Substring(MbTilesScheme.Length + 3)),
            HttpScheme or HttpsScheme => CreateHttpSource(sources),
            _ => throw new ArgumentException($"Scheme '{scheme}' of '{source}' is not supported"),
        };
    }

    private static ITileDataSource CreateMbTilesSource(string source)
    {
        return new MbTilesTileDataSource.MbTilesTileDataSource(source);
    }

    private static ITileDataSource CreateHttpSource(string[] sources)
    {
        return new HttpTileDataSource.HttpTileDataSource(sources, 0, 6);
    }
}
