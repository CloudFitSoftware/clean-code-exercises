namespace ChapterNine.Application.Exceptions;

[System.Serializable]
public class InvalidUserException : Exception
{
    public InvalidUserException() { }
    public InvalidUserException(string message) : base(message) { }
    public InvalidUserException(string message, System.Exception inner) : base(message, inner) { }
}