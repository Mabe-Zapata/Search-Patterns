using SearchPatterns.Application.CityRouting.Enums;

namespace SearchPatterns.Application.CityRouting.DTOs;

public class CityRoutingRequest
{
    public string StartCity { get; set; } = string.Empty;
    public string TargetCity { get; set; } = string.Empty;
    public RoutingAlgorithmType Algorithm { get; set; }
}
