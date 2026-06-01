using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resonance.App.Models;

namespace Resonance.App.Data;

public class ResonanceDbContext : IdentityDbContext<ApplicationUser>
{
    public ResonanceDbContext(
        DbContextOptions<ResonanceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<MoodTag> MoodTags => Set<MoodTag>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<SessionTag> SessionTags => Set<SessionTag>();
    public DbSet<FocusTag> FocusTags => Set<FocusTag>();
    public DbSet<UserInstrument> UserInstruments => Set<UserInstrument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SessionTag>()
            .HasKey(st => new { st.SessionId, st.FocusTagId });

        modelBuilder.Entity<UserInstrument>()
            .HasOne(ui => ui.User)
            .WithMany()
            .HasForeignKey(ui => ui.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<FocusTag>()
            .HasOne(ft => ft.User)
            .WithMany()
            .HasForeignKey(ft => ft.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.UserInstrument)
            .WithMany()
            .HasForeignKey(s => s.UserInstrumentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.MoodTag)
            .WithMany()
            .HasForeignKey(s => s.MoodTagId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<SessionTag>()
            .HasOne(st => st.Session)
            .WithMany(s => s.SessionTags)
            .HasForeignKey(st => st.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SessionTag>()
            .HasOne(st => st.FocusTag)
            .WithMany(ft => ft.SessionTags)
            .HasForeignKey(st => st.FocusTagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}