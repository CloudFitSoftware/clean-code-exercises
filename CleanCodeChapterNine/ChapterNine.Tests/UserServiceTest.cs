using ChapterNine.Application.Exceptions;
using ChapterNine.Application.Interfaces;
using ChapterNine.Application.Models;
using ChapterNine.Application.Services;

namespace ChapterNine.Tests;

public class UserServiceTest
{

    private readonly Mock<ICacheService> cacheServiceMock = new();
    private readonly Mock<IUserRepository> repositoryMock = new();
    private readonly Mock<IUserValidator> userValidatorMock = new();
    private readonly IUserService userService;

    public UserServiceTest()
    {
        cacheServiceMock.Reset();
        repositoryMock.Reset();
        userValidatorMock.Reset();
        userService = new UserService(repositoryMock.Object, cacheServiceMock.Object, userValidatorMock.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_Cached()
    {

        // Setup fake users for cache return
        var cachedUsers = new List<User>
        {
            CreateFakeUser(Guid.NewGuid(), "User", "One", "userone@test.com"),
            CreateFakeUser(Guid.NewGuid(), "Test", "Two", "testtwo@test.com")
        };

        // The cache should return the above list of users when called with cache key "users"
        cacheServiceMock.Setup(mock => mock.GetValueAsync<IEnumerable<User>?>("users", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedUsers);

        var results = await userService.GetUsersAsync(default);
        
        // Assert that 2 results are returned and that the repository was nehver invoked
        Assert.Equal(2, results.Count());
        repositoryMock.Verify(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAllUserAsync_NotCached()
    {
        // Setup fake users for cache return
        var repositoryUsers = new List<User>
        {
            CreateFakeUser(Guid.NewGuid(), "User", "One", "userone@test.com"),
            CreateFakeUser(Guid.NewGuid(), "Test", "Two", "testtwo@test.com")
        };

        // The cache should return the above list of users when called with cache key "users"
        cacheServiceMock.Setup(mock => mock.GetValueAsync<IEnumerable<User>>("users", It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        repositoryMock.Setup(mock => mock.GetAllAsync(default)).ReturnsAsync(repositoryUsers);

        var results = await userService.GetUsersAsync(default);

        // Assert that 2 results are returned and that the repository and cache were invoked once 
        Assert.Equal(2, results.Count());
        repositoryMock.Verify(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        cacheServiceMock.Verify(mock => mock.SetValueAsync("users", It.IsAny<IEnumerable<User>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_FailsValidation()
    {
        userValidatorMock.Setup(mock => mock.IsValid(It.IsAny<User>())).Returns(false);

        // Assert that a InvalidUserException is thrown and the database is never invoked
        await Assert.ThrowsAsync<InvalidUserException>(() => userService.CreateUserAsync(CreateFakeUser(Guid.NewGuid(), "First", "Last"), default));
        repositoryMock.Verify(mock => mock.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateUserAsync_Success()
    {
        var userId = Guid.NewGuid();
        var newUser = CreateFakeUser(userId, "First", "Last", "test@test.com");
        var returnedUser = CreateFakeUser(userId, "First", "Last", "test@test.com");

        userValidatorMock.Setup(mock => mock.IsValid(newUser)).Returns(true);
        repositoryMock.Setup(mock => mock.CreateUserAsync(newUser, It.IsAny<CancellationToken>())).ReturnsAsync(returnedUser);

        // Assert that the method ran successfully, returning the output of the save operation
        var result = await userService.CreateUserAsync(newUser, default);
        Assert.Equal(returnedUser.Id, result.Id);
    }

    [Fact]
    public async Task UpdateUserAsync_FailsValidation()
    {
        userValidatorMock.Setup(mock => mock.IsValid(It.IsAny<User>())).Returns(false);

        // Assert that a InvalidUserException is thrown and the database is never invoked
        await Assert.ThrowsAsync<InvalidUserException>(() => userService.UpdateUserAsync(CreateFakeUser(Guid.NewGuid(), "First", "Last"), default));
        repositoryMock.Verify(mock => mock.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_Success()
    {
        var userId = Guid.NewGuid();
        var user = CreateFakeUser(userId, "First", "Last", "test@test.com");
        var returnedUser = CreateFakeUser(userId, "First", "Last", "test@test.com");

        userValidatorMock.Setup(mock => mock.IsValid(user)).Returns(true);
        repositoryMock.Setup(mock => mock.UpdateUserAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(returnedUser);

        // Assert that the method ran successfully, returning the output of the save operation
        var result = await userService.UpdateUserAsync(user, default);
        Assert.Equal(returnedUser.Id, result.Id);
    }

    [Fact]
    public async Task DeleteUserAsync_Success()
    {
        var userId = Guid.NewGuid();
        await userService.DeleteUserAsync(userId, default);

        // We only need to verify that the delete method was called
        repositoryMock.Verify(mock => mock.DeleteUserAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    private User CreateFakeUser(Guid id, string firstName, string lastName, string? email = null)
    {
        return new User
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = email
        };
    }

}