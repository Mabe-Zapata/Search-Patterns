using SearchPatterns.Domain.MissionariesAndCannibals.Entities;
using SearchPatterns.Domain.MissionariesAndCannibals.Enums;
using SearchPatterns.Domain.MissionariesAndCannibals.Interfaces;

namespace SearchPatterns.Application.MissionariesAndCannibals.Services;

/// <summary>
/// BFS solver for the Missionaries and Cannibals problem.
/// Explores all states at depth n before moving to n+1,
/// guaranteeing the shortest (optimal) solution.
/// </summary>
public sealed class MissionariesBfsSolverService : IMissionariesBfsSolver
{
	/// <summary>
	/// The five possible moves: (missionaries, cannibals) that can travel in the boat.
	/// The boat carries at least 1 and at most 2 people.
	/// </summary>
	private static readonly (int Missionaries, int Cannibals, OperationType Type, string Label)[] Moves =
	[
		(1, 0, OperationType.Move1Missionary, "1 misionero cruza"),
		(0, 1, OperationType.Move1Cannibal, "1 caníbal cruza"),
		(2, 0, OperationType.Move2Missionaries, "2 misioneros cruzan"),
		(0, 2, OperationType.Move2Cannibals, "2 caníbales cruzan"),
		(1, 1, OperationType.Move1Missionary1Cannibal, "1 misionero y 1 caníbal cruzan"),
	];

	public SolutionPath Solve()
	{
		var root = new MissionariesState
		{
			MissionariesLeft = 3,
			CannibalsLeft = 3,
			MissionariesRight = 0,
			CannibalsRight = 0,
			BoatPosition = BankLocation.Left,
			Parent = null,
			Depth = 0,
			ActionTaken = "Estado inicial"
		};

		var queue = new Queue<MissionariesState>();
		var visited = new HashSet<MissionariesState>();

		queue.Enqueue(root);
		visited.Add(root);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();

			if (current.IsGoal)
				return SolutionPath.Solved(BuildPath(current));

			foreach (var successor in GetSuccessors(current))
			{
				if (!visited.Contains(successor))
				{
					visited.Add(successor);
					queue.Enqueue(successor);
				}
			}
		}

		return SolutionPath.Unsolvable();
	}

	private static IEnumerable<MissionariesState> GetSuccessors(MissionariesState state)
	{
		var destination = Opposite(state.BoatPosition);

		foreach (var (m, c, opType, label) in Moves)
		{
			MissionariesState? candidate;

			if (state.BoatPosition == BankLocation.Left)
			{
				// Moving from left to right
				if (state.MissionariesLeft < m || state.CannibalsLeft < c)
					continue;

				candidate = new MissionariesState
				{
					Parent = state,
					Depth = state.Depth + 1,
					ActionTaken = $"→ {label}",
					MissionariesLeft = state.MissionariesLeft - m,
					CannibalsLeft = state.CannibalsLeft - c,
					MissionariesRight = state.MissionariesRight + m,
					CannibalsRight = state.CannibalsRight + c,
					BoatPosition = destination
				};
			}
			else
			{
				// Moving from right to left
				if (state.MissionariesRight < m || state.CannibalsRight < c)
					continue;

				candidate = new MissionariesState
				{
					Parent = state,
					Depth = state.Depth + 1,
					ActionTaken = $"← {label}",
					MissionariesLeft = state.MissionariesLeft + m,
					CannibalsLeft = state.CannibalsLeft + c,
					MissionariesRight = state.MissionariesRight - m,
					CannibalsRight = state.CannibalsRight - c,
					BoatPosition = destination
				};
			}

			if (candidate.IsValid)
				yield return candidate;
		}
	}

	private static List<OperationStep> BuildPath(MissionariesState goal)
	{
		var reversed = new List<MissionariesState>();
		for (var node = goal; node is not null; node = node.Parent)
			reversed.Add(node);

		reversed.Reverse();

		return reversed
			.Select((state, index) => new OperationStep(
				StepNumber: index,
				Operation: index == 0 ? OperationType.Move1Missionary : GuessOperation(reversed, index),
				Action: state.ActionTaken,
				Description: BuildDescription(state),
				MissionariesLeft: state.MissionariesLeft,
				CannibalsLeft: state.CannibalsLeft,
				MissionariesRight: state.MissionariesRight,
				CannibalsRight: state.CannibalsRight,
				BoatPosition: state.BoatPosition
			))
			.ToList();
	}

	/// <summary>
	/// Determines which operation was applied by comparing with the previous state.
	/// </summary>
	private static OperationType GuessOperation(List<MissionariesState> path, int index)
	{
		if (index == 0)
			return OperationType.Move1Missionary; // placeholder for initial state

		var prev = path[index - 1];
		var curr = path[index];

		int mDelta = Math.Abs(curr.MissionariesLeft - prev.MissionariesLeft);
		int cDelta = Math.Abs(curr.CannibalsLeft - prev.CannibalsLeft);

		return (mDelta, cDelta) switch
		{
			(1, 0) => OperationType.Move1Missionary,
			(0, 1) => OperationType.Move1Cannibal,
			(2, 0) => OperationType.Move2Missionaries,
			(0, 2) => OperationType.Move2Cannibals,
			(1, 1) => OperationType.Move1Missionary1Cannibal,
			_ => OperationType.Move1Missionary
		};
	}

	private static string BuildDescription(MissionariesState state)
	{
		return $"Izquierda: [M={state.MissionariesLeft}, C={state.CannibalsLeft}]  →  Derecha: [M={state.MissionariesRight}, C={state.CannibalsRight}]  Bote: {(state.BoatPosition == BankLocation.Left ? "Izquierda" : "Derecha")}";
	}

	private static BankLocation Opposite(BankLocation location) =>
		location == BankLocation.Left ? BankLocation.Right : BankLocation.Left;
}
