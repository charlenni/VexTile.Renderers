using VexTile.Style.Mapbox;
using Xunit;

namespace VexTile.Styles.MapboxTests
{
    public class MapboxLoaderTests
    {
        readonly string _path = "..\\..\\..\\..\\..\\tests\\files\\osm-liberty.json";

        [Fact]
        public void CheckLoadingTest()
        {
            var stream = File.Open(_path, FileMode.Open, FileAccess.Read);

            var mapboxStyleFile = MapboxStyleFileLoader.Load(stream);

            Assert.True(mapboxStyleFile != null);
            Assert.True(mapboxStyleFile.Name == "OSM Liberty");
            Assert.True(mapboxStyleFile.Sources.Count == 2);
            Assert.True(mapboxStyleFile.Sources["openmaptiles"].Type == "vector");
            Assert.True(mapboxStyleFile.Sources["natural_earth_shaded_relief"].Type == "raster");
            Assert.True(mapboxStyleFile.Layers.Length == 102);
            Assert.True(mapboxStyleFile.Layers[0].Id == "background");
            Assert.True(mapboxStyleFile.Layers[0].Type == "background");
            Assert.True(mapboxStyleFile.Layers[14].Id == "water");
            Assert.True(mapboxStyleFile.Layers[14].SourceLayer == "water");
            Assert.True(mapboxStyleFile.Layers[14].Type == "fill");
            Assert.True(mapboxStyleFile.Layers[14].Source == "openmaptiles");
        }

    }
}
