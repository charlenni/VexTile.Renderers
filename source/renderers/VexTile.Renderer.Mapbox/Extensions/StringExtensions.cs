using NetTopologySuite.Features;
using System.Text.RegularExpressions;

namespace VexTile.Renderer.Mapbox.Extensions
{
    public static class StringExtensions
    {
        static Regex _regExFields = new Regex(@"\{(.*?)\}", (RegexOptions)8);

        /// <summary>
        /// Replace all fields in string with values
        /// </summary>
        /// <param name="text">String with fields to replace</param>
        /// <param name="attributes">Tags to replace fields with</param>
        /// <returns></returns>
        public static string ReplaceFields(this string text, AttributesTable attributes)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var result = text;

            var match = _regExFields.Match(text);

            while (match.Success)
            {
                var field = match.Groups[1].Captures[0].Value;

                // Search field
                var replacement = string.Empty;

                if (attributes.Exists(field))
                    replacement = attributes[field].ToString();

                // Replace field with new value
                result = result.Replace(match.Groups[0].Captures[0].Value, replacement);

                // Check for next field
                match = match.NextMatch();
            }
            ;

            return result;
        }
    }
}
