namespace Jellyfin.Plugin.JellyTag.Services;

/// <summary>
/// Badge category types.
/// </summary>
public enum BadgeCategory
{
    /// <summary>
    /// Resolution badge (4K, 1080p, 720p, SD).
    /// </summary>
    Resolution,

    /// <summary>
    /// HDR badge (HDR10, HDR10+, Dolby Vision, HLG).
    /// </summary>
    Hdr,

    /// <summary>
    /// Audio codec badge (Atmos, DTS-X, TrueHD, etc.).
    /// </summary>
    Audio,

    /// <summary>
    /// 3D content badge.
    /// </summary>
    ThreeD,

    /// <summary>
    /// Video codec badge (HEVC, AV1, VP9).
    /// </summary>
    VideoCodec,

    /// <summary>
    /// Language badge (audio language).
    /// </summary>
    Language,

    /// <summary>
    /// Subtitle indicator badge (VOSTFR, VOSTEN, etc.).
    /// </summary>
    Subtitle,

    /// <summary>
    /// Rating badge (community rating, critic rating).
    /// </summary>
    Rating,

    /// <summary>
    /// Edition badge parsed from the file name (Plex naming, e.g. {edition-IMAX}).
    /// </summary>
    Edition
}

/// <summary>
/// Represents a detected badge to overlay on an image.
/// </summary>
public class BadgeInfo
{
    /// <summary>
    /// Gets or sets the badge category.
    /// </summary>
    public BadgeCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the badge key (unique identifier, e.g. "4k", "hdr10", "atmos").
    /// </summary>
    public string BadgeKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the embedded resource file name (e.g. "badge-4k.png").
    /// </summary>
    public string ResourceFileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets literal text to render for dynamic badges (e.g. "7.8"). Takes precedence over key-based text.
    /// </summary>
    public string? DisplayText { get; set; }
}
