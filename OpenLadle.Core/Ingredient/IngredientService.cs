using OpenLadle.Core.Exceptions;

namespace OpenLadle.Core.Ingredient;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository ingredientRepository;

    public IngredientService(IIngredientRepository ingredientRepository)
    {
        this.ingredientRepository = ingredientRepository;
    }

    public async Task<IngredientEntity> Create(IngredientEntity ingredient)
    {
        if (await IngredientWithNameExists(ingredient.Name))
        {
            throw new ResourceAlreadyExistsException("An ingredient by this name already exists.");
        }

        await ingredientRepository.AddAsync(ingredient);

        return ingredient;
    }

    public async Task<IngredientEntity?> Retrieve(Guid id)
    {
        return await ingredientRepository.GetByIdAsync(id);
    }

    public async Task<IngredientEntity> Update(Guid id, IngredientEntity ingredient)
    {
        if (await IngredientWithNameExists(ingredient.Name))
        {
            throw new ResourceAlreadyExistsException("An ingredient by this name already exists.");
        }

        var result = await ingredientRepository.GetByIdAsync(id);

        if (result == null)
        {
            throw new ResourceDoesNotExistException("An ingredient with this Id was not found.");
        }

        result.Name = ingredient.Name;

        await ingredientRepository.EditAsync(result);

        return result;
    }

    public async Task Delete(Guid id)
    {
        var result = await ingredientRepository.GetByIdAsync(id);

        if (result == null)
        {
            throw new ResourceDoesNotExistException("An ingredient with this Id was not found.");
        }

        await ingredientRepository.DeleteAsync(result);
    }

    private async Task<bool> IngredientWithNameExists(string name)
    {
        return await ingredientRepository.GetByNameAsync(name) != null;
    }
}
