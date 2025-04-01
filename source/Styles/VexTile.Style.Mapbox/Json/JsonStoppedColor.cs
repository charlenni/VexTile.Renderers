using Newtonsoft.Json;
using VexTile.Common.Primitives;

namespace VexTile.Style.Mapbox.Json;

/// <summary>
/// Class holding StoppedColor data in Json format
/// </summary>
public class JsonStoppedColor
{
    [JsonProperty("base")]
    public float Base { get; set; } = 1f;

    [JsonProperty("stops")]
    public IList<KeyValuePair<float, Color>> Stops { get; set; } = [];
}
