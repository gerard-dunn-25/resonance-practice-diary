using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resonance.App.Models;

namespace Resonance.App.Data;

public class ResonanceDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ResonanceDbContext(
        DbContextOptions<ResonanceDbContext> options)
        : base(options)
    {
    }
}