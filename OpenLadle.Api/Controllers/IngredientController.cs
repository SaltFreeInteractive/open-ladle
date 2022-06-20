using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenLadle.Api.Models.Ingredient;
using OpenLadle.Core.Exceptions;
using OpenLadle.Core.Ingredient;
using ILogger = Serilog.ILogger;

namespace OpenLadle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngredientController : ControllerBase
{
    private readonly ILogger logger;
    private readonly IMapper mapper;
    private readonly IIngredientService ingredientService;

    public IngredientController(ILogger logger, IMapper mapper, IIngredientService ingredientService)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.ingredientService = ingredientService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIngredientRequest createIngredientRequest)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingredient = await ingredientService.Create(mapper.Map<IngredientEntity>(createIngredientRequest));

            return Ok(mapper.Map<CreateIngredientResponse>(ingredient));
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
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIngredientRequest updateIngredientRequest)
    {
        try
        {
            var ingredient = await ingredientService.Update(id, mapper.Map<IngredientEntity>(updateIngredientRequest));

            return Ok(mapper.Map<UpdateIngredientResponse>(ingredient));
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
