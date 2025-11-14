public interface ISummarizer
{
    IEnumerable<Summary> SummarizeByKey(string key, List<Dictionary<string, string>> records);
}