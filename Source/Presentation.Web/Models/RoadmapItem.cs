namespace Erdmier.ZooTycoonNexus.Presentation.Web.Models;

/// <summary>Represents a card on the product roadmap.</summary>
/// <param name="Icon">The image path for the critter illustration displayed on the card.</param>
/// <param name="Status">The current status of the roadmap item.</param>
/// <param name="Title">The roadmap item title. May contain HTML entities (e.g. <c>&amp;amp;</c>).</param>
/// <param name="Description">A brief description of the planned feature. May contain inline HTML (e.g. <c>&lt;code&gt;</c> tags).</param>
/// <param name="BulletPoints">Feature bullet points. Items may contain inline HTML and are rendered as <c>MarkupString</c>.</param>
public sealed record RoadmapItem(
    string Icon,
    RoadmapStatuses Status,
    string Title,
    string Description,
    IReadOnlyList<string> BulletPoints);
