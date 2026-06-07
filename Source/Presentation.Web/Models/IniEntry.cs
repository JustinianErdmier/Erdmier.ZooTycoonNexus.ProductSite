namespace Erdmier.ZooTycoonNexus.Presentation.Web.Models;

/// <summary>Represents a single key-value row in a feature card's INI snippet preview.</summary>
/// <param name="Key">The INI key name, always rendered in amber.</param>
/// <param name="Value">The INI value text.</param>
/// <param name="Kind">
/// The display kind, which determines the CSS colour token applied to the value span.
/// Use <see cref="IniEntryKinds.Ok"/> for confirmed values, <see cref="IniEntryKinds.Value"/> for
/// regular config values, and <see cref="IniEntryKinds.Key"/> for key-path-style values.
/// </param>
public sealed record IniEntry(string Key, string Value, IniEntryKinds Kind);
