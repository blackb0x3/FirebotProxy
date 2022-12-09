using System.Text;
using FirebotProxy.Models;

namespace FirebotProxy.Extensions;

public static class StringExtensions
{
    private static readonly char[] Padding = { '=' };
    private static readonly BijectiveDictionary<char, char> Base64UrlCharacterMap = GenerateBase64UrlCharacterMap();

    public static string ToUrlSafeBase64String(this string toEncode)
    {
        var sb = new StringBuilder();
        var normalBase64Encode = Convert.ToBase64String(Encoding.UTF8.GetBytes(toEncode));

        foreach (var chr in normalBase64Encode)
        {
            // TryGetForward returns a set, but in this case, the mapping is 1 -> 1
            var characterToAppend = Base64UrlCharacterMap.TryGetForward(chr, out var alts)
                ? alts.First()
                : chr;

            sb.Append(characterToAppend);
        }

        return sb.ToString().TrimEnd(Padding);
    }

    private static BijectiveDictionary<char, char> GenerateBase64UrlCharacterMap()
    {
        var bijectiveDictionary = new BijectiveDictionary<char, char>();
        bijectiveDictionary.Add('+', '-');
        bijectiveDictionary.Add('/', '_');

        return bijectiveDictionary;
    }
}