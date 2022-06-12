using AutoMapper;

namespace OpenLadle.Shared.IngredientModels;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<IngredientCreateViewModel, Ingredient>();
    }
}
