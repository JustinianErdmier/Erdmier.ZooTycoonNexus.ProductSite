namespace Erdmier.ZooTycoonNexus.Presentation.Web.Models;

/// <summary>Represents a known problem that Zoo Tycoon Nexus solves, displayed as a card in the Problems section.</summary>
/// <param name="Title">The short title of the problem.</param>
/// <param name="Description">A brief description of the issue players face.</param>
/// <param name="Fix">A description of how Nexus resolves the problem.</param>
public sealed record ProblemItem(string Title, string Description, string Fix);
