namespace Resonance.App.Models;

public class Goal
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public string Text { get; set; } = string.Empty;

    public GoalStatus Status { get; set; } = GoalStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}