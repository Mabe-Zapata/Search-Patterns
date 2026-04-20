namespace SearchPatterns.Domain.MapColoring.Entities;

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
public record ValidationResult(bool IsValid, List<string> Errors)
{
    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationResult Success() => new(true, new List<string>());
    
    /// <summary>
    /// Creates a failed validation result with error messages.
    /// </summary>
    public static ValidationResult Failure(List<string> errors) => new(false, errors);
}
