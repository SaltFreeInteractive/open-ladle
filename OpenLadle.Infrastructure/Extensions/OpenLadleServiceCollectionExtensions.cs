using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Ingredient;
using OpenLadle.Infrastructure;
using OpenLadle.Infrastructure.Extensions;
using OpenLadle.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenLadleServiceCollectionExtensions
{
    public static IServiceCollection AddOpenLadle(this IServiceCollection services, Action<OpenLadleOptions> options)
    {
        var optionsResult = new OpenLadleOptions();
        options.Invoke(optionsResult);

        services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(
                optionsResult.ConnectionString,
                ServerVersion.AutoDetect(optionsResult.ConnectionString)
            );
        });

        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IIngredientService, IngredientService>();

        return services;
    }
}
