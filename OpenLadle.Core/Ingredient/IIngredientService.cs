namespace OpenLadle.Core.Ingredient;

public interface IIngredientService
{
    Task<IngredientEntity> Create(IngredientEntity ingredient);
    Task Delete(Guid id);
    Task<IngredientEntity?> Retrieve(Guid id);
    Task<IngredientEntity> Update(Guid id, IngredientEntity ingredient);
}