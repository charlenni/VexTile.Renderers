using SkiaSharp;

namespace VexTile.Renderer.Common;

public record TileInformation
{
    public bool Border { get; init; } = false;

    public bool Text { get; init; } = false;

    public float BorderSize { get; init; } = 2;

    public float TextSize { get; init; } = 20;

    public SKColor Color { get; init; } = SKColors.Red;
}
