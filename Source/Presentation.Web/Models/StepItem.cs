namespace Erdmier.ZooTycoonNexus.Presentation.Web.Models;

/// <summary>Represents a step in the "How it works" section.</summary>
/// <param name="Title">The step title.</param>
/// <param name="Description">A brief description of what happens in this step.</param>
public sealed record StepItem(string Title, string Description);
