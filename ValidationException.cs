namespace TestDataUSBasicLibrary;

/// <summary>
/// Represents a validation exception.
/// </summary>
public class ValidationException(string message) : Exception(message)
{
}