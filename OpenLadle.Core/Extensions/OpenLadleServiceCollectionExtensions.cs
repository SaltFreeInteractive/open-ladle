using OpenLadle.Core.Abstractions;
using OpenLadle.Core.Services;
using OpenLadle.Shared.UserModels;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenLadleServiceCollectionExtensions
{
    public static IServiceCollection AddOpenLadle(this IServiceCollection services)
    {
        services.AddScoped<IIngredientService, IngredientService>();

        return services;
    }
}
