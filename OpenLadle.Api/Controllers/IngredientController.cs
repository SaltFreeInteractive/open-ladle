using Microsoft.AspNetCore.Mvc;
using OpenLadle.Core.Exceptions;
using OpenLadle.Core.Services;
using OpenLadle.Shared.IngredientModels;
using ILogger = Serilog.ILogger;

namespace OpenLadle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngredientController : ControllerBase
{
    private readonly ILogger logger;
    private readonly IngredientService ingredientService;

    public IngredientController(ILogger logger, IngredientService ingredientService)
    {
        this.logger = logger;
        this.ingredientService = ingredientService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IngredientCreateViewModel ingredientCreateViewModel)
    {
        try
        {
            return Ok(await ingredientService.Create(ingredientCreateViewModel));
        }
        catch(ResourceAlreadyExistsException exception)
        {
            logger.Error("{Message}", exception.Message);
            return BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            logger.Error("{Message}", exception.Message);
            return Problem();
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Retrieve(Guid id)
    {
        try
        {
            var result = await ingredientService.Retrieve(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception exception)
        {
            logger.Error("{Message}", exception.Message);
            return Problem();
        }
    }
}
