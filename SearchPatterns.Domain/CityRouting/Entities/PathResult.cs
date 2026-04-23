namespace SearchPatterns.Domain.CityRouting.Entities;

/// <summary>
/// DTO de Dominio para retornar los resultados del cálculo de la ruta más corta.
/// </summary>
public class PathResult
{
    public bool IsSuccessful { get; }
    public List<string> RouteDescriptions { get; }
    public double TotalDistanceKm { get; }
    public string Message { get; }
    public List<ExplorationStep> ExplorationSteps { get; }

    private PathResult(bool isSuccessful, List<string> routeDescriptions, double totalDistance, string message, List<ExplorationStep> explorationSteps)
    {
        IsSuccessful = isSuccessful;
        RouteDescriptions = routeDescriptions;
        TotalDistanceKm = totalDistance;
        Message = message;
        ExplorationSteps = explorationSteps;
    }

    public static PathResult Success(List<string> route, double totalDistance, List<ExplorationStep> explorationSteps)
    {
        return new PathResult(true, route, totalDistance, $"Ruta encontrada exitosamente. Distancia estimada: {totalDistance} km", explorationSteps);
    }

    public static PathResult Failure(string message)
    {
        return new PathResult(false, new List<string>(), 0, message, new List<ExplorationStep>());
    }
}

public class ExplorationStep
{
    public int Step { get; set; }
    public string City { get; set; } = "";
    public double G { get; set; }
    public double H { get; set; }
    public double F { get; set; }
    public string FromCity { get; set; } = "";
    public string Reason { get; set; } = "";
}
