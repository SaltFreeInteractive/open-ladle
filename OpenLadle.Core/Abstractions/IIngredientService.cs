using OpenLadle.Shared.IngredientModels;

namespace OpenLadle.Core.Abstractions
{
    public interface IIngredientService
    {
        Task<IngredientViewModel> Create(IngredientCreateRequest ingredientCreateRequest);
        Task Delete(Guid id);
        Task<IngredientViewModel?> Retrieve(Guid id);
        Task<IngredientViewModel> Update(Guid id, IngredientUpdateRequest ingredientUpdateRequest);
    }
}