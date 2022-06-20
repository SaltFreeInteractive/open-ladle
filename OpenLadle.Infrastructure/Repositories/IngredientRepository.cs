using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Ingredient;

namespace OpenLadle.Infrastructure.Repositories;

public class IngredientRepository : Repository<IngredientEntity>, IIngredientRepository
{
    public IngredientRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {

    }

    public async Task<IngredientEntity?> GetByNameAsync(string name)
    {
        return await applicationDbContext.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.Name == name);
    }
}
