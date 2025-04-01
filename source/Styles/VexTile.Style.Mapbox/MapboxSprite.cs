using Newtonsoft.Json;

namespace VexTile.Style.Mapbox;

/// <summary>
/// Class holding Sprite data in Json format
/// </summary>
public class MapboxSprite
{
    [JsonProperty("x")]
    public int X { get; set; }

    [JsonProperty("y")]
    public int Y { get; set; }

    [JsonProperty("width")]
    public int Width { get; set; }

    [JsonProperty("height")]
    public int Height { get; set; }

    [JsonProperty("pixelRatio")]
    public float PixelRatio { get; set; }

    [JsonProperty("content")]
    public IList<float> Content { get; set; } = [];

    [JsonProperty("strechX")]
    public IList<float> StrechX { get; set; } = [];

    [JsonProperty("strechY")]
    public IList<float> StrechY { get; set; } = [];
}
