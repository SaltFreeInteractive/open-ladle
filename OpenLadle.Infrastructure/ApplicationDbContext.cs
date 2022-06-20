using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Ingredient;
using OpenLadle.Core.User;

namespace OpenLadle.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{
    public DbSet<IngredientEntity> Ingredients { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IngredientEntity >(entity =>
        {
            entity.HasIndex(ingredient => ingredient.Name)
                .IsUnique();
        });
    }
}
