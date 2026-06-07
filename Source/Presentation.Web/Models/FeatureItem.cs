namespace Erdmier.ZooTycoonNexus.Presentation.Web.Models;

/// <summary>Represents a feature card displayed in the Features section.</summary>
/// <param name="Icon">The image path for the Win95-style feature icon.</param>
/// <param name="Title">The feature title.</param>
/// <param name="Description">A short description of what the feature does.</param>
/// <param name="Entries">INI snippet rows rendered beneath the description. Pass an empty collection when no snippet should be shown.</param>
public sealed record FeatureItem(string                  Icon,
                                 string                  Title,
                                 string                  Description,
                                 IReadOnlyList<IniEntry> Entries);
