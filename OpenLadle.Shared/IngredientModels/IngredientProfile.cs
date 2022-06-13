using AutoMapper;

namespace OpenLadle.Shared.IngredientModels;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<IngredientCreateRequest, Ingredient>();

        CreateMap<IngredientUpdateRequest, Ingredient>();

        CreateMap<Ingredient, IngredientViewModel>();
    }
}
