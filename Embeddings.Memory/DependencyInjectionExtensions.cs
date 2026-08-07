using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Embeddings.Abstractions;

namespace Staticsoft.Embeddings.Memory;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseMemoryTextEmbeddings(this IServiceCollection services)
        => services.UseMemoryTextEmbeddings(_ => new());

    public static IServiceCollection UseMemoryTextEmbeddings(
        this IServiceCollection services,
        Func<IServiceProvider, MemoryTextEmbeddingsOptions> options
    ) => services
        .AddSingleton<TextEmbeddings, MemoryTextEmbeddings>()
        .AddSingleton(options);
}
