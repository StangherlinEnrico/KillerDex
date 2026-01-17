using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IKillerService, KillerService>();
        services.AddScoped<ISurvivorService, SurvivorService>();
        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<IPerkService, PerkService>();
        services.AddScoped<IKillerAddonService, KillerAddonService>();
        services.AddScoped<ISurvivorAddonService, SurvivorAddonService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IOfferingService, OfferingService>();
        services.AddScoped<IRealmService, RealmService>();
        services.AddScoped<IMapService, MapService>();
        services.AddScoped<IStatusEffectService, StatusEffectService>();

        return services;
    }
}
