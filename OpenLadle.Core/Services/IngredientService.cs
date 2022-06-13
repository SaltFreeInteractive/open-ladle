using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Abstractions;
using OpenLadle.Core.Exceptions;
using OpenLadle.Data;
using OpenLadle.Shared.IngredientModels;

namespace OpenLadle.Core.Services;

public class IngredientService : IIngredientService
{
    private readonly ApplicationDbContext applicationDbContext;
    private readonly IMapper mapper;

    public IngredientService(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        this.applicationDbContext = applicationDbContext;
        this.mapper = mapper;
    }

    public async Task<IngredientViewModel> Create(IngredientCreateRequest ingredientCreateRequest)
    {
        if (await IngredientWithNameExists(ingredientCreateRequest.Name))
        {
            throw new ResourceAlreadyExistsException("An ingredient by this name already exists.");
        }

        var newIngredient = mapper.Map<Ingredient>(ingredientCreateRequest);

        await applicationDbContext.Ingredients.AddAsync(newIngredient);
        await applicationDbContext.SaveChangesAsync();

        return mapper.Map<IngredientViewModel>(newIngredient);
    }

    public async Task<IngredientViewModel?> Retrieve(Guid id)
    {
        return mapper.Map<IngredientViewModel>(await applicationDbContext.Ingredients.FindAsync(id));
    }

    public async Task<IngredientViewModel> Update(Guid id, IngredientUpdateRequest ingredientUpdateRequest)
    {
        if (await IngredientWithNameExists(ingredientUpdateRequest.Name))
        {
            throw new ResourceAlreadyExistsException("An ingredient by this name already exists.");
        }

        var result = await applicationDbContext.Ingredients.FindAsync(id);

        if (result == null)
        {
            throw new ResourceDoesNotExistException("An ingredient with this Id was not found.");
        }

        mapper.Map(ingredientUpdateRequest, result);

        await applicationDbContext.SaveChangesAsync();

        return mapper.Map<IngredientViewModel>(result);
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
