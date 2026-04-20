using SearchPatterns.Domain.FarmerPuzzle.Entities;
using SearchPatterns.Domain.FarmerPuzzle.Enums;
using SearchPatterns.Domain.FarmerPuzzle.Interfaces;

namespace SearchPatterns.Application.FarmerPuzzle.Services;
public sealed class FarmerBfsSolverService : IFarmerBfsSolver
{
	public SolutionResult Solve()
	{
		var root = new FarmerState
		{
			Farmer = Side.Left,
			Wolf = Side.Left,
			Goat = Side.Left,
			Cabbage = Side.Left,
			Parent = null,
			Depth = 0,
			ActionTaken = "Estado inicial"
		};

		var queue = new Queue<FarmerState>();
		var visited = new HashSet<FarmerState>();

		queue.Enqueue(root);
		visited.Add(root);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();

			if (current.IsGoal)
				return SolutionResult.Solved(BuildPath(current));

			foreach (var successor in GetSuccessors(current))
			{
				if (!visited.Contains(successor))
				{
					visited.Add(successor);
					queue.Enqueue(successor);
				}
			}
		}

		return SolutionResult.Unsolvable();
	}

	private static IEnumerable<FarmerState> GetSuccessors(FarmerState state)
	{
		var destination = Opposite(state.Farmer);

		var candidates = new[]
		{
			TryMove(state, destination, false, false, false, "Solo el granjero cruza"),
			state.Wolf    == state.Farmer ? TryMove(state, destination, true,  false, false, "El granjero cruza con el lobo")  : null,
			state.Goat    == state.Farmer ? TryMove(state, destination, false, true,  false, "El granjero cruza con la cabra") : null,
			state.Cabbage == state.Farmer ? TryMove(state, destination, false, false, true,  "El granjero cruza con la col")   : null,
		};

		return candidates.Where(c => c is not null).Cast<FarmerState>();
	}

	private static FarmerState? TryMove(
		FarmerState parent,
		Side destination,
		bool moveWolf,
		bool moveGoat,
		bool moveCabbage,
		string action)
	{
		var candidate = new FarmerState
		{
			Parent = parent,
			Depth = parent.Depth + 1,
			ActionTaken = action,
			Farmer = destination,
			Wolf = moveWolf ? destination : parent.Wolf,
			Goat = moveGoat ? destination : parent.Goat,
			Cabbage = moveCabbage ? destination : parent.Cabbage
		};

		return candidate.IsUnsafe ? null : candidate;
	}

	private static List<MoveStep> BuildPath(FarmerState goal)
	{
		var reversed = new List<FarmerState>();
		for (var node = goal; node is not null; node = node.Parent)
			reversed.Add(node);

		reversed.Reverse();

		return reversed
			.Select((state, index) => new MoveStep(
				StepNumber: index,
				Action: state.ActionTaken,
				Description: BuildDescription(state),
				FarmerSide: state.Farmer,
				WolfSide: state.Wolf,
				GoatSide: state.Goat,
				CabbageSide: state.Cabbage
			))
			.ToList();
	}

	private static string BuildDescription(FarmerState state)
	{
		var left = new List<string>();
		var right = new List<string>();

		void Assign(string name, Side side)
		{
			if (side == Side.Left) left.Add(name);
			else right.Add(name);
		}

		Assign("Granjero", state.Farmer);
		Assign("Lobo", state.Wolf);
		Assign("Cabra", state.Goat);
		Assign("Col", state.Cabbage);

		return $"Izquierda: [{string.Join(", ", left)}]  →  Derecha: [{string.Join(", ", right)}]";
	}


	private static Side Opposite(Side side) =>
		side == Side.Left ? Side.Right : Side.Left;
}