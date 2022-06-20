using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Exceptions;
using OpenLadle.Core.Ingredient;
using OpenLadle.Infrastructure;
using OpenLadle.Infrastructure.Repositories;

namespace OpenLadle.Test;

public class IngredientTests
{
    private ApplicationDbContext testDbContext = null!;
    private IIngredientRepository ingredientRepository = null!;
    private IIngredientService ingredientService = null!;

    [SetUp]
    public async Task Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "test-data")
            .Options;
        testDbContext = new ApplicationDbContext(dbContextOptions);
        await testDbContext.Database.EnsureCreatedAsync();

        ingredientRepository = new IngredientRepository(testDbContext);

        ingredientService = new IngredientService(ingredientRepository);
    }

    [TearDown]
    public void TearDown()
    {
        testDbContext.Dispose();
    }

    [Test]
    public async Task Create_ValidInput_ReturnCreatedResource()
    {
        var validInput = new IngredientEntity
        {
            Name = "Create_ValidInput"
        };

        var result = await ingredientService.Create(validInput);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IngredientEntity>());
        Assert.That(result.Name, Is.EqualTo(validInput.Name));
    }

    [Test]
    public async Task Create_InputWithExistingName_ThrowException()
    {
        var existingIngredient = new IngredientEntity
        {
            Name = "Create_InputWithExistingName"
        };
        await ingredientRepository.AddAsync(existingIngredient);

        var inputWithExistingName = new IngredientEntity
        {
            Name = existingIngredient.Name
        };

        Assert.ThrowsAsync<ResourceAlreadyExistsException>(async () => await ingredientService.Create(inputWithExistingName));
    }

    [Test]
    public async Task Retrieve_ValidId_ReturnsResource()
    {
        var existingIngredient = new IngredientEntity
        {
            Name = "Retrieve_ValidId"
        };
        await ingredientRepository.AddAsync(existingIngredient);

        var result = await ingredientService.Retrieve(existingIngredient.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IngredientEntity>());
        Assert.That(result.Name, Is.EqualTo(existingIngredient.Name));
    }

    [Test]
    public async Task Retrieve_InvalidId_ReturnsNull()
    {
        var invalidId = Guid.NewGuid();

        var result = await ingredientService.Retrieve(invalidId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Update_ValidIdValidInput_ReturnsUpdatedResource()
    {
        var existingIngredient = new IngredientEntity
        {
            Name = "Update_ValidIdValidParameters"
        };
        await ingredientRepository.AddAsync(existingIngredient);

        var validInput = new IngredientEntity
        {
            Name = "Changed_Update_ValidIdValidParameters"
        };

        var result = await ingredientService.Update(existingIngredient.Id, validInput);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IngredientEntity>());
        Assert.That(result.Name, Is.EqualTo(validInput.Name));
    }

    [Test]
    public void Update_InvalidIdValidInput_ThrowsException()
    {
        var invalidId = Guid.NewGuid();

        var validInput = new IngredientEntity
        {
            Name = "Update_InvalidIdValidInput"
        };

        Assert.ThrowsAsync<ResourceDoesNotExistException>(async () => await ingredientService.Update(invalidId, validInput));
    }

    [Test]
    public async Task Update_ValidIdInputWithExistingName_ThrowsException()
    {
        var firstExistingIngredient = new IngredientEntity
        {
            Name = "First_Update_ValidIdInputWithExistingName"
        };
        await ingredientRepository.AddAsync(firstExistingIngredient);
        var secondExistingIngredient = new IngredientEntity
        {
            Name = "Second_Update_ValidIdInputWithExistingName"
        };
        await ingredientRepository.AddAsync(secondExistingIngredient);

        var inputWithExistingName = new IngredientEntity
        {
            Name = secondExistingIngredient.Name
        };

        Assert.ThrowsAsync<ResourceAlreadyExistsException>(async () => await ingredientService.Update(firstExistingIngredient.Id, inputWithExistingName));
    }

    [Test]
    public async Task Delete_ValidId_ReturnsNothing()
    {
        var existingIngredient = new IngredientEntity
        {
            Name = "Delete_ValidId"
        };
        await ingredientRepository.AddAsync(existingIngredient);

        await ingredientService.Delete(existingIngredient.Id);

        var result = await ingredientRepository.GetByIdAsync(existingIngredient.Id);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void Delete_InvalidId_ThrowsException()
    {
        var inlavidId = Guid.NewGuid();

        Assert.ThrowsAsync<ResourceDoesNotExistException>(async () => await ingredientService.Delete(inlavidId));
    }
}