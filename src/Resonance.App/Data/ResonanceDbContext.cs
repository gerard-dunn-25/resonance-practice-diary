using Microsoft.EntityFrameworkCore;
using Resonance.App.Models;

namespace Resonance.App.Data;

public class ResonanceDbContext : DbContext
{
    public ResonanceDbContext(DbContextOptions<ResonanceDbContext> options)
        : base(options)
    {
    }

    public DbSet<PracticePlaceholder> PracticePlaceholders =>
        Set<PracticePlaceholder>();
}