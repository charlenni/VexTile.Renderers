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
    public string StyleType { get; set; } = string.Empty;

    [JsonConverter(typeof(FilterConverter))]
    [JsonProperty("filter")]
    public IFilter Filter { get; set; } = new EmptyFilter();

    [JsonProperty("layout")]
    public MapboxLayout Layout { get; set; } = MapboxLayout.Empty;

    [JsonProperty("maxzoom")]
    public int MaxZoom { get; set; } = -1;

    [JsonProperty("metadata")]
    public JObject? Metadata { get; set; }

    [JsonProperty("minzoom")]
    public int MinZoom { get; set; } = -1;

    [JsonProperty("paint")]
    public MapboxPaint Paint { get; set; } = MapboxPaint.Empty;

    [JsonProperty("slot")]
    public string Slot { get; set; } = string.Empty;

    [JsonProperty("source")]
    public string Source { get; set; } = string.Empty;

    [JsonProperty("source-layer")]
    public string SourceLayer { get; set; } = string.Empty;

    [JsonProperty("interactive")]
    public bool Interactive { get; set; }

    public bool Visible => Layout?.Visibility == "visible";

    public override string ToString()
    {
        return Id + " " + StyleType;
    }

    public void Update(EvaluationContext context)
    {
        throw new NotImplementedException();
    }
}
