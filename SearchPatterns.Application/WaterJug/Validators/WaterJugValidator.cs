namespace SearchPatterns.Application.WaterJug.Validators;

/// <summary>
/// Result of validating water jug problem parameters.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the validation passed.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets the list of validation errors, if any.
    /// </summary>
    public IReadOnlyList<string> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResult"/> class.
    /// </summary>
    /// <param name="isValid">Whether validation passed.</param>
    /// <param name="errors">List of validation errors.</param>
    public ValidationResult(bool isValid, IReadOnlyList<string> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    /// <returns>A valid <see cref="ValidationResult"/>.</returns>
    public static ValidationResult Success() => new(true, Array.Empty<string>());

    /// <summary>
    /// Creates a failed validation result with the specified errors.
    /// </summary>
    /// <param name="errors">List of validation errors.</param>
    /// <returns>An invalid <see cref="ValidationResult"/> with errors.</returns>
    public static ValidationResult Failure(IEnumerable<string> errors)
    {
        var errorList = errors.ToList();
        return new ValidationResult(false, errorList);
    }
}

/// <summary>
/// Interface for validating water jug problem parameters.
/// </summary>
public interface IWaterJugValidator
{
    /// <summary>
    /// Validates the parameters for a water jug problem.
    /// </summary>
    /// <param name="jugACapacity">The capacity of jug A.</param>
    /// <param name="jugBCapacity">The capacity of jug B.</param>
    /// <param name="targetAmount">The target amount to measure.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the parameters are valid.</returns>
    ValidationResult Validate(int jugACapacity, int jugBCapacity, int targetAmount);
}

/// <summary>
/// Validates input parameters for the water jug problem.
/// </summary>
public class WaterJugValidator : IWaterJugValidator
{
    /// <inheritdoc/>
    public ValidationResult Validate(int jugACapacity, int jugBCapacity, int targetAmount)
    {
        var errors = new List<string>();

        // Validate jug A capacity is positive
        if (jugACapacity <= 0)
        {
            errors.Add("Jug A capacity must be a positive integer");
        }

        // Validate jug B capacity is positive
        if (jugBCapacity <= 0)
        {
            errors.Add("Jug B capacity must be a positive integer");
        }

        // If capacities are invalid, return early with those errors
        if (errors.Count > 0)
        {
            return ValidationResult.Failure(errors);
        }

        // Validate target is greater than zero
        if (targetAmount <= 0)
        {
            errors.Add("Target amount must be greater than zero");
        }

        // Validate target does not exceed combined capacity
        int combinedCapacity = jugACapacity + jugBCapacity;
        if (targetAmount > combinedCapacity)
        {
            errors.Add("Target amount exceeds combined jug capacity");
        }

        // Validate target is achievable (not greater than the largest jug)
        int maxCapacity = Math.Max(jugACapacity, jugBCapacity);
        if (targetAmount > maxCapacity)
        {
            errors.Add("Target amount is greater than the largest jug capacity");
        }

        return errors.Count > 0
            ? ValidationResult.Failure(errors)
            : ValidationResult.Success();
    }
}