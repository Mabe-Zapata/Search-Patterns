namespace SearchPatterns.Domain.WaterJug.Entities;

/// <summary>
/// Represents a jug with a specific maximum capacity for holding water.
/// </summary>
public class Jug
{
    public int Capacity { get; }
    public int CurrentAmount { get; set; }

    public Jug(int capacity, int currentAmount = 0)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be a positive integer.", nameof(capacity));
        
        Capacity = capacity;
        CurrentAmount = Math.Clamp(currentAmount, 0, capacity);
    }

    public void Fill()
    {
        CurrentAmount = Capacity;
    }

    public void Empty()
    {
        CurrentAmount = 0;
    }

    public void TransferTo(Jug destination)
    {
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));

        int availableSpace = destination.Capacity - destination.CurrentAmount;
        int transferAmount = Math.Min(CurrentAmount, availableSpace);

        CurrentAmount -= transferAmount;
        destination.CurrentAmount += transferAmount;
    }
}