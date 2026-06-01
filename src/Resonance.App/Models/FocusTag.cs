namespace Resonance.App.Models;

public class FocusTag
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public ICollection<SessionTag> SessionTags { get; set; } =
        new List<SessionTag>();
}