using SearchPatterns.Application.CityRouting.Enums;
using SearchPatterns.Domain.CityRouting.Entities;

namespace SearchPatterns.Application.CityRouting.Services;

public interface ICityRoutingService
{
    /// <summary>
    /// Calcula la mejor ruta usando una de las estrategias especificadas en el Enum.
    /// </summary>
    PathResult CalculateRoute(string startCityName, string targetCityName, RoutingAlgorithmType algorithmType);
    
    /// <summary>
    /// Obtiene las ciudades disponibles para mostrarlas en la UI.
    /// </summary>
    IEnumerable<string> GetAvailableCities();
}
