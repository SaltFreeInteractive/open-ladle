using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Abstractions;
using OpenLadle.Core.Exceptions;
using OpenLadle.Data;
using OpenLadle.Shared.IngredientModels;

namespace OpenLadle.Core.Services;

public class IngredientService : IIngredientService
{
    private readonly ApplicationDbContext applicationDbContext;

    public IngredientService(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public async Task<Ingredient> Create(Ingredient ingredient)
    {
        if (await IngredientWithNameExists(ingredient.Name))
        {
            throw new ResourceAlreadyExistsException("An ingredient by this name already exists.");
        }

        await applicationDbContext.Ingredients.AddAsync(ingredient);
        await applicationDbContext.SaveChangesAsync();

        return ingredient;
    }

    public async Task<Ingredient?> Retrieve(Guid id)
    {
        return await applicationDbContext.Ingredients.FindAsync(id);
    }

    public async Task<Ingredient> Update(Guid id, Ingredient ingredient)
    {
        if (await IngredientWithNameExists(ingredient.Name))
        {
            throw new ResourceAlreadyExistsException("An ingredient by this name already exists.");
        }

        var result = await applicationDbContext.Ingredients.FindAsync(id);

        if (result == null)
        {
            throw new ResourceDoesNotExistException("An ingredient with this Id was not found.");
        }

        result.Name = ingredient.Name;

        await applicationDbContext.SaveChangesAsync();

        return result;
    }

    public async Task Delete(Guid id)
    {
        var result = await applicationDbContext.Ingredients.FindAsync(id);

        if (result == null)
        {
            throw new ResourceDoesNotExistException("An ingredient with this Id was not found.");
        }

        applicationDbContext.Remove(result);
        await applicationDbContext.SaveChangesAsync();
    }

    private async Task<bool> IngredientWithNameExists(string name)
    {
        return await applicationDbContext.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.Name == name) != null;
    }
}
