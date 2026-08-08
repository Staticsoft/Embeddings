using Amazon;
using Amazon.BedrockRuntime;
using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Embeddings.Bedrock;

namespace Staticsoft.Embeddings.Tests;

public class BedrockTextEmbeddingsTests : TextEmbeddingsTests
{
    protected override IServiceCollection Services => base.Services
        .UseBedrockTextEmbeddings(
            _ => new AmazonBedrockRuntimeClient(GetAccessKeyId(), GetSecretAccessKey(), GetRegion()),
            _ => new BedrockTextEmbeddingsOptions()
        );

    static string GetAccessKeyId()
        => EnvVariable("EmbeddingsAccessKeyId");

    static string GetSecretAccessKey()
        => EnvVariable("EmbeddingsSecretAccessKey");

    static RegionEndpoint GetRegion()
        => RegionEndpoint.GetBySystemName(EnvVariable("EmbeddingsRegion"));

    static string EnvVariable(string name)
        => Environment.GetEnvironmentVariable(name)
        ?? throw new ArgumentNullException($"Environment variable {name} is not set");
}
