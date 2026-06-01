namespace Resonance.App.Models;

public class SessionTag
{
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;

    public int FocusTagId { get; set; }
    public FocusTag FocusTag { get; set; } = null!;
}