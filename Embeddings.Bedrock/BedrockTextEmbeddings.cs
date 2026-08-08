using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Staticsoft.Embeddings.Abstractions;
using System.Text;
using System.Text.Json;

namespace Staticsoft.Embeddings.Bedrock;

/// <summary>
/// Embeddings produced by Amazon Titan Text Embeddings V2 through Bedrock.
/// Vectors are requested normalized, satisfying the unit-length contract.
/// </summary>
public class BedrockTextEmbeddings(
    AmazonBedrockRuntimeClient client,
    BedrockTextEmbeddingsOptions options
) : TextEmbeddings
{
    readonly AmazonBedrockRuntimeClient Client = client;
    readonly BedrockTextEmbeddingsOptions Options = options;

    public int Dimensions
        => Options.Dimensions;

    public async Task<float[]> Embed(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Text cannot be empty", nameof(text));

        var response = await Client.InvokeModelAsync(CreateRequest(text));
        return await ParseEmbedding(response);
    }

    InvokeModelRequest CreateRequest(string text)
        => new()
        {
            ModelId = Options.ModelId,
            ContentType = "application/json",
            Accept = "application/json",
            Body = new MemoryStream(Encoding.UTF8.GetBytes(SerializeRequestBody(text)))
        };

    string SerializeRequestBody(string text)
        => JsonSerializer.Serialize(new
        {
            inputText = text,
            dimensions = Options.Dimensions,
            normalize = true
        });

    static async Task<float[]> ParseEmbedding(InvokeModelResponse response)
    {
        using var body = await JsonDocument.ParseAsync(response.Body);
        return body.RootElement
            .GetProperty("embedding")
            .EnumerateArray()
            .Select(value => value.GetSingle())
            .ToArray();
    }
}
