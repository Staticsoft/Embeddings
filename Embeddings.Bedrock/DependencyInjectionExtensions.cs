using Amazon.BedrockRuntime;
using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Embeddings.Abstractions;

namespace Staticsoft.Embeddings.Bedrock;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseBedrockTextEmbeddings(
        this IServiceCollection services,
        Func<IServiceProvider, AmazonBedrockRuntimeClient> client,
        Func<IServiceProvider, BedrockTextEmbeddingsOptions> options
    ) => services
        .AddSingleton<TextEmbeddings, BedrockTextEmbeddings>()
        .AddSingleton(client)
        .AddSingleton(options);
}
