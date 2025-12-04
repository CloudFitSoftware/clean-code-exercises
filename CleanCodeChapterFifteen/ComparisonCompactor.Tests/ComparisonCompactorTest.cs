#nullable disable
using Xunit;

namespace ComparisonCompactor.Tests;

public class ComparisonCompactorTest
{
    [Fact]
    public void TestMessage()
    {
        string failure = new ComparisonCompactor(0, "b", "c").Compact("a");
        Xunit.Assert.Equal("a expected:<[b]> but was:<[c]>", failure);
    }

    [Fact]
    public void TestStartSame()
    {
        string failure = new ComparisonCompactor(1, "ba", "bc").Compact(null);
        Xunit.Assert.Equal("expected:<b[a]> but was:<b[c]>", failure);
    }

    [Fact]
    public void TestEndSame()
    {
        string failure = new ComparisonCompactor(1, "ab", "cb").Compact(null);
        Xunit.Assert.Equal("expected:<[a]b> but was:<[c]b>", failure);
    }

    [Fact]
    public void TestSame()
    {
        string failure = new ComparisonCompactor(1, "ab", "ab").Compact(null);
        Xunit.Assert.Equal("expected:<ab> but was:<ab>", failure);
    }

    [Fact]
    public void TestNoContextStartAndEndSame()
    {
        string failure = new ComparisonCompactor(0, "abc", "adc").Compact(null);
        Xunit.Assert.Equal("expected:<...[b]...> but was:<...[d]...>", failure);
    }

    [Fact]
    public void TestStartAndEndContext()
    {
        string failure = new ComparisonCompactor(1, "abc", "adc").Compact(null);
        Xunit.Assert.Equal("expected:<a[b]c> but was:<a[d]c>", failure);
    }

    [Fact]
    public void TestStartAndEndContextWithEllipses()
    {
        string failure =
            new ComparisonCompactor(1, "abcde", "abfde").Compact(null);
        Xunit.Assert.Equal("expected:<...b[c]d...> but was:<...b[f]d...>", failure);
    }

    [Fact]
    public void TestComparisonErrorStartSameComplete()
    {
        string failure = new ComparisonCompactor(2, "ab", "abc").Compact(null);
        Xunit.Assert.Equal("expected:<ab[]> but was:<ab[c]>", failure);
    }

    [Fact]
    public void TestComparisonErrorEndSameComplete()
    {
        string failure = new ComparisonCompactor(0, "bc", "abc").Compact(null);
        Xunit.Assert.Equal("expected:<[]...> but was:<[a]...>", failure);
    }

    [Fact]
    public void TestComparisonErrorEndSameCompleteContext()
    {
        string failure = new ComparisonCompactor(2, "bc", "abc").Compact(null);
        Xunit.Assert.Equal("expected:<[]bc> but was:<[a]bc>", failure);
    }

    [Fact]
    public void TestComparisonErrorOverlapingMatches()
    {
        string failure = new ComparisonCompactor(0, "abc", "abbc").Compact(null);
        Xunit.Assert.Equal("expected:<...[]...> but was:<...[b]...>", failure);
    }

    [Fact]
    public void TestComparisonErrorOverlapingMatchesContext()
    {
        string failure = new ComparisonCompactor(2, "abc", "abbc").Compact(null);
        Xunit.Assert.Equal("expected:<ab[]c> but was:<ab[b]c>", failure);
    }

    [Fact]
    public void TestComparisonErrorOverlapingMatches2()
    {
        string failure = new ComparisonCompactor(0, "abcdde",
            "abcde").Compact(null);
        Xunit.Assert.Equal("expected:<...[d]...> but was:<...[]...>", failure);
    }

    [Fact]
    public void TestComparisonErrorOverlapingMatches2Context()
    {
        string failure =
            new ComparisonCompactor(2, "abcdde", "abcde").Compact(null);
        Xunit.Assert.Equal("expected:<...cd[d]e> but was:<...cd[]e>", failure);
    }

    [Fact]
    public void TestComparisonErrorWithActualNull()
    {
        string failure = new ComparisonCompactor(0, "a", null).Compact(null);
        Xunit.Assert.Equal("expected:<a> but was:<null>", failure);
    }

    [Fact]
    public void TestComparisonErrorWithActualNullContext()
    {
        string failure = new ComparisonCompactor(2, "a", null).Compact(null);
        Xunit.Assert.Equal("expected:<a> but was:<null>", failure);
    }

    [Fact]
    public void TestComparisonErrorWithExpectedNull()
    {
        string failure = new ComparisonCompactor(0, null, "a").Compact(null);
        Xunit.Assert.Equal("expected:<null> but was:<a>", failure);
    }

    [Fact]
    public void TestComparisonErrorWithExpectedNullContext()
    {
        string failure = new ComparisonCompactor(2, null, "a").Compact(null);
        Xunit.Assert.Equal("expected:<null> but was:<a>", failure);
    }

    [Fact]
    public void TestBug609972()
    {
        string failure = new ComparisonCompactor(10, "S&P500", "0").Compact(null);
        Xunit.Assert.Equal("expected:<[S&P50]0> but was:<[]0>", failure);
    }
}
