using OpenLadle.Shared.IngredientModels;

namespace OpenLadle.Core.Abstractions
{
    public interface IIngredientService
    {
        Task<Ingredient> Create(Ingredient ingredient);
        Task Delete(Guid id);
        Task<Ingredient?> Retrieve(Guid id);
        Task<Ingredient> Update(Guid id, Ingredient ingredient);
    }
}