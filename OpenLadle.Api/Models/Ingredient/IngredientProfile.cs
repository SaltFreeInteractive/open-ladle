using AutoMapper;
using OpenLadle.Core.Ingredient;

namespace OpenLadle.Api.Models.Ingredient;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<CreateIngredientRequest, IngredientEntity>();

        CreateMap<IngredientEntity, CreateIngredientResponse>();

        CreateMap<UpdateIngredientRequest, IngredientEntity>();

        CreateMap<IngredientEntity, UpdateIngredientResponse>();
    }
}
