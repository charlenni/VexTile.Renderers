using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VexTile.Common.Enums;
using VexTile.Common.Interfaces;
using VexTile.Style.Mapbox.Filter;

namespace VexTile.Style.Mapbox.Json.Converter;

/// <summary>
/// Create style type from given string in Mapbox GL style layer
/// </summary>
public class StyleTypeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string);
    }

    public override bool CanRead => true;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);

        return token.ToString().ToLower() switch
        {
            "background" => StyleType.Background,
            "fill" => StyleType.Fill,
            "line" => StyleType.Line,
            "raster" => StyleType.Raster,
            "symbol" => StyleType.Symbol,
            "fill-extrusion" => StyleType.FillExtrusion,
            _ => throw new NotImplementedException($"Style with type '{token}' is unknown")
        };
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
