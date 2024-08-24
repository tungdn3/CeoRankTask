using CeoRankTask.Core.Dtos;
using CeoRankTask.Core.Interfaces;
using CeoRankTask.Core.Services;
using CeoRankTask.Infrastructure.Repositories;
using FluentValidation;

namespace CeoRankTask.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CeoRankRequestDtoValidator>();
        services.AddSingleton<IGoogleExtractor, GoogleExtractor>();
        services.AddScoped<ICeoRankService, CeoRankService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ScraperRepositoryOptions>(configuration.GetSection(ScraperRepositoryOptions.ScraperRepository));
        services.AddHttpClient<IScraperRepository, ScraperRepository>();

        return services;
    }
}
