namespace Resonance.App.Models;

public class MoodTag
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;

    public bool IsSystem { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}