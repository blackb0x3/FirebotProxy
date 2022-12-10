using System.Reflection;
using System.Text.RegularExpressions;
using FirebotProxy.Domain.IoC;
using FirebotProxy.Domain.PrimaryPorts.GetChatWordCloud;
using FirebotProxy.Validation;
using FluentValidation;
using Newtonsoft.Json;

namespace FirebotProxy.Domain.Validators;

public class WordCloudSettingsValidator : AbstractValidator<WordCloudSettings>
{
    private const string StandardWebFontsFileName = "standard-web-fonts.json";
    private const string StandardGoogleFontsFileName = "standard-google-fonts.json";

    private static readonly Regex HexColourRegex = new("^[#]?[0-9A-Fa-f]{1,6}$");
    private static readonly string[] StandardWebFonts = LoadStandardWebFonts();
    private static readonly string[] StandardGoogleFonts = LoadStandardGoogleFonts();

    public WordCloudSettingsValidator()
    {
        RuleFor(settings => settings.Width)
            .GreaterThan(0)
            .WithMessage("Must be greater than 0 pixels.");

        RuleFor(settings => settings.Height)
            .GreaterThan(0)
            .WithMessage("Must be greater than 0 pixels.");

        RuleFor(settings => settings.BackgroundHexColour)
            .Must(BeAValidHexColour)
            .WithMessage("Must be a valid colour in hexadecimal format.");

        RuleFor(settings => settings.FontFamily)
            .Must(BeASupportedFontFamily)
            .WithMessage("Must be a supported font family.");

        RuleForEach(settings => settings.WordHexColours)
            .Must(BeAValidHexColour)
            .WithMessage("Must be a valid colour in hexadecimal format.");
    }

    private static bool BeASupportedFontFamily(string fontFamilyToCheck)
    {
        return Array.BinarySearch(StandardWebFonts, fontFamilyToCheck) >= 0 ||
               Array.BinarySearch(StandardGoogleFonts, fontFamilyToCheck) >= 0;
    }

    private static bool BeAValidHexColour(string hexColourToCheck)
    {
        return HexColourRegex.IsMatch(hexColourToCheck);
    }

    private static string[] LoadStandardWebFonts()
    {
        var path = Path.Join(AppDomainDirectory, StandardWebFontsFileName);

        return LoadFontFile(path);
    }

    private static string[] LoadStandardGoogleFonts()
    {
        var path = Path.Join(AppDomainDirectory, StandardGoogleFontsFileName);

        return LoadFontFile(path);
    }

    private static string[] LoadFontFile(string fontFilePath)
    {
        return (string[]?)JsonConvert.DeserializeObject(File.ReadAllText(fontFilePath)) ?? Array.Empty<string>();
    }

    private static string AppDomainDirectory => Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ?? string.Empty;
}