namespace VexTile.Style.Mapbox.Expressions;

internal class MGLNullType : MGLType
{
    public object Value => null;

    public override string ToString()
    {
        return "null";
    }
}
