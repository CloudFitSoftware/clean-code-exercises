namespace ChapterNine.Application.Interfaces;

public interface ICacheService
{
    Task<T?> GetValueAsync<T>(string cacheKey, CancellationToken cancellationToken);
    Task SetValueAsync<T>(string cacheKey, T value, CancellationToken cancellationToken);
}