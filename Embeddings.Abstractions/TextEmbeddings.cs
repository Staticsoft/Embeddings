namespace Staticsoft.Embeddings.Abstractions;

public interface TextEmbeddings
{
    int Dimensions { get; }

    /// <summary>
    /// Converts text into a unit-length embedding vector with
    /// <see cref="Dimensions"/> elements. Deterministic for the same text.
    /// Throws <see cref="ArgumentException"/> when the text is empty or whitespace.
    /// </summary>
    Task<float[]> Embed(string text);
}
