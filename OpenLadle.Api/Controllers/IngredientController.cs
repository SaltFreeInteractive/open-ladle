using Microsoft.AspNetCore.Mvc;
using OpenLadle.Core.Abstractions;
using OpenLadle.Core.Exceptions;
using OpenLadle.Shared.IngredientModels;
using ILogger = Serilog.ILogger;

namespace OpenLadle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngredientController : ControllerBase
{
    private readonly ILogger logger;
    private readonly IIngredientService ingredientService;

    public IngredientController(ILogger logger, IIngredientService ingredientService)
    {
        this.logger = logger;
        this.ingredientService = ingredientService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IngredientCreateRequest ingredientCreateViewModel)
    {
        try
        {
            return Ok(await ingredientService.Create(ingredientCreateViewModel));
        }
        catch(ResourceAlreadyExistsException exception)
        {
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] IngredientUpdateRequest ingredientUpdateRequest)
    {
        try
        {
            return Ok(await ingredientService.Update(id, ingredientUpdateRequest));
        }
        catch (ResourceDoesNotExistException)
        {
            return NotFound();
        }
        catch (ResourceAlreadyExistsException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (Exception exception)
        {
            logger.Error("{Message}", exception.Message);
            return Problem();
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await ingredientService.Delete(id);
            return Ok();
        }
        catch (ResourceDoesNotExistException)
        {
            return NotFound();
        }
        catch (Exception exception)
        {
            logger.Error("{Message}", exception.Message);
            return Problem();
        }
    }
}
