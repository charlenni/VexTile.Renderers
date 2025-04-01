using Newtonsoft.Json;

namespace VexTile.Style.Mapbox
{
    public class MapboxStyleFileLoader
    {
        public static MapboxStyleFile? Load(Stream jsonStream)
        {
            MapboxStyleFile? mapboxStyleFile;

            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(jsonStream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                mapboxStyleFile = serializer.Deserialize< MapboxStyleFile>(jsonTextReader);
            }

            return mapboxStyleFile;
        }
    }
}
