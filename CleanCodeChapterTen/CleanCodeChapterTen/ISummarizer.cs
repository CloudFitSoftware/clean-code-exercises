public interface ISummarizer
{
    void SummarizeByKey(string key, List<Dictionary<string, string>> records);
}