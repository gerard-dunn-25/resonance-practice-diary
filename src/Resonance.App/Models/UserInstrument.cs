namespace Resonance.App.Models;

public class UserInstrument
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public string InstrumentFamily { get; set; } = string.Empty;
    public string InstrumentName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}