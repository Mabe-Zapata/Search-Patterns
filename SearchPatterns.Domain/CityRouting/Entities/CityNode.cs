namespace SearchPatterns.Domain.CityRouting.Entities;

/// <summary>
/// Representa una ciudad en el mapa (un nodo en el grafo).
/// </summary>
public class CityNode
{
    public string Name { get; }
    
    // Coordenadas ficticias (Latitud y Longitud) usadas para la heurística en A*
    public double Latitude { get; }
    public double Longitude { get; }

    public CityNode(string name, double latitude, double longitude)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    // Calcula la distancia aproximada en línea recta hacia otra ciudad (Equivalente simple de distancia Euclidiana)
    public double DistanceTo(CityNode other)
    {
        // Aplicamos una heurística simple usando el teorema de Pitágoras
        // (Nota: En un mapa real se usaría la fórmula de Haversine con lat/lon de la tierra,
        // pero esto es suficiente para que la heurística de A* funcione a escala).
        // Multiplicamos por 111 porque 1 grado de lat/lon equivale aproximadamente a 111 km.
        var degDistance = Math.Sqrt(Math.Pow(Latitude - other.Latitude, 2) + Math.Pow(Longitude - other.Longitude, 2));
        return degDistance * 111.0; 
    }

    public override bool Equals(object? obj)
    {
        if (obj is CityNode other)
        {
            return Name == other.Name;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString() => Name;
}
