using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Polly;
using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Interfaces;
using SeoRankTask.Core.Services;
using SeoRankTask.Infrastructure;
using SeoRankTask.Infrastructure.Repositories;

namespace SeoRankTask.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddValidatorsFromAssemblyContaining<CeoRankRequestDtoValidator>()
            .AddSingleton<IExtractor, GoogleExtractor>()
            .AddSingleton<IExtractor, BingExtractor>()
            .AddScoped<ISeoRankService, SeoRankService>()
            .AddScoped<IWatchListService, WatchListService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddGoogleClient(configuration)
            .AddBingClient(configuration)
            .AddSeoRankDbContext(configuration);

        return services;
    }

    private static IServiceCollection AddGoogleClient(this IServiceCollection services, IConfiguration configuration)
    {
        string googleBaseUrl = configuration.GetValue<string>("GoogleBaseUrl")!;
        services
            .AddHttpClient(InfrastructureConstants.HttpClientNames.Google, httpClient =>
            {
                httpClient.BaseAddress = new Uri(googleBaseUrl);
            })
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.RetryAsync(3))
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        services.AddSingleton<IScraperRepository, GoogleClient>();
        return services;
    }

    private static IServiceCollection AddBingClient(this IServiceCollection services, IConfiguration configuration)
    {
        string bingBaseUrl = configuration.GetValue<string>("BingBaseUrl")!;
        services
            .AddHttpClient(InfrastructureConstants.HttpClientNames.Bing, httpClient =>
            {
                httpClient.BaseAddress = new Uri(bingBaseUrl);

                // Bing requires User-Agent header
                httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36");
            })
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.RetryAsync(3))
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        services.AddSingleton<IScraperRepository, BingClient>();
        return services;
    }

    private static IServiceCollection AddSeoRankDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SeoRankContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlExpressConnection")));

        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddScoped<IWatchListRepository, WatchListRepository>();

        return services;
    }
}
