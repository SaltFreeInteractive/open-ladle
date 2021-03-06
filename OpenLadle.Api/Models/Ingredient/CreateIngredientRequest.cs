using System.ComponentModel.DataAnnotations;

namespace OpenLadle.Api.Models.Ingredient;

public class CreateIngredientRequest
{
    [Required]
    public string Name { get; set; } = null!;
}
