namespace ComparisonCompactor
{
    public class ComparisonCompactor
    {
        private const string ELLIPSIS = "...";
        private const string DELTA_END = "]";
        private const string DELTA_START = "[";

        private int fContextLength;
        private string fExpected;
        private string fActual;
        private int fPrefix;
        private int fSuffix;

        public ComparisonCompactor(int contextLength,
                                   string expected,
                                   string actual)
        {
            fContextLength = contextLength;
            fExpected = expected;
            fActual = actual;
        }

        public string Compact(string message)
        {
            if (fExpected == null || fActual == null || AreStringsEqual())
                // Was Assert.format
                return Assert.Format(message, fExpected, fActual);

            FindCommonPrefix();
            FindCommonSuffix();
            string expected = CompactString(fExpected);
            string actual = CompactString(fActual);
            // Was Assert.format
            return Assert.Format(message, expected, actual);
        }

        private string CompactString(string source)
        {
            string result = DELTA_START +
                source.Substring(fPrefix, source.Length -
                    fSuffix + 1 - fPrefix) + DELTA_END;
            if (fPrefix > 0)
                result = ComputeCommonPrefix() + result;
            if (fSuffix > 0)
                result = result + ComputeCommonSuffix();
            return result;
        }

        private void FindCommonPrefix()
        {
            fPrefix = 0;
            int end = Math.Min(fExpected.Length, fActual.Length);
            for (; fPrefix < end; fPrefix++)
            {
                if (fExpected[fPrefix] != fActual[fPrefix])
                    break;
            }
        }

        private void FindCommonSuffix()
        {
            int expectedSuffix = fExpected.Length - 1;
            int actualSuffix = fActual.Length - 1;
            for (;
                 actualSuffix >= fPrefix && expectedSuffix >= fPrefix;
                 actualSuffix--, expectedSuffix--)
            {
                if (fExpected[expectedSuffix] != fActual[actualSuffix])
                    break;
            }
            fSuffix = fExpected.Length - expectedSuffix;
        }

        private string ComputeCommonPrefix()
        {
            return (fPrefix > fContextLength ? ELLIPSIS : "") +
                fExpected.Substring(Math.Max(0, fPrefix - fContextLength),
                    fPrefix - Math.Max(0, fPrefix - fContextLength));
        }

        private string ComputeCommonSuffix()
        {
            int end = Math.Min(fExpected.Length - fSuffix + 1 + fContextLength,
                fExpected.Length);
            return fExpected.Substring(fExpected.Length - fSuffix + 1, end - (fExpected.Length - fSuffix + 1)) +
                (fExpected.Length - fSuffix + 1 < fExpected.Length -
                    fContextLength ? ELLIPSIS : "");
        }

        private bool AreStringsEqual()
        {
            return fExpected.Equals(fActual);
        }
    }
}
