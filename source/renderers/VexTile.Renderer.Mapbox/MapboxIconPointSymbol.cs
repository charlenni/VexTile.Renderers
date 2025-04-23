using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Strtree;
using NetTopologySuite.IO.VectorTiles.Tiles;
using SkiaSharp;
using System.Globalization;
using System.Text.RegularExpressions;
using VexTile.Common.Primitives;
using VexTile.Renderer.Common.Interfaces;
using VexTile.Style.Mapbox;
using VexTile.Style.Mapbox.Enums;
using VexTile.Style.Mapbox.Expressions;

namespace VexTile.Renderer.Mapbox;

public class MapboxIconPointSymbol : MapboxSymbol
{
    bool _isIconOptional;
    bool _isIconAllowOverlap;
    bool _isTextAllowOverlap;
    SKImage _icon;
    float _iconRotation;
    int _iconWidth;
    int _iconHeight;
    float _iconScale;
    int _iconPadding;
    SKPaint _iconPaint = new SKPaint();
    SKPoint _iconAnchor;
    SKPoint _iconOffset;
    StoppedFloat _iconBrightnessMin;
    StoppedFloat _iconBrightnessMax;
    StoppedFloat _iconContrast;
    StoppedFloat _iconSaturation;
    StoppedFloat _iconOpacity;
    StoppedFloatArray _iconTranslate;
    StoppedEnum<MapAlignment> _iconTranslateAnchor;

    static Regex regex = new Regex(@".*\{(.*)\}.*", RegexOptions.Compiled);

    public MapboxIconPointSymbol(Tile tile, Point point, MapboxTileStyle style, Func<string, SKImage> spriteFactory, EvaluationContext context, IFeature feature) : base(tile)
    {
        context.Feature = feature;

        CreateCommon(style, context);

        var spriteName = (string)(style.Layout.IconImage?.Evaluate(context) ?? string.Empty);

        if (spriteName == string.Empty)
        {
            return;
        }

        spriteName = ReplaceWithTags(spriteName, (AttributesTable)feature.Attributes, context);

        var sprite = spriteFactory(spriteName);

        if (sprite == null)
        {
            // TODO: What to do?
            System.Diagnostics.Debug.WriteLine($"Couldn't find sprite with name '{spriteName}'");
            //throw new SpriteNotFoundException($"Couldn't find sprite with name '{spriteName}'");
            return;
        }

        CreateIcon(style, sprite, context);
    }

    /// <summary>
    /// Point where symbol is placed in tile coordinates
    /// </summary>
    public Point Point { get; }

    public bool CheckForSpace(SKCanvas canvas, EvaluationContext context, STRtree<ISymbol> tree)
    {
        // Check, if there is space for icon
        var envIcon = CalcEnvelope(canvas, context);

        var symbols = tree.Query(envIcon);

        foreach (var symbol in symbols)
        {
            if (!symbol.AllowOthers)
            {
                return false;
            }
        }

        return true;
    }

    public void Draw(SKCanvas canvas, EvaluationContext context, ref STRtree<ISymbol> tree)
    {
        Envelope? envIcon = null;

        DrawIcon(canvas, context);

        envIcon = CalcEnvelope(canvas, context);

        if (envIcon != null)
        {
            tree.Insert(envIcon, this);
        }
    }

    private void CreateIcon(MapboxTileStyle style, SKImage sprite, EvaluationContext context)
    {
        _isIconAllowOverlap = style.Layout.IconAllowOverlap;
        _isIconOptional = style.Layout.IconOptional;

        AllowOthers = style.Layout.IconIgnorePlacement;

        _icon = sprite;
        _iconBrightnessMin = style.Paint.IconColorBrightnessMin;
        _iconBrightnessMax = style.Paint.IconColorBrightnessMax;
        _iconContrast = style.Paint.IconColorContrast;
        _iconSaturation = style.Paint.IconColorSaturation;
        _iconOpacity = style.Paint.IconOpacity;

        _iconScale = (float)(style.Layout.IconSize.Evaluate(context) ?? 1);
        _iconRotation = (float)(style.Layout.IconRotate.Evaluate(context) ?? 0);
        _iconPadding = (int)(float)(style.Layout.IconPadding.Evaluate(context) ?? 0);

        _iconWidth = (int)Math.Ceiling(sprite.Width * _iconScale) + _iconPadding * 2;
        _iconHeight = (int)Math.Ceiling(sprite.Height * _iconScale) + _iconPadding * 2;

        _iconAnchor = CreateAnchor(style.Layout.IconAnchor, _iconWidth, _iconHeight);
        _iconOffset = CreateOffset((float[])style.Layout.IconOffset.Evaluate(context), _iconScale);

        _iconTranslate = style.Paint.IconTranslate;
        _iconTranslateAnchor = style.Paint.IconTranslateAnchor;
    }

    private Envelope CalcEnvelope(SKCanvas canvas, EvaluationContext context)
    {
        var result = new Envelope();

        return result;
    }

