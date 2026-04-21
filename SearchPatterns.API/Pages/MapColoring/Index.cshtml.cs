using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SearchPatterns.Application.MapColoring.DTOs;
using System.Text;

namespace SearchPatterns.API.Pages.MapColoring;

public class IndexModel : PageModel
{
    [BindProperty]
    public int RegionCount { get; set; } = 4;

    [BindProperty]
    public bool[,]? AdjacencyMatrix { get; set; }

    public SolveResponseDto? Solution { get; set; }

    public string? ErrorMessage { get; set; }

    public bool IsLoading { get; set; }

    public void OnGet()
    {
        InitializeMatrix();
    }

    public async Task<IActionResult> OnPostSolveAsync()
    {
        try
        {
            IsLoading = true;

            // Get the matrix from form and convert to int[][]
            var matrix = new int[RegionCount][];
            for (int i = 0; i < RegionCount; i++)
            {
                matrix[i] = new int[RegionCount];
                for (int j = 0; j < RegionCount; j++)
                {
                    var key = $"AdjacencyMatrix_{i}_{j}";
                    var value = Request.Form[key].FirstOrDefault();
                    matrix[i][j] = value == "on" ? 1 : 0;
                }
            }

            var client = HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var request = new SolveRequestDto
            {
                RegionCount = RegionCount,
                AdjacencyMatrix = matrix
            };

            var json = System.Text.Json.JsonSerializer.Serialize(request, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{baseUrl}/api/mapcoloring/solve", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                Solution = System.Text.Json.JsonSerializer.Deserialize<SolveResponseDto>(responseJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (Solution != null && !Solution.IsSolvable)
                {
                    ErrorMessage = Solution.ErrorMessage;
                }
            }
            else
            {
                ErrorMessage = "Error calling the API. Please try again.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }

        return Page();
    }

    public IActionResult OnPostUpdateMatrix()
    {
        InitializeMatrix();
        return Page();
    }

    private void InitializeMatrix()
    {
        if (RegionCount < 1) RegionCount = 1;
        if (RegionCount > 100) RegionCount = 100;
        AdjacencyMatrix = new bool[RegionCount, RegionCount];
    }

    public string GetMatrixHtml()
    {
        if (AdjacencyMatrix == null || RegionCount < 1)
        {
            return "<p>No hay matriz para mostrar</p>";
        }

        var sb = new StringBuilder();
        sb.AppendLine("<table class='matrix-table'>");
        
        // Header row
        sb.AppendLine("<tr><th></th>");
        for (int j = 0; j < RegionCount; j++)
        {
            sb.AppendLine($"<th>{j}</th>");
        }
        sb.AppendLine("</tr>");

        // Data rows
        for (int i = 0; i < RegionCount; i++)
        {
            sb.AppendLine($"<tr><th>{i}</th>");
            for (int j = 0; j < RegionCount; j++)
            {
                var checkedAttr = AdjacencyMatrix[i, j] ? "checked" : "";
                var disabledAttr = i == j ? "disabled" : "";
                sb.AppendLine($"<td><input type='checkbox' name='AdjacencyMatrix_{i}_{j}' {checkedAttr} {disabledAttr}></td>");
            }
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</table>");
        return sb.ToString();
    }

    public string GetGraphSvg()
    {
        if (Solution == null || !Solution.IsSolvable || Solution.ColorAssignment == null)
        {
            return "";
        }

        var sb = new StringBuilder();
        var size = 400;
        var centerX = size / 2;
        var centerY = size / 2;
        var radius = size / 2 - 50;

        sb.AppendLine($"<svg class='graph-svg' width='{size}' height='{size}' viewBox='0 0 {size} {size}'>");

        // Calculate positions in a circle
        var positions = new (double x, double y)[RegionCount];
        for (int i = 0; i < RegionCount; i++)
        {
            var angle = 2 * Math.PI * i / RegionCount - Math.PI / 2;
            positions[i] = (
                centerX + radius * Math.Cos(angle),
                centerY + radius * Math.Sin(angle)
            );
        }

        // Draw edges
        for (int i = 0; i < RegionCount; i++)
        {
            for (int j = i + 1; j < RegionCount; j++)
            {
                if (AdjacencyMatrix != null && AdjacencyMatrix[i, j])
                {
                    sb.AppendLine($"<line class='edge-line' x1='{positions[i].x}' y1='{positions[i].y}' x2='{positions[j].x}' y2='{positions[j].y}' />");
                }
            }
        }

        // Draw nodes
        var colorMap = new Dictionary<string, string>
        {
            { "Red", "#FF0000" },
            { "Blue", "#0000FF" },
            { "Green", "#00FF00" },
            { "Yellow", "#FFFF00" }
        };

        for (int i = 0; i < RegionCount; i++)
        {
            var color = Solution.ColorAssignment.ContainsKey(i) 
                ? Solution.ColorAssignment[i] 
                : "Gray";
            var fillColor = colorMap.ContainsKey(color) ? colorMap[color] : "#808080";
            
            sb.AppendLine($"<circle class='region-circle' cx='{positions[i].x}' cy='{positions[i].y}' r='25' fill='{fillColor}' />");
            sb.AppendLine($"<text class='region-label' x='{positions[i].x}' y='{positions[i].y}' dominant-baseline='middle'>{i}</text>");
        }

        sb.AppendLine("</svg>");
        return sb.ToString();
    }
}