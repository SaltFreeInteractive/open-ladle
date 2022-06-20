using System.ComponentModel.DataAnnotations;

namespace OpenLadle.Api.Models.Ingredient;

public class UpdateIngredientRequest
{
    [Required]
    public string Name { get; set; } = null!;
}
