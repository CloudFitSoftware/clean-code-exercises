using ChapterNine.Application.Models;

namespace ChapterNine.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
    Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken);
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
}