using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenLadle.Shared.IngredientModels;
using OpenLadle.Shared.UserModels;

namespace OpenLadle.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Ingredient>(entity =>
        {
            entity.HasIndex(ingredient => ingredient.Name)
                .IsUnique();
        });
    }
}
