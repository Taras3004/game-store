using System.Text.RegularExpressions;

namespace GameStore.Models.Helpers
{
    public static partial class KeyGenerator
    {
        [GeneratedRegex(@"[^\p{L}0-9\s\-]", RegexOptions.CultureInvariant)]
        private static partial Regex InvalidCharsRegex();

        [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant)]
        private static partial Regex WhitespaceRegex();

        [GeneratedRegex(@"\-+", RegexOptions.CultureInvariant)]
        private static partial Regex MultipleHyphensRegex();

        public static string GenerateKey(string gameName)
        {
            if (string.IsNullOrWhiteSpace(gameName))
                return string.Empty;

            var key = gameName.ToLowerInvariant();

            key = InvalidCharsRegex().Replace(key, "");
            key = WhitespaceRegex().Replace(key, "-");
            key = MultipleHyphensRegex().Replace(key, "-");

            return key.Trim('-');
        }
    }
}
