using ChapterNine.Application.Interfaces;
using ChapterNine.Application.Models;

namespace ChapterNine.Application.Services;

public class UserService(IUserRepository userRepository, ICacheService cacheService, IUserValidator userValidator) : IUserService
{
    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}