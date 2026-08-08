using Staticsoft.Embeddings.Abstractions;
using Staticsoft.Testing;
using Xunit;

namespace Staticsoft.Embeddings.Tests;

public abstract class TextEmbeddingsTests : TestBase<TextEmbeddings>
{
    const string Text = "Thank you for your order! The mug ships tomorrow morning.";
    const string SimilarText = "Thanks for ordering! Your mug will ship tomorrow.";
    const string UnrelatedText = "Grandmaster sacrifices the queen and wins the endgame.";

    [Fact]
    public async Task ReturnsVectorWithDeclaredDimensions()
    {
        var vector = await SUT.Embed(Text);
        Assert.Equal(SUT.Dimensions, vector.Length);
    }

    [Fact]
    public async Task ReturnsUnitLengthVector()
    {
        var vector = await SUT.Embed(Text);
        Assert.Equal(1, Length(vector), precision: 2);
    }

    [Fact]
    public async Task ReturnsSameEmbeddingForSameText()
    {
        var first = await SUT.Embed(Text);
        var second = await SUT.Embed(Text);
        Assert.True(Cosine(first, second) > 0.999, $"Expected near-identical embeddings, cosine was {Cosine(first, second)}");
    }

    [Fact]
    public async Task EmbedsSimilarTextsCloserThanUnrelatedTexts()
    {
        var text = await SUT.Embed(Text);
        var similar = await SUT.Embed(SimilarText);
        var unrelated = await SUT.Embed(UnrelatedText);

        Assert.True(
            Cosine(text, similar) > Cosine(text, unrelated),
            $"Expected similarity({nameof(SimilarText)}) > similarity({nameof(UnrelatedText)}), got {Cosine(text, similar)} <= {Cosine(text, unrelated)}"
        );
    }

    [Fact]
    public async Task ThrowsOnEmptyText()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => SUT.Embed(string.Empty));
        await Assert.ThrowsAsync<ArgumentException>(() => SUT.Embed("   "));
    }

    static double Length(float[] vector)
        => Math.Sqrt(vector.Sum(value => (double)value * value));

    static double Cosine(float[] first, float[] second)
        => first.Zip(second, (a, b) => (double)a * b).Sum() / (Length(first) * Length(second));
}
