namespace Staticsoft.Embeddings.Bedrock;

public class BedrockTextEmbeddingsOptions
{
    public string ModelId { get; init; } = "amazon.titan-embed-text-v2:0";

    /// <summary>Titan Text Embeddings V2 accepts 1024, 512 or 256.</summary>
    public int Dimensions { get; init; } = 1024;
}