    private void DrawIcon(SKCanvas canvas, EvaluationContext context)
    {
        canvas.Save();

        _iconPaint.ColorFilter = CreateColorFilter(
            (float)(_iconBrightnessMin.Evaluate(context) ?? 0.0f),
            (float)(_iconBrightnessMax.Evaluate(context) ?? 1.0f),
            (float)(_iconContrast.Evaluate(context) ?? 1.0f),
            (float)(_iconSaturation.Evaluate(context) ?? 1.0f),
            (float)(_iconOpacity.Evaluate(context) ?? 1.0f));

        var translate = (float[])(_iconTranslate.Evaluate(context) ?? new float[] { 0f, 0f });
        var translateAnchor = (MapAlignment)(_iconTranslateAnchor.Evaluate(context) ?? MapAlignment.Map);

        if (translateAnchor == MapAlignment.Viewport)
        {
            canvas.RotateDegrees(-context.Rotation);
        }

        canvas.Scale(1f / context.Scale);
        canvas.Translate(translate[0], translate[1]);
        canvas.Translate(_iconOffset);
        canvas.RotateDegrees(_iconRotation);

        canvas.DrawImage(_icon, new SKRect(_iconPadding, _iconPadding, _iconWidth - _iconPadding, _iconHeight - _iconPadding), _iconPaint);

        canvas.Restore();
    }

    private SKPoint CreateAnchor(Anchor anchor, int width, int height)
    {
        return anchor switch
        {
            Anchor.Center => new SKPoint(width / 2, height / 2),
            Anchor.Left => new SKPoint(0, height / 2),
            Anchor.Right => new SKPoint(-width, height / 2),
            Anchor.Top => new SKPoint(width / 2, 0),
            Anchor.Bottom => new SKPoint(width / 2, -height),
            Anchor.TopLeft => new SKPoint(0, 0),
            Anchor.TopRight => new SKPoint(-width, 0),
            Anchor.BottomLeft => new SKPoint(0, -height),
            Anchor.BottomRight => new SKPoint(-width, -height),
            _ => throw new NotImplementedException($"Unknown IconAnchor")
        };
    }

    private string ReplaceWithTags(string text, AttributesTable attributes, EvaluationContext context = null)
    {
        var match = regex.Match(text);

        if (!match.Success)
            return text;

        var val = match.Groups[1].Value;

        if (attributes.Exists(val))
            return text.Replace($"{{{val}}}", attributes[val].ToString());

        if (context != null && context.Attributes != null && context.Attributes.Exists(val))
            return text.Replace($"{{{val}}}", context.Attributes[val].ToString());

        // Check, if match starts with name
        if (val.StartsWith("name"))
        {
            // Try to take the localized name
            var code = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if (attributes.Exists("name:" + code))
                return text.Replace($"{{{val}}}", attributes["name:" + code].ToString());
            if (attributes.Exists("name_" + code))
                return text.Replace($"{{{val}}}", attributes["name_" + code].ToString());

            // We didn't find a name in the tags, so remove this part
            return text.Replace($"{{{val}}}", "");
        }

        return text;
    }

    private SKPoint CreateOffset(float[] offset, float scale)
    {
        if (offset.Length == 2)
        {
            return new SKPoint(offset[0] * scale, offset[1] * scale);
        }
        else
        {
            return new SKPoint(0, 0);
        }
    }

    private SKColorFilter CreateColorFilter(float minBrightness, float maxBrightness, float contrast, float saturation, float opacity)
    {
        // Brightness-Transformation
        float brightnessRange = maxBrightness - minBrightness;
        float brightnessOffset = minBrightness;

        // Convert: contrast/saturation ∈ [-1..1] → [0..2]
        float mappedContrast = 1f + contrast;
        float mappedSaturation = 1f + saturation;

        // ITU-R BT.601
        float lumR = 0.2126f;
        float lumG = 0.7152f;
        float lumB = 0.0722f;

        float sr = (1f - mappedSaturation) * lumR;
        float sg = (1f - mappedSaturation) * lumG;
        float sb = (1f - mappedSaturation) * lumB;

        // Contrast Offset
        float contrastOffset = 0.5f * (1f - mappedContrast) * 255f;

        // Final Brightness-Offset
        float brightnessShift = 255f * brightnessOffset;

        // Combined Matrix
        float[] matrix = new float[]
        {
            // Red
            (sr + mappedSaturation) * mappedContrast * brightnessRange,
            sg * mappedContrast * brightnessRange,
            sb * mappedContrast * brightnessRange,
            0,
            contrastOffset + brightnessShift,

            // Green
            sr * mappedContrast * brightnessRange,
            (sg + mappedSaturation) * mappedContrast * brightnessRange,
            sb * mappedContrast * brightnessRange,
            0,
            contrastOffset + brightnessShift,

            // Blue
            sr * mappedContrast * brightnessRange,
            sg * mappedContrast * brightnessRange,
            (sb + mappedSaturation) * mappedContrast * brightnessRange,
            0,
            contrastOffset + brightnessShift,

            // Alpha
            0, 0, 0, opacity, 0
        };

        return SKColorFilter.CreateColorMatrix(matrix);
    }

    private static (float rotationDeg, float scaleX, float scaleY, SKPoint translation) AnalyzeCanvasTransform(SKCanvas canvas)
    {
        var m = canvas.TotalMatrix;

        // Rotation berechnen (in Grad)
        float rotationRad = (float)Math.Atan2(m.SkewY, m.ScaleY);
        float rotationDeg = (float)(rotationRad * (180f / Math.PI));

        // Skalierung berechnen (Betrag der Vektoren)
        float scaleX = (float)Math.Sqrt(m.ScaleX * m.ScaleX + m.SkewX * m.SkewX);
        float scaleY = (float)Math.Sqrt(m.SkewY * m.SkewY + m.ScaleY * m.ScaleY);

        // Translation (Position)
        var translation = new SKPoint(m.TransX, m.TransY);

        return (rotationDeg, scaleX, scaleY, translation);
    }
}
