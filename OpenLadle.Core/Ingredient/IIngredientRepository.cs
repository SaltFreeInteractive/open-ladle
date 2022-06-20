using OpenLadle.Core.Abstractions;

namespace OpenLadle.Core.Ingredient;

public interface IIngredientRepository : IRepository<IngredientEntity>
{
    Task<IngredientEntity?> GetByNameAsync(string name);
}
