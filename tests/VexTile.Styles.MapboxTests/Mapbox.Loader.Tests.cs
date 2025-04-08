using VexTile.Style.Mapbox;
using Xunit;

namespace VexTile.Styles.MapboxTests
{
    public class MapboxLoaderTests
    {
        readonly string _path = "files";

        [Fact]
        public async Task CheckLoadingStyleFileTest()
        {
            var stream = File.Open(Path.Combine(_path, "osm-liberty.json"), FileMode.Open, FileAccess.Read);

            var mapboxStyleFile = await MapboxStyleFileLoader.Load(stream);

            Assert.True(mapboxStyleFile != null);
            Assert.True(mapboxStyleFile.Name == "OSM Liberty");
            Assert.True(mapboxStyleFile.Sources.Count == 2);
            Assert.True(mapboxStyleFile.Sources["openmaptiles"].SourceType == Common.Enums.SourceType.Vector);
            Assert.True(mapboxStyleFile.Sources["natural_earth_shaded_relief"].SourceType == Common.Enums.SourceType.Raster);
            Assert.True(mapboxStyleFile.Layers.Length == 102);
            Assert.True(mapboxStyleFile.Layers[0].Id == "background");
            Assert.True(mapboxStyleFile.Layers[0].StyleType == "background");
            Assert.True(mapboxStyleFile.Layers[14].Id == "water");
            Assert.True(mapboxStyleFile.Layers[14].SourceLayer == "water");
            Assert.True(mapboxStyleFile.Layers[14].StyleType == "fill");
            Assert.True(mapboxStyleFile.Layers[14].Source == "openmaptiles");
        }

        [Fact]
        public async Task CheckLoadingSpriteFileTest()
        {
            var stream = File.Open(Path.Combine(_path, "osm-liberty.json"), FileMode.Open, FileAccess.Read);

            var mapboxStyleFile = await MapboxStyleFileLoader.Load(stream);

            Assert.True(mapboxStyleFile != null);
            Assert.True(mapboxStyleFile.Sprites != null);
            Assert.True(mapboxStyleFile.Sprites.Sprites.Count() == 241);
        }
    }
}
