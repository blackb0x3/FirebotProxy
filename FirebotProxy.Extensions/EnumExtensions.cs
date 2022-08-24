using System.ComponentModel;

namespace FirebotProxy.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T e) where T : Enum
    {
        var fi = e.GetType().GetField(e.ToString());

        var attributes = (DescriptionAttribute[])fi?.GetCustomAttributes(typeof(DescriptionAttribute), false)!;

        return attributes.Length > 0
            ? attributes[0].Description
            : e.ToString();
    }
}