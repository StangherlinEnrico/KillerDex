using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DbdContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(DbdContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            }));

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register repositories
        services.AddScoped<IKillerRepository, KillerRepository>();
        services.AddScoped<ISurvivorRepository, SurvivorRepository>();
        services.AddScoped<IChapterRepository, ChapterRepository>();
        services.AddScoped<IPerkRepository, PerkRepository>();
        services.AddScoped<IKillerAddonRepository, KillerAddonRepository>();
        services.AddScoped<ISurvivorAddonRepository, SurvivorAddonRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IOfferingRepository, OfferingRepository>();
        services.AddScoped<IRealmRepository, RealmRepository>();
        services.AddScoped<IMapRepository, MapRepository>();
        services.AddScoped<IStatusEffectRepository, StatusEffectRepository>();

        return services;
    }
}
