namespace SearchPatterns.Domain.WaterJug.Entities;

/// <summary>
/// Represents the current amount of water in each jug, represented as a tuple (jug_a_amount, jug_b_amount).
/// </summary>
public record WaterState(int JugACurrent, int JugBCurrent)
{
    public WaterState() : this(0, 0) { }

    public bool IsGoalState(int targetAmount)
    {
        return JugACurrent == targetAmount || JugBCurrent == targetAmount;
    }

    public override string ToString() => $"({JugACurrent}, {JugBCurrent})";
}