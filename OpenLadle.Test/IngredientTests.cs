using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Abstractions;
using OpenLadle.Core.Exceptions;
using OpenLadle.Data;
using OpenLadle.Shared.IngredientModels;

namespace OpenLadle.Test;

public class IngredientTests
{
    private ApplicationDbContext testDbContext = null!;
    private IMapper testMapper = null!;
    private IIngredientService ingredientService = null!;

    [SetUp]
    public async Task Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "test-data")
            .Options;
        testDbContext = new ApplicationDbContext(dbContextOptions);
        await testDbContext.Database.EnsureCreatedAsync();

        var mapperConfiguration = new MapperConfiguration(mapperConfigurationOptions =>
        {
            mapperConfigurationOptions.AddProfile<IngredientProfile>();
        });
        testMapper = mapperConfiguration.CreateMapper();

        ingredientService = new IngredientService(testDbContext, testMapper);
    }

    [TearDown]
    public void TearDown()
    {
        testDbContext.Dispose();
    }

    [Test]
    public async Task Create_ValidInput_ReturnCreatedResource()
    {
        var validInput = new IngredientCreateRequest
        {
            Name = "Create_ValidInput"
        };

        var result = await ingredientService.Create(validInput);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IngredientViewModel>());
        Assert.That(result.Name, Is.EqualTo(validInput.Name));
    }

    [Test]
    public async Task Create_InputWithExistingName_ThrowException()
    {
        var existingIngredient = new Ingredient
        {
            Name = "Create_InputWithExistingName"
        };
        await testDbContext.Ingredients.AddAsync(existingIngredient);
        await testDbContext.SaveChangesAsync();

        var inputWithExistingName = new IngredientCreateRequest
        {
            Name = existingIngredient.Name
        };

        Assert.ThrowsAsync<ResourceAlreadyExistsException>(async () => await ingredientService.Create(inputWithExistingName));
    }

    [Test]
    public async Task Retrieve_ValidId_ReturnsResource()
    {
        var existingIngredient = new Ingredient
        {
            Name = "Retrieve_ValidId"
        };
        await testDbContext.Ingredients.AddAsync(existingIngredient);
        await testDbContext.SaveChangesAsync();

        var result = await ingredientService.Retrieve(existingIngredient.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IngredientViewModel>());
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
        var existingIngredient = new Ingredient
        {
            Name = "Update_ValidIdValidParameters"
        };
        await testDbContext.Ingredients.AddAsync(existingIngredient);
        await testDbContext.SaveChangesAsync();

        var validInput = new IngredientUpdateRequest
        {
            Name = "Changed_Update_ValidIdValidParameters"
        };

        var result = await ingredientService.Update(existingIngredient.Id, validInput);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IngredientViewModel>());
        Assert.That(result.Name, Is.EqualTo(validInput.Name));
    }

    [Test]
    public void Update_InvalidIdValidInput_ThrowsException()
    {
        var invalidId = Guid.NewGuid();

        var validInput = new IngredientUpdateRequest
        {
            Name = "Update_InvalidIdValidInput"
        };

        Assert.ThrowsAsync<ResourceDoesNotExistException>(async () => await ingredientService.Update(invalidId, validInput));
    }

    [Test]
    public async Task Update_ValidIdInputWithExistingName_ThrowsException()
    {
        var firstExistingIngredient = new Ingredient
        {
            Name = "First_Update_ValidIdInputWithExistingName"
        };
        await testDbContext.Ingredients.AddAsync(firstExistingIngredient);
        var secondExistingIngredient = new Ingredient
        {
            Name = "Second_Update_ValidIdInputWithExistingName"
        };
        await testDbContext.Ingredients.AddAsync(secondExistingIngredient);
        await testDbContext.SaveChangesAsync();

        var inputWithExistingName = new IngredientUpdateRequest
        {
            Name = secondExistingIngredient.Name
        };

        Assert.ThrowsAsync<ResourceAlreadyExistsException>(async () => await ingredientService.Update(firstExistingIngredient.Id, inputWithExistingName));
    }

    [Test]
    public async Task Delete_ValidId_ReturnsNothing()
    {
        var existingIngredient = new Ingredient
        {
            Name = "Delete_ValidId"
        };
        await testDbContext.Ingredients.AddAsync(existingIngredient);
        await testDbContext.SaveChangesAsync();

        await ingredientService.Delete(existingIngredient.Id);

        var result = await testDbContext.Ingredients.FindAsync(existingIngredient.Id);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void Delete_InvalidId_ThrowsException()
    {
        var inlavidId = Guid.NewGuid();

        Assert.ThrowsAsync<ResourceDoesNotExistException>(async () => await ingredientService.Delete(inlavidId));
    }
}