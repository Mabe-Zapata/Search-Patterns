namespace SearchPatterns.Domain.MissionariesAndCannibals.Enums;

/// <summary>
/// Represents the possible moves in the Missionaries and Cannibals problem.
/// Each operation moves one or two people across the river in the boat.
/// </summary>
public enum OperationType
{
	/// <summary>Move 1 missionary across the river.</summary>
	Move1Missionary,

	/// <summary>Move 1 cannibal across the river.</summary>
	Move1Cannibal,

	/// <summary>Move 2 missionaries across the river.</summary>
	Move2Missionaries,

	/// <summary>Move 2 cannibals across the river.</summary>
	Move2Cannibals,

	/// <summary>Move 1 missionary and 1 cannibal across the river.</summary>
	Move1Missionary1Cannibal
}
