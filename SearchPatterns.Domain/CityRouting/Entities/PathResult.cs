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

    private PathResult(bool isSuccessful, List<string> routeDescriptions, double totalDistance, string message)
    {
        IsSuccessful = isSuccessful;
        RouteDescriptions = routeDescriptions;
        TotalDistanceKm = totalDistance;
        Message = message;
    }

    public static PathResult Success(List<string> route, double totalDistance)
    {
        return new PathResult(true, route, totalDistance, $"Ruta encontrada exitosamente. Distancia estimada: {totalDistance} km");
    }

    public static PathResult Failure(string message)
    {
        return new PathResult(false, new List<string>(), 0, message);
    }
}
