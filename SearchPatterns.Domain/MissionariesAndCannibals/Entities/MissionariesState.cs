using SearchPatterns.Domain.MissionariesAndCannibals.Enums;

namespace SearchPatterns.Domain.MissionariesAndCannibals.Entities;

/// <summary>
/// Represents the current state of the Missionaries and Cannibals puzzle.
/// Tracks the count of missionaries and cannibals on each bank, plus the boat location.
/// </summary>
public sealed class MissionariesState : IEquatable<MissionariesState>
{
	public MissionariesState? Parent { get; init; }
	public int Depth { get; init; }
	public string ActionTaken { get; init; } = string.Empty;

	/// <summary>Number of missionaries on the left bank.</summary>
	public int MissionariesLeft { get; init; }

	/// <summary>Number of cannibals on the left bank.</summary>
	public int CannibalsLeft { get; init; }

	/// <summary>Number of missionaries on the right bank.</summary>
	public int MissionariesRight { get; init; }

	/// <summary>Number of cannibals on the right bank.</summary>
	public int CannibalsRight { get; init; }

	/// <summary>Current position of the boat.</summary>
	public BankLocation BoatPosition { get; init; }

	/// <summary>
	/// The goal state: all 3 missionaries and 3 cannibals are on the right bank.
	/// </summary>
	public bool IsGoal =>
		MissionariesLeft == 0 &&
		CannibalsLeft == 0 &&
		MissionariesRight == 3 &&
		CannibalsRight == 3;

	/// <summary>
	/// A state is valid if, on every bank that has missionaries,
	/// the number of missionaries is greater than or equal to the number of cannibals.
	/// A bank with 0 missionaries is always safe (cannibals can't eat anyone).
	/// </summary>
	public bool IsValid =>
		MissionariesLeft >= 0 && CannibalsLeft >= 0 &&
		MissionariesRight >= 0 && CannibalsRight >= 0 &&
		(MissionariesLeft == 0 || MissionariesLeft >= CannibalsLeft) &&
		(MissionariesRight == 0 || MissionariesRight >= CannibalsRight);

	public bool Equals(MissionariesState? other) =>
		other is not null &&
		MissionariesLeft == other.MissionariesLeft &&
		CannibalsLeft == other.CannibalsLeft &&
		MissionariesRight == other.MissionariesRight &&
		CannibalsRight == other.CannibalsRight &&
		BoatPosition == other.BoatPosition;

	public override bool Equals(object? obj) => Equals(obj as MissionariesState);

	public override int GetHashCode() =>
		HashCode.Combine(MissionariesLeft, CannibalsLeft, MissionariesRight, CannibalsRight, BoatPosition);

	public override string ToString() =>
		$"L[M={MissionariesLeft},C={CannibalsLeft}] Boat={BoatPosition} R[M={MissionariesRight},C={CannibalsRight}]";
}
