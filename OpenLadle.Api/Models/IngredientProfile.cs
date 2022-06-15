using AutoMapper;
using OpenLadle.Shared.IngredientModels;

namespace OpenLadle.Api.Models;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<CreateIngredientRequest, Ingredient>();

        CreateMap<Ingredient, CreateIngredientResponse>();

        CreateMap<UpdateIngredientRequest, Ingredient>();

        CreateMap<Ingredient, UpdateIngredientResponse>();
    }
}
