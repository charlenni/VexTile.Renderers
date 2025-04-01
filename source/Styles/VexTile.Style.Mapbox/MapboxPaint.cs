using Newtonsoft.Json;
using VexTile.Common.Primitives;
using VexTile.Style.Mapbox.Expressions;
using VexTile.Style.Mapbox.Json.Converter;

namespace VexTile.Style.Mapbox;

public class MapboxPaint
{
    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("background-color")]
    public StoppedColor BackgroundColor { get; set; } = new StoppedColor { SingleVal = Color.Black };
    
    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("background-emissive-strength")]
    public StoppedFloat BackgroundEmissiveStrength { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("background-opacity")]
    public StoppedFloat BackgroundOpacity { get; set; } = new StoppedFloat() { SingleVal = 1.0f };

    [JsonConverter(typeof(StoppedStringConverter))]
    [JsonProperty("background-pattern")]
    public StoppedString BackgroundPattern { get; set; } = new StoppedString { SingleVal = string.Empty };

    [JsonConverter(typeof(StoppedBooleanConverter))]
    [JsonProperty("fill-antialias")]
    public StoppedBoolean FillAntialias { get; set; } = new StoppedBoolean() { SingleVal = true };

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("fill-color")]
    public StoppedColor FillColor { get; set; } = new StoppedColor { SingleVal = Color.Black };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("fill-emissive-strength")]
    public StoppedFloat FillEmissiveStrength { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("fill-opacity")]
    public StoppedFloat FillOpacity { get; set; } = new StoppedFloat() { SingleVal = 1.0f };

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("fill-outline-color")]
    public StoppedColor FillOutlineColor { get; set; } = new StoppedColor { SingleVal = Color.Black };

    [JsonConverter(typeof(StoppedStringConverter))]
    [JsonProperty("fill-pattern")]
    public StoppedString FillPattern { get; set; } = new StoppedString() { SingleVal = string.Empty };

    [JsonConverter(typeof(StoppedFloatArrayConverter))]
    [JsonProperty("fill-translate")]
    public StoppedFloatArray FillTranslate { get; set; } = new StoppedFloatArray() { SingleVal = [0, 0] };

    [JsonProperty("fill-translate-anchor")]
    public string FillTranslateAnchor { get; set; } = "map";

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("line-blur")]
    public StoppedFloat LineBlur { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("line-color")]
    public StoppedColor LineColor { get; set; } = new StoppedColor { SingleVal = Color.Black };

    [JsonConverter(typeof(StoppedFloatArrayConverter))]
    [JsonProperty("line-dasharray")]
    public StoppedFloatArray LineDashArray { get; set; } = new StoppedFloatArray() { SingleVal = [] };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("line-emissive-strength")]
    public StoppedFloat LineEmissiveStrength { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("line-gap-width")]
    public StoppedFloat LineGapWidth { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("line-gradient")]
    public StoppedColor LineGradient { get; set; } = new StoppedColor() { SingleVal = Color.Black };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("line-occlusion-opacity")]
    public StoppedFloat LineOcclusionOpacity { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("line-offset")]
    public StoppedFloat LineOffset { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("line-opacity")]
    public StoppedFloat LineOpacity { get; set; } = new StoppedFloat() { SingleVal = 1.0f };

    [JsonConverter(typeof(StoppedStringConverter))]
    [JsonProperty("line-pattern")]
    public StoppedString LinePattern { get; set; } = new StoppedString() { SingleVal = string.Empty };

    [JsonConverter(typeof(StoppedFloatArrayConverter))]
    [JsonProperty("line-translate")]
    public StoppedFloatArray LineTranslate { get; set; } = new StoppedFloatArray() { SingleVal = [0, 0] };

    [JsonProperty("line-translate-anchor")]
    public string LineTranslateAnchor { get; set; } = "map";

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("line-width")]
    public StoppedFloat LineWidth { get; set; } = new StoppedFloat() { SingleVal = 1.0f };

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("icon-color")]
    public StoppedColor IconColor { get; set; } = new StoppedColor { SingleVal = Color.Black };

    [JsonProperty("icon-color-brightness-max")]
    public float IconColorBrightnessMax { get; set; } = 1.0f;

    [JsonProperty("icon-color-brightness-min")]
    public float IconColorBrightnessMin { get; set; } = 0.0f;

    [JsonProperty("icon-color-contrast")]
    public float IconColorContrast { get; set; } = 0.0f;

    [JsonProperty("icon-color-saturation")]
    public float IconColorSaturation { get; set; } = 0.0f;

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("icon-emissive-strength")]
    public StoppedFloat IconEmissiveStrength { get; set; } = new StoppedFloat() { SingleVal = 1.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("icon-halo-blur")]
    public StoppedFloat IconHaloBlur { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("icon-halo-color")]
    public StoppedColor IconHaloColor { get; set; } = new StoppedColor { SingleVal = Color.Black };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("icon-halo-width")]
    public StoppedFloat IconHaloWidth { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("icon-image-cross-fade")]
    public StoppedFloat IconImageCrossFade { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("icon-occlusion-opacity")]
    public StoppedFloat IconOcclusionOpacity { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("icon-opacity")]
    public StoppedFloat IconOpacity { get; set; } = new StoppedFloat() { SingleVal = 1.0f };

    [JsonConverter(typeof(StoppedFloatArrayConverter))]
    [JsonProperty("icon-translate")]
    public StoppedFloatArray IconTranslate { get; set; } = new StoppedFloatArray() { SingleVal = [0, 0] };

    [JsonProperty("icon-translate-anchor")]
    public string IconTranslateAnchor { get; set; } = "map";

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("text-color")]
    public StoppedColor TextColor { get; set; } = new StoppedColor { SingleVal = Color.Black };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("text-emissive-strength")]
    public StoppedFloat TextEmissiveStrength { get; set; } = new StoppedFloat() { SingleVal = 1.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("text-halo-blur")]
    public StoppedFloat TextHaloBlur { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("text-halo-color")]
    public StoppedColor TextHaloColor { get; set; } = new StoppedColor { SingleVal = Color.Black };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("text-halo-width")]
    public StoppedFloat TextHaloWidth { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("text-occlusion-opacity")]
    public StoppedFloat TextOcclusionOpacity { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("text-opacity")]
    public StoppedFloat TextOpacity { get; set; } = new StoppedFloat { SingleVal = 1.0f };

    [JsonConverter(typeof(StoppedFloatArrayConverter))]
    [JsonProperty("text-translate")]
    public StoppedFloatArray TextTranslate { get; set; } = new StoppedFloatArray() { SingleVal = [0, 0] };

    [JsonProperty("text-translate-anchor")]
    public string TextTranslateAnchor { get; set; } = "map";

    [JsonProperty("raster-brightness-max")]
    public float RasterBrightnessMax { get; set; } = 1.0f;

    [JsonProperty("raster-brightness-min")]
    public float RasterBrightnessMin { get; set; } = 0.0f;

    [JsonConverter(typeof(StoppedColorConverter))]
    [JsonProperty("raster-color")]
    public StoppedColor RasterColor { get; set; } = new StoppedColor { SingleVal = Color.Empty };

    [JsonConverter(typeof(StoppedFloatArrayConverter))]
    [JsonProperty("raster-color-mix")]
    public StoppedFloatArray RasterColorMix { get; set; } = new StoppedFloatArray { SingleVal = [0.2126f, 0.7152f, 0.0722f, 0.0f] };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("raster-color-range")]
    public StoppedFloat RasterColorRange { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("raster-contrast")]
    public StoppedFloat RasterContrast { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("raster-emissive-strength")]
    public StoppedFloat RasterEmissiveStrength { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("raster-fade-duration")]
    public StoppedFloat RasterFadeDuration { get; set; } = new StoppedFloat() { SingleVal = 300.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("raster-hue-rotate")]
    public StoppedFloat RasterHueRotate { get; set; } = new StoppedFloat() { SingleVal = 0.0f };

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("raster-opacity")]
    public StoppedFloat RasterOpacity { get; set; } = new StoppedFloat() { SingleVal = 1.0f };

    [JsonProperty("raster-resampling")]
    public string RasterResampling { get; set; } = "linear";

    [JsonConverter(typeof(StoppedFloatConverter))]
    [JsonProperty("raster-saturation")]
    public StoppedFloat RasterSaturation { get; set; } = new StoppedFloat() { SingleVal = 0.0f };
}
