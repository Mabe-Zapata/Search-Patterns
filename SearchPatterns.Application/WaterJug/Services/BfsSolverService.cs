using SearchPatterns.Domain.WaterJug.Entities;
using SearchPatterns.Domain.WaterJug.Enums;
using SearchPatterns.Domain.WaterJug.Interfaces;

namespace SearchPatterns.Application.WaterJug.Services;

/// <summary>
/// BFS (Breadth-First Search) solver for the Water Jug Problem.
/// Implements the IBfsSolver interface to find the shortest path to the target amount.
/// </summary>
public class BfsSolverService : IBfsSolver
{
    /// <summary>
    /// Solves the water jug problem using BFS algorithm.
    /// </summary>
    /// <param name="jugACapacity">Capacity of Jug A.</param>
    /// <param name="jugBCapacity">Capacity of Jug B.</param>
    /// <param name="targetAmount">Target amount of water to measure.</param>
    /// <returns>A SolutionPath containing the steps to reach the goal state.</returns>
    public SolutionPath Solve(int jugACapacity, int jugBCapacity, int targetAmount)
    {
        // Validate inputs
        if (jugACapacity <= 0)
            throw new ArgumentException("Jug A capacity must be a positive integer.", nameof(jugACapacity));
        if (jugBCapacity <= 0)
            throw new ArgumentException("Jug B capacity must be a positive integer.", nameof(jugBCapacity));
        if (targetAmount <= 0)
            throw new ArgumentException("Target amount must be a positive integer.", nameof(targetAmount));
        if (targetAmount > Math.Max(jugACapacity, jugBCapacity))
            throw new ArgumentException("Target amount cannot exceed the capacity of both jugs.", nameof(targetAmount));

        // Initial state: both jugs empty
        var initialState = new WaterState(0, 0);
        
        // If target is 0, we're already at goal
        if (targetAmount == 0)
            return SolutionPath.Solvable(initialState, new List<OperationStep>(), initialState);

        // BFS data structures
        var queue = new Queue<(WaterState State, List<OperationStep> Steps)>();
        var visited = new HashSet<string>();
        
        // Initialize with starting state
        queue.Enqueue((initialState, new List<OperationStep>()));
        visited.Add(initialState.ToString());
        
        // BFS traversal
        while (queue.Count > 0)
        {
            var (currentState, currentSteps) = queue.Dequeue();
            
            // Check if we reached the goal
            if (currentState.IsGoalState(targetAmount))
            {
                return SolutionPath.Solvable(initialState, currentSteps, currentState);
            }
            
            // Generate all possible operations from current state
            var nextStates = GenerateNextStates(currentState, jugACapacity, jugBCapacity);
            
            foreach (var (nextState, operation, description) in nextStates)
            {
                var stateKey = nextState.ToString();
                
                if (!visited.Contains(stateKey))
                {
                    visited.Add(stateKey);
                    
                    var newSteps = new List<OperationStep>(currentSteps)
                    {
                        new OperationStep(operation, description, nextState)
                    };
                    
                    queue.Enqueue((nextState, newSteps));
                }
            }
        }
        
        // No solution found
        return SolutionPath.Unsolvable(initialState);
    }

    /// <summary>
    /// Generates all possible next states from the current state using the 6 operations.
    /// </summary>
    private List<(WaterState State, OperationType Operation, string Description)> GenerateNextStates(
        WaterState state, 
        int jugACapacity, 
        int jugBCapacity)
    {
        var results = new List<(WaterState, OperationType, string)>();
        
        // Fill A
        var fillAState = new WaterState(jugACapacity, state.JugBCurrent);
        results.Add((fillAState, OperationType.FillA, $"Llenar Jarra A ({jugACapacity}L)"));
        
        // Fill B
        var fillBState = new WaterState(state.JugACurrent, jugBCapacity);
        results.Add((fillBState, OperationType.FillB, $"Llenar Jarra B ({jugBCapacity}L)"));
        
        // Empty A
        var emptyAState = new WaterState(0, state.JugBCurrent);
        results.Add((emptyAState, OperationType.EmptyA, "Vaciar Jarra A"));
        
        // Empty B
        var emptyBState = new WaterState(state.JugACurrent, 0);
        results.Add((emptyBState, OperationType.EmptyB, "Vaciar Jarra B"));
        
        // Transfer A to B
        var (newA, newB) = Transfer(state.JugACurrent, state.JugBCurrent, jugACapacity, jugBCapacity);
        var transferAtoBState = new WaterState(newA, newB);
        var transferAmountAtoB = Math.Min(state.JugACurrent, jugBCapacity - state.JugBCurrent);
        results.Add((transferAtoBState, OperationType.TransferAtoB, 
            $"Transferir {transferAmountAtoB}L de Jarra A a Jarra B"));
        
        // Transfer B to A
        var (newA2, newB2) = Transfer(state.JugBCurrent, state.JugACurrent, jugBCapacity, jugACapacity);
        var transferBtoAState = new WaterState(newA2, newB2);
        var transferAmountBtoA = Math.Min(state.JugBCurrent, jugACapacity - state.JugACurrent);
        results.Add((transferBtoAState, OperationType.TransferBtoA, 
            $"Transferir {transferAmountBtoA}L de Jarra B a Jarra A"));
        
        return results;
    }

    /// <summary>
    /// Simulates transferring water from one jug to another.
    /// </summary>
    private (int SourceNewAmount, int DestinationNewAmount) Transfer(
        int sourceAmount, 
        int destinationAmount, 
        int sourceCapacity, 
        int destinationCapacity)
    {
        int availableSpace = destinationCapacity - destinationAmount;
        int transferAmount = Math.Min(sourceAmount, availableSpace);
        
        return (sourceAmount - transferAmount, destinationAmount + transferAmount);
    }
}