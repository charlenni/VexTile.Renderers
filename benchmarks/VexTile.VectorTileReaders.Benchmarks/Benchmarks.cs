using BenchmarkDotNet.Attributes;
using NetTopologySuite.IO.VectorTiles.Tiles;
using VexTile.Common.Interfaces;
using VexTile.TileConverter.Mapbox;
using VexTile.TileDataSource.MbTilesTileDataSource;

namespace VexTile.VectorTileReaders.Benchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        readonly string _path = "files\\zurich.mbtiles";

        ITileConverter? _tileConverter;
        List<Tile> _tiles = new List<Tile> { new Tile(1072, 717, 11), new Tile(8580, 5738, 14), new Tile(8581, 5738, 14), new Tile(8580, 5739, 14) };
        List<byte[]?> _data = new List<byte[]?>();

        [GlobalSetup]
        public void Setup()
        {
            var dataSource = new MbTilesTileDataSource(_path);

            _tileConverter = new MapboxTileConverter(dataSource);

            foreach (var tile in _tiles)
                _data.Add(dataSource.GetTileAsync(tile).ConfigureAwait(false).GetAwaiter().GetResult());
        }

        [Benchmark]
        [Arguments(0)]
        [Arguments(1)]
        [Arguments(2)]
        [Arguments(3)]
        public void ReadVectorTile(int i)
        {
            _tileConverter?.Convert(_tiles[i], _data[i])
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        [Benchmark]
        public void ReadVectorTiles()
        {
            for (var i = 0; i < _tiles.Count(); i++)
                _tileConverter?.Convert(_tiles[i], _data[i])
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
        }
    }
}
