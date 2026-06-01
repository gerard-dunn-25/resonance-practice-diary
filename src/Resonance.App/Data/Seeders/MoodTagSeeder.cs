using Microsoft.EntityFrameworkCore;
using Resonance.App.Models;

namespace Resonance.App.Data.Seeders;

public static class MoodTagSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ResonanceDbContext>();

        await EnsureMoodTagAsync(db, "In the zone", "#818CF8");
        await EnsureMoodTagAsync(db, "Grinding", "#FB923C");
        await EnsureMoodTagAsync(db, "Distracted", "#94A3B8");
        await EnsureMoodTagAsync(db, "Frustrated", "#F87171");
        await EnsureMoodTagAsync(db, "Surprised myself", "#34D399");
    }

    private static async Task EnsureMoodTagAsync(
        ResonanceDbContext db,
        string label,
        string colour)
    {
        var exists = await db.MoodTags.AnyAsync(t =>
            t.IsSystem && t.Label == label);

        if (exists)
        {
            return;
        }

        db.MoodTags.Add(
            new MoodTag
            {
                Label = label,
                Colour = colour,
                IsSystem = true
            });

        await db.SaveChangesAsync();
    }
}