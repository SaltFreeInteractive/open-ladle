using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Exceptions;
using OpenLadle.Data;
using OpenLadle.Shared.IngredientModels;

namespace OpenLadle.Core.Services;

public class IngredientService
{
    private readonly ApplicationDbContext applicationDbContext;
    private readonly IMapper mapper;

    public IngredientService(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        this.applicationDbContext = applicationDbContext;
        this.mapper = mapper;
    }

    public async Task<Ingredient> Create(IngredientCreateViewModel ingredientCreateViewModel)
    {
        var newIngredient = mapper.Map<Ingredient>(ingredientCreateViewModel);

        var existingIngredient = await applicationDbContext.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.Name == newIngredient.Name);

        if (existingIngredient != null)
        {
            throw new ResourceAlreadyExistsException("An ingredient by this name already exists.");
        }

        await applicationDbContext.Ingredients.AddAsync(newIngredient);
        await applicationDbContext.SaveChangesAsync();

        return newIngredient;
    }

    public async Task<Ingredient?> Retrieve(Guid id)
    {
        return await applicationDbContext.Ingredients.FindAsync(id);
    }
}
