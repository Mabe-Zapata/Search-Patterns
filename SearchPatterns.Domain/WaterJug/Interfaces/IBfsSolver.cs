using SearchPatterns.Domain.WaterJug.Entities;

namespace SearchPatterns.Domain.WaterJug.Interfaces;

/// <summary>
/// Interface for BFS solver that solves the Water Jug Problem.
/// </summary>
public interface IBfsSolver
{
    /// <summary>
    /// Solves the water jug problem using BFS algorithm.
    /// </summary>
    /// <param name="jugACapacity">Capacity of Jug A.</param>
    /// <param name="jugBCapacity">Capacity of Jug B.</param>
    /// <param name="targetAmount">Target amount of water to measure.</param>
    /// <returns>A SolutionPath containing the steps to reach the goal state.</returns>
    SolutionPath Solve(int jugACapacity, int jugBCapacity, int targetAmount);
}