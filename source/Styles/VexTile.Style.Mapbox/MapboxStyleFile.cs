using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VexTile.Style.Mapbox;

/// <summary>
/// Class for holding Mapbox/OpenMapTiles style file data
/// </summary>
public class MapboxStyleFile
{
    [JsonProperty("layers")]
    public MapboxTileStyle[] Layers { get; set; } = [];

    [JsonProperty("sources")]
    public Dictionary<string, MapboxSource> Sources { get; set; } = [];

    [JsonProperty("version")]
    public int Version { get; set; }

    [JsonProperty("bearing")]
    public float Bearing { get; set; } = 0.0f;

    [JsonProperty("center")]
    public float[] Center { get; set; } = [];

    [JsonProperty("glyphs")]
    public string Glyphs { get; set; } = "mapbox://fonts/mapbox/{fontstack}/{range}.pbf";

    [JsonProperty("metadata")]
    public JObject? Metadata { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("pitch")]
    public float Pitch { get; set; } = 0.0f;

    [JsonProperty("sprite")]
    public string Sprite { get; set; } = string.Empty;

    [JsonProperty("zoom")]
    public float Zoom { get; set; }
}
