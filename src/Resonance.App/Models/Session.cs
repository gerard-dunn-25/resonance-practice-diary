namespace Resonance.App.Models;

public class Session
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int UserInstrumentId { get; set; }
    public UserInstrument UserInstrument { get; set; } = null!;

    public DateOnly Date { get; set; }

    public int DurationMinutes { get; set; }

    public string Reflection { get; set; } = string.Empty;

    public int? MoodTagId { get; set; }
    public MoodTag? MoodTag { get; set; }

    public ICollection<SessionTag> SessionTags { get; set; } =
        new List<SessionTag>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}