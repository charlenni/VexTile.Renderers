namespace VexTile.DataSource.Basic.Extensions;

public static class StreamExtensions
{
    public static byte[] ToBytes(this Stream input)
    {
        using var ms = new MemoryStream();

        switch (input.GetType().Name)
        {
            case "ContentLengthReadStream":
            case "ReadOnlyStream":
                // not implemented
                break;
            default:
                if (input.Position != 0)
                {
                    // set position to 0 so that i can copy all the data
                    input.Position = 0;
                }

                break;
        }

        input.CopyTo(ms);
        return ms.ToArray();
    }
}