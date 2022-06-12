using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Exceptions;
using OpenLadle.Data;
using OpenLadle.Shared.IngredientModels;

namespace OpenLadle.Test;

public class IngredientTests
{
    private ApplicationDbContext testDbContext = null!;
    private IMapper testMapper = null!;
    private IngredientService ingredientService = null!;

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
        var validInput = new IngredientCreateViewModel
        {
            Name = "Apple"
        };

        var result = await ingredientService.Create(validInput);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(validInput.Name));
    }

    [Test]
    public async Task Create_InputWithExistingName_ThrowException()
    {
        var existingIngredient = new Ingredient
        {
            Name = "Carrot"
        };
        await testDbContext.Ingredients.AddAsync(existingIngredient);
        await testDbContext.SaveChangesAsync();

        var inputWithExistingName = new IngredientCreateViewModel
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
            Name = "Grape"
        };
        await testDbContext.Ingredients.AddAsync(existingIngredient);
        await testDbContext.SaveChangesAsync();

        var result = await ingredientService.Retrieve(existingIngredient.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(existingIngredient.Name));
    }

    [Test]
    public async Task Retrieve_InvalidId_ReturnsNull()
    {
        var invalidId = Guid.NewGuid();

        var result = await ingredientService.Retrieve(invalidId);

        Assert.That(result, Is.Null);
    }
}