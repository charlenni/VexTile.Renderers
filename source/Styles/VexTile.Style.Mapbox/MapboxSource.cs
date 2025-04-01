using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VexTile.Style.Mapbox;

/// <summary>
/// Source in TileJSON format
/// See https://github.com/mapbox/tilejson-spec/tree/master/2.2.0
/// </summary>
public class MapboxSource
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    [JsonProperty("tileSize")]
    public int TileSize { get; set; } = 512;

    [JsonProperty("pixel_size")]
    public int PixelSize { get; set; }

    [JsonProperty("vector_layers")]
    public IList<MapboxTileStyle> VectorLayers { get; set; } = [];

    [JsonProperty("tilejson")]
    public string TileJson { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("version")]
    public string Version { get; set; } = string.Empty;

    [JsonProperty("attribution")]
    public string Attribution { get; set; } = string.Empty;

    [JsonProperty("template")]
    public string Template { get; set; } = string.Empty;

    [JsonProperty("legend")]
    public string Legend { get; set; } = string.Empty;

    [JsonProperty("scheme")]
    public string Scheme { get; set; } = string.Empty;

    [JsonProperty("tiles")]
    public IList<string> Tiles { get; set; } = [];

    [JsonProperty("grids")]
    public IList<string> Grids { get; set; } = [];

    [JsonProperty("data")]
    public IList<string> Data { get; set; } = [];

    [JsonProperty("minzoom")]
    public int ZoomMin { get; set; }

    [JsonProperty("maxzoom")]
    public int ZoomMax { get; set; }

    [JsonProperty("bounds")]
    public float[] Bounds { get; set; } = [-180.0f, -85.051129f, 180.0f, 85.051129f];

    [JsonProperty("center")]
    public JValue[] Center { get; set; }

    public override string ToString()
    {
        return Id + " " + Type;
    }
}
