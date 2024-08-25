using CeoRankTask.Core.Dtos;
using CeoRankTask.Core.Interfaces;
using CeoRankTask.Core.Services;
using FluentValidation;
using Microsoft.Net.Http.Headers;
using Polly;
using SeoRankTask.Infrastructure;
using SeoRankTask.Infrastructure.Repositories;

namespace CeoRankTask.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CeoRankRequestDtoValidator>();
        services.AddSingleton<IExtractor, GoogleExtractor>();
        services.AddSingleton<IExtractor, BingExtractor>();
        services.AddScoped<ISeoRankService, SeoRankService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddGoogleClient(configuration)
            .AddBingClient(configuration);

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
}
