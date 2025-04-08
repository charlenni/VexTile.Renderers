using SkiaSharp;
using VexTile.Common.Primitives;

namespace VexTile.Renderer.Mapbox;

public class MapboxPaint
{
    readonly SKPaint _paint = new SKPaint() { IsAntialias = true, BlendMode = SKBlendMode.SrcOver };  // Set this by default
    EvaluationContext? _lastContext;
    float _strokeWidth;

    public MapboxPaint(string id)
    {
        Id = id;
    }

    // Id of layer for this OMTPaint
    public string Id { get; }

    public SKPaint CreateSKPaint(EvaluationContext context)
    {
        if (_lastContext != null && context.Equals(_lastContext))
            return _paint;

        if (variableColor || variableOpacity)
        {
            var c = variableColor ? funcColor(context) : color;
            var o = variableOpacity ? funcOpacity(context) : opacity;
            _paint.Color = c.WithAlpha((byte)(c.Alpha * o));
        }

        if (variableStyle)
        {
            _paint.Style = funcStyle(context);
        }

        if (variableAntialias)
        {
            _paint.IsAntialias = funcAntialias(context);
        }

        if (variableStrokeWidth)
        {
            _paint.StrokeWidth = funcStrokeWidth(context) * context.Scale;
        }
        else
        {
            _paint.StrokeWidth = _strokeWidth * context.Scale;
        }

        if (variableStrokeCap)
        {
            _paint.StrokeCap = funcStrokeCap(context);
        }

        if (variableStrokeJoin)
        {
            _paint.StrokeJoin = funcStrokeJoin(context);
        }

        if (variableStrokeMiter)
        {
            _paint.StrokeMiter = funcStrokeMiter(context);
        }

        if (variableShader)
        {
            _paint.Shader = funcShader(context);
        }

        // We have to multiply the dasharray with the linewidth
        if (variableDashArray)
        {
            var array = funcDashArray(context);
            for (var i = 0; i < array.Length; i++)
                array[i] = array[i] * _paint.StrokeWidth;
            _paint.PathEffect = SKPathEffect.CreateDash(array, 0);
        }
        else if (fixDashArray != null)
        {
            var array = new float[fixDashArray.Length];
            for (var i = 0; i < array.Length; i++)
                array[i] = fixDashArray[i] * _paint.StrokeWidth;
            _paint.PathEffect = SKPathEffect.CreateDash(array, 0);
        }

        _lastContext = new EvaluationContext(context.Zoom, context.Scale, context.Rotation, context.Attributes);

        return _paint;
    }

    #region Color

    SKColor color = SKColor.Empty;

    bool variableColor = false;

    Func<EvaluationContext, SKColor>? funcColor;

    public void SetFixColor(SKColor c)
    {
        variableColor = false;
        color = c;
        _paint.Color = color.WithAlpha((byte)(color.Alpha * opacity));
    }

    public void SetVariableColor(Func<EvaluationContext, SKColor> func)
    {
        variableColor = true;
        funcColor = func;
    }

    #endregion

    #region Opacity

    float opacity = 1.0f;

    bool variableOpacity = false;

    Func<EvaluationContext, float>? funcOpacity;

    public void SetFixOpacity(float o)
    {
        variableOpacity = false;
        opacity = o;
        _paint.Color = color.WithAlpha((byte)(color.Alpha * opacity));
    }

    public void SetVariableOpacity(Func<EvaluationContext, float> func)
    {
        variableOpacity = true;
        funcOpacity = func;
    }

    #endregion

    #region Style

    bool variableStyle = false;

    Func<EvaluationContext, SKPaintStyle>? funcStyle;

    public void SetFixStyle(SKPaintStyle style)
    {
        variableStyle = false;
        _paint.Style = style;
    }

    public void SetVariableStyle(Func<EvaluationContext, SKPaintStyle> func)
    {
        variableStyle = true;
        funcStyle = func;
    }

    #endregion

    #region Antialias

    bool variableAntialias = false;

    Func<EvaluationContext, bool>? funcAntialias;

    public void SetFixAntialias(bool antialias)
    {
        variableAntialias = false;
        _paint.IsAntialias = antialias;
    }

    public void SetVariableAntialias(Func<EvaluationContext, bool> func)
    {
        variableAntialias = true;
        funcAntialias = func;
    }

    #endregion

    #region StrokeWidth

    bool variableStrokeWidth = false;

    Func<EvaluationContext, float>? funcStrokeWidth;

    public void SetFixStrokeWidth(float width)
    {
        variableStrokeWidth = false;
        _strokeWidth = width;
    }

    public void SetVariableStrokeWidth(Func<EvaluationContext, float> func)
    {
        variableStrokeWidth = true;
        funcStrokeWidth = func;
    }

    #endregion

    #region StrokeCap

    bool variableStrokeCap = false;

    Func<EvaluationContext, SKStrokeCap>? funcStrokeCap;

    public void SetFixStrokeCap(SKStrokeCap cap)
    {
        variableStrokeCap = false;
        _paint.StrokeCap = cap;
    }

    public void SetVariableStrokeCap(Func<EvaluationContext, SKStrokeCap> func)
    {
        variableStrokeCap = true;
        funcStrokeCap = func;
    }

    #endregion

    #region StrokeJoin

    bool variableStrokeJoin = false;

    Func<EvaluationContext, SKStrokeJoin>? funcStrokeJoin;

    public void SetFixStrokeJoin(SKStrokeJoin join)
    {
        variableStrokeJoin = false;
        _paint.StrokeJoin = join;
    }

    public void SetVariableStrokeJoin(Func<EvaluationContext, SKStrokeJoin> func)
    {
        variableStrokeJoin = true;
        funcStrokeJoin = func;
    }

    #endregion

    #region StrokeMiter

    bool variableStrokeMiter = false;

    Func<EvaluationContext, float>? funcStrokeMiter;

    public void SetFixStrokeMiter(float miter)
    {
        variableStrokeMiter = false;
        _paint.StrokeMiter = miter;
    }

    public void SetVariableStrokeMiter(Func<EvaluationContext, float> func)
    {
        variableStrokeMiter = true;
        funcStrokeMiter = func;
    }

    #endregion

    #region Shader

    bool variableShader = false;

    Func<EvaluationContext, SKShader>? funcShader;

    public void SetFixShader(SKShader shader)
    {
        variableShader = false;
        _paint.Shader = shader;
    }

    public void SetVariableShader(Func<EvaluationContext, SKShader> func)
    {
        variableShader = true;
        funcShader = func;
    }

    #endregion

    #region DashArray

    bool variableDashArray = false;
    float[] fixDashArray = [];

    Func<EvaluationContext, float[]>? funcDashArray;

    public void SetFixDashArray(float[] array)
    {
        variableDashArray = false;
        fixDashArray = new float[array.Length];
        for (int i = 0; i < array.Length; i++)
            fixDashArray[i] = array[i];
    }

    public void SetVariableDashArray(Func<EvaluationContext, float[]> func)
    {
        variableDashArray = true;
        funcDashArray = func;
    }

    #endregion
}
