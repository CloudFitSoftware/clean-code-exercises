using ChapterNine.Application.Models;

namespace ChapterNine.Application.Interfaces;

public interface IUserValidator
{
    bool IsValid(User user);
}