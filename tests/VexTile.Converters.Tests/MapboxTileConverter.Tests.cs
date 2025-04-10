using VexTile.TileConverter.Mapbox;
using VexTile.TileDataSource.MbTilesTileDataSource;
using Xunit;

namespace VexTile.Readers.Tests;

public class MapboxConverterTests
{
    readonly string _path = "files\\zurich.mbtiles";

    [Fact]
    public async Task VectorTileConverterTest()
    {
        var dataSource = new MbTilesTileDataSource(_path, determineZoomLevelsFromTilesTable: true, determineTileRangeFromTilesTable: true);

        Assert.True(dataSource.Version == "3.6.1");

        var tileConverter = new MapboxTileConverter(dataSource);

        var vectorTile = await tileConverter.Convert(new NetTopologySuite.IO.VectorTiles.Tiles.Tile(8580, 5738, 14));

        Assert.NotNull(vectorTile);
        Assert.True(vectorTile.TileId == 183498457);
        Assert.True(vectorTile.IsEmpty == false);
        Assert.True(vectorTile.Layers.Count == 12);
        Assert.True(vectorTile.Layers[0].Name == "water");
        Assert.True(vectorTile.Layers[0].Features.Count == 9);
        Assert.True(vectorTile.Layers[0].Features[0].Attributes.Count == 2);
        Assert.True(vectorTile.Layers[0].Features[0].Attributes.GetNames()[0] == "class");
        Assert.True(vectorTile.Layers[0].Features[0].Attributes.GetValues()[0].ToString() == "river");
    }
}
