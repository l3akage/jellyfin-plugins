using System.Globalization;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.JellyTag.Services;

/// <summary>
/// Renders the text of a status rule from the parts of a tag such as "NewSeasonSoon:2:2026-11-03".
/// </summary>
public static partial class StatusText
{
    [GeneratedRegex(@"(?<=\p{L})\.")]
    private static partial Regex TrailingPeriodRegex();

    [GeneratedRegex(@"\{(?<name>date|DATE)(?::(?<fmt>[^}]*))?\}|\{(?<idx>\d+)\}")]
    private static partial Regex TokenRegex();

    /// <summary>
    /// Replaces {1}, {2}... with the tag parts after the name and {date[:format]} with the first part
    /// that is an ISO date ({DATE:format} upper-cases the result).
    /// </summary>
    /// <param name="template">Rule text.</param>
    /// <param name="tagParts">The tag split at ':' (index 0 is the tag name).</param>
    /// <param name="cultureName">Culture for date formatting; empty or unknown = invariant.</param>
    /// <returns>The rendered text, or an empty string if a token cannot be resolved.</returns>
    public static string Render(string template, string[] tagParts, string? cultureName)
    {
        var culture = CultureInfo.InvariantCulture;
        if (!string.IsNullOrWhiteSpace(cultureName))
        {
            try
            {
                culture = CultureInfo.GetCultureInfo(cultureName);
            }
            catch (CultureNotFoundException)
            {
                // keep invariant
            }
        }

        DateTime? date = null;
        for (var i = 1; i < tagParts.Length; i++)
        {
            if (DateTime.TryParseExact(tagParts[i].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                date = parsed;
                break;
            }
        }

        var unresolved = false;
        var result = TokenRegex().Replace(template, match =>
        {
            if (match.Groups["idx"].Success)
            {
                var index = int.Parse(match.Groups["idx"].Value, CultureInfo.InvariantCulture);
                if (index < tagParts.Length)
                {
                    return tagParts[index].Trim();
                }

                unresolved = true;
                return string.Empty;
            }

            if (date == null)
            {
                unresolved = true;
                return string.Empty;
            }

            var format = match.Groups["fmt"].Success && match.Groups["fmt"].Length > 0 ? match.Groups["fmt"].Value : "d";
            // "/" in a custom format would become the culture's date separator ("." for de-DE)
            var formatted = date.Value.ToString(format.Replace("/", "\\/"), culture);

            // Abbreviations such as "Nov." / "Di." look odd in a banner
            formatted = TrailingPeriodRegex().Replace(formatted, string.Empty);
            return match.Groups["name"].Value == "DATE" ? formatted.ToUpper(culture) : formatted;
        });

        return unresolved ? string.Empty : result.Trim();
    }
}
