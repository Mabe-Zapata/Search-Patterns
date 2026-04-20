using SearchPatterns.Domain.FarmerPuzzle.Enums;

namespace SearchPatterns.Domain.FarmerPuzzle.Entities;

public sealed class FarmerState : IEquatable<FarmerState>
{
	public FarmerState? Parent { get; init; }
	public int Depth { get; init; }
	public string ActionTaken { get; init; } = string.Empty;
	public Side Farmer { get; init; }
	public Side Wolf { get; init; }
	public Side Goat { get; init; }
	public Side Cabbage { get; init; }

	public bool IsGoal =>
		Farmer == Side.Right &&
		Wolf == Side.Right &&
		Goat == Side.Right &&
		Cabbage == Side.Right;

	public bool IsUnsafe =>
		(Wolf == Goat && Wolf != Farmer) ||
		(Goat == Cabbage && Goat != Farmer);

	public bool Equals(FarmerState? other) =>
		other is not null &&
		Farmer == other.Farmer &&
		Wolf == other.Wolf &&
		Goat == other.Goat &&
		Cabbage == other.Cabbage;

	public override bool Equals(object? obj) => Equals(obj as FarmerState);

	public override int GetHashCode() =>
		HashCode.Combine(Farmer, Wolf, Goat, Cabbage);

	public override string ToString() =>
		$"Farmer={Farmer} Wolf={Wolf} Goat={Goat} Cabbage={Cabbage}";
}