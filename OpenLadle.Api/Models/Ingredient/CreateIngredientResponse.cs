namespace OpenLadle.Api.Models.Ingredient;

public class CreateIngredientResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
