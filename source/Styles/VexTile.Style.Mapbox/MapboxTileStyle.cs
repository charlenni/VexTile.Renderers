using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VexTile.Common.Interfaces;
using VexTile.Common.Primitives;
using VexTile.Style.Mapbox.Filter;
using VexTile.Style.Mapbox.Json.Converter;

namespace VexTile.Style.Mapbox;

public class MapboxTileStyle : ITileStyle
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonConverter(typeof(FilterConverter))]
    [JsonProperty("filter")]
    public IFilter Filter { get; set; } = new EmptyFilter();

    [JsonProperty("layout")]
    public MapboxLayout? Layout { get; set; } = null;

    [JsonProperty("maxzoom")]
    public int MaxZoom { get; set; }

    [JsonProperty("metadata")]
    public JObject? Metadata { get; set; }

    [JsonProperty("minzoom")]
    public int MinZoom { get; set; }

    [JsonProperty("paint")]
    public MapboxPaint? Paint { get; set; }

    [JsonProperty("slot")]
    public string Slot { get; set; } = string.Empty;

    [JsonProperty("source")]
    public string Source { get; set; } = string.Empty;

    [JsonProperty("source-layer")]
    public string SourceLayer { get; set; } = string.Empty;

    [JsonProperty("interactive")]
    public bool Interactive { get; set; }

    public IEnumerable<IVectorPaint> Paints => throw new NotImplementedException();

    public bool Visible => Layout?.Visibility == "visible";

    public override string ToString()
    {
        return Id + " " + Type;
    }

    public void Update(EvaluationContext context)
    {
        throw new NotImplementedException();
    }
}
