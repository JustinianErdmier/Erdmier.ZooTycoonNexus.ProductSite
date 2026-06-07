namespace Erdmier.ZooTycoonNexus.Presentation.Web.Common.Extensions;

/// <summary>Extension methods for <see cref="IniEntryKinds"/>.</summary>
public static class IniEntryKindsExtensions
{
    /// <summary>Maps an <see cref="IniEntryKinds"/> value to its corresponding CSS class token for use in a feature card's INI snippet.</summary>
    /// <param name="kind">The INI entry kind to map.</param>
    /// <returns>
    /// <c>"k"</c> for <see cref="IniEntryKinds.Key"/>;
    /// <c>"ok"</c> for <see cref="IniEntryKinds.Ok"/>;
    /// <c>"v"</c> for <see cref="IniEntryKinds.Value"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="kind"/> is not a recognised value.</exception>
    public static string MapToCssToken(this IniEntryKinds kind) => kind switch
    {
        IniEntryKinds.Key   => "k",
        IniEntryKinds.Ok    => "ok",
        IniEntryKinds.Value => "v",
        _                   => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
    };
}
