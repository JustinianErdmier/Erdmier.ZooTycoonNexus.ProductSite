namespace Erdmier.ZooTycoonNexus.Presentation.Web.Models;

/// <summary>Represents a way to contribute to the project, displayed as a chip in the Contribute section.</summary>
/// <param name="Icon">The image path for the contribution-type icon.</param>
/// <param name="Title">The label for the contribution type.</param>
public sealed record HelpWayItem(string Icon, string Title);
