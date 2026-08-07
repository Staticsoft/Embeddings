using Staticsoft.Embeddings.Abstractions;

namespace Staticsoft.Embeddings.Memory;

/// <summary>
/// Deterministic in-process embeddings: character n-grams hashed into the
/// vector with signed weights, normalized to unit length. Texts sharing
/// content produce closer vectors than unrelated texts, so similarity
/// ordering is meaningful, unlike with random vectors.
/// </summary>
public class MemoryTextEmbeddings(
    MemoryTextEmbeddingsOptions options
) : TextEmbeddings
{
    readonly MemoryTextEmbeddingsOptions Options = options;

    const int GramSize = 3;

    public int Dimensions
        => Options.Dimensions;

    public Task<float[]> Embed(string text)
        => Task.FromResult(EmbedText(text));

    float[] EmbedText(string text)
        => UnitLength(Accumulate(Grams(Normalized(text))));

    static string Normalized(string text)
        => string.IsNullOrWhiteSpace(text)
        ? throw new ArgumentException("Text cannot be empty", nameof(text))
        : text.ToLowerInvariant();

    static IEnumerable<string> Grams(string text)
        => text.Length <= GramSize
        ? [text]
        : Enumerable.Range(0, text.Length - GramSize + 1).Select(start => text.Substring(start, GramSize));

    float[] Accumulate(IEnumerable<string> grams)
    {
        var vector = new float[Dimensions];
        foreach (var gram in grams)
        {
            var hash = Hash(gram);
            vector[(int)(hash % (uint)Dimensions)] += Sign(hash);
        }
        return vector;
    }

    static float Sign(uint hash)
        => (hash & 0x80000000) == 0 ? 1 : -1;

    static uint Hash(string gram)
        => gram.Aggregate(2166136261, (hash, character) => (hash ^ character) * 16777619);

    static float[] UnitLength(float[] vector)
    {
        var length = MathF.Sqrt(vector.Sum(value => value * value));
        return vector.Select(value => value / length).ToArray();
    }
}
