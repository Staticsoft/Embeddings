using Microsoft.Extensions.DependencyInjection;
using Staticsoft.Embeddings.Memory;

namespace Staticsoft.Embeddings.Tests;

public class MemoryTextEmbeddingsTests : TextEmbeddingsTests
{
    protected override IServiceCollection Services => base.Services
        .UseMemoryTextEmbeddings();
}
