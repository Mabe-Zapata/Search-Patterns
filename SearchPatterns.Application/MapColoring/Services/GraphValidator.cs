using SearchPatterns.Domain.MapColoring.Entities;
using SearchPatterns.Domain.MapColoring.Interfaces;

namespace SearchPatterns.Application.MapColoring.Services;

/// <summary>
/// Service for validating graph inputs for the map coloring problem.
/// </summary>
public class GraphValidator : IGraphValidator
{
    /// <summary>
    /// Validates a graph structure.
    /// </summary>
    public ValidationResult Validate(
        int regionCount, 
        Dictionary<int, List<int>>? adjacencyList, 
        int[][]? adjacencyMatrix)
    {
        var errors = new List<string>();

        // Validate region count (1-100)
        if (regionCount < 1)
        {
            errors.Add("El número de regiones debe ser al menos 1");
        }
        
        if (regionCount > 100)
        {
            errors.Add("El número de regiones no puede exceder 100");
        }

        // Validate format: exactly one of adjacency list or matrix must be provided
        if (adjacencyList == null && adjacencyMatrix == null)
        {
            errors.Add("Debe proporcionar una lista de adyacencia o una matriz de adyacencia");
        }
        
        if (adjacencyList != null && adjacencyMatrix != null)
        {
            errors.Add("Debe proporcionar solo una representación: lista de adyacencia o matriz de adyacencia");
        }

        // If region count is invalid, return early
        if (regionCount < 1 || regionCount > 100)
        {
            return errors.Count > 0 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();
        }

        // Validate adjacency list if provided
        if (adjacencyList != null)
        {
            ValidateAdjacencyList(regionCount, adjacencyList, errors);
        }

        // Validate adjacency matrix if provided
        if (adjacencyMatrix != null)
        {
            ValidateAdjacencyMatrix(regionCount, adjacencyMatrix, errors);
        }

        return errors.Count > 0 
            ? ValidationResult.Failure(errors) 
            : ValidationResult.Success();
    }

    private void ValidateAdjacencyList(
        int regionCount, 
        Dictionary<int, List<int>> adjacencyList, 
        List<string> errors)
    {
        // Validate all region IDs are in range [0, RegionCount-1]
        foreach (var kvp in adjacencyList)
        {
            int regionId = kvp.Key;
            if (regionId < 0 || regionId >= regionCount)
            {
                errors.Add($"ID de región inválido: {regionId}. Debe estar en el rango [0, {regionCount - 1}]");
            }

            foreach (var adjacentId in kvp.Value)
            {
                if (adjacentId < 0 || adjacentId >= regionCount)
                {
                    errors.Add($"ID de región adyacente inválido: {adjacentId}. Debe estar en el rango [0, {regionCount - 1}]");
                }
            }
        }

        // Validate no self-loops (region not adjacent to itself)
        foreach (var kvp in adjacencyList)
        {
            if (kvp.Value.Contains(kvp.Key))
            {
                errors.Add($"Auto-bucle detectado: la región {kvp.Key} no puede ser adyacente a sí misma");
            }
        }

        // Validate symmetry: if A is adjacent to B, then B is adjacent to A
        foreach (var kvp in adjacencyList)
        {
            int regionA = kvp.Key;
            foreach (var regionB in kvp.Value)
            {
                // Check if regionB exists in the adjacency list
                if (!adjacencyList.ContainsKey(regionB))
                {
                    errors.Add($"Adyacencia asimétrica: la región {regionA} es adyacente a {regionB}, pero {regionB} no tiene lista de adyacencia");
                    continue;
                }

                // Check if regionA is in regionB's adjacency list
                if (!adjacencyList[regionB].Contains(regionA))
                {
                    errors.Add($"Adyacencia asimétrica: la región {regionA} es adyacente a {regionB}, pero {regionB} no es adyacente a {regionA}");
                }
            }
        }
    }

    private void ValidateAdjacencyMatrix(
        int regionCount, 
        int[][] adjacencyMatrix, 
        List<string> errors)
    {
        // Validate matrix dimensions are RegionCount × RegionCount
        if (adjacencyMatrix.Length != regionCount)
        {
            errors.Add($"Las dimensiones de la matriz de adyacencia deben ser {regionCount}x{regionCount}, pero son {adjacencyMatrix.Length}x?");
            return; // Can't validate further if dimensions are wrong
        }

        for (int i = 0; i < adjacencyMatrix.Length; i++)
        {
            if (adjacencyMatrix[i] == null || adjacencyMatrix[i].Length != regionCount)
            {
                errors.Add($"Las dimensiones de la matriz de adyacencia deben ser {regionCount}x{regionCount}, pero la fila {i} tiene longitud {adjacencyMatrix[i]?.Length ?? 0}");
                return;
            }
        }

        // Validate no self-loops (diagonal elements are 0)
        for (int i = 0; i < regionCount; i++)
        {
            if (adjacencyMatrix[i][i] != 0)
            {
                errors.Add($"Auto-bucle detectado: la región {i} no puede ser adyacente a sí misma (matriz[{i}][{i}] debe ser 0)");
            }
        }

        // Validate symmetry: matrix[i][j] == matrix[j][i]
        for (int i = 0; i < regionCount; i++)
        {
            for (int j = i + 1; j < regionCount; j++)
            {
                if (adjacencyMatrix[i][j] != adjacencyMatrix[j][i])
                {
                    errors.Add($"Matriz de adyacencia asimétrica: matriz[{i}][{j}] = {adjacencyMatrix[i][j]}, pero matriz[{j}][{i}] = {adjacencyMatrix[j][i]}");
                }
            }
        }
    }
}
