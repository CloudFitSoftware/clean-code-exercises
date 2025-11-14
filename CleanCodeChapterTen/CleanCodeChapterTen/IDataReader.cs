public interface IDataReader
{
    IEnumerable<IDictionary<string, string>> ReadData(string filePath);
}