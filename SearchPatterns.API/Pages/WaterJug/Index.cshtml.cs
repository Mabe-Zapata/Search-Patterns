using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SearchPatterns.Application.WaterJug.DTOs;

namespace SearchPatterns.API.Pages.WaterJug;

public class IndexModel : PageModel
{
    [BindProperty]
    public int JugACapacity { get; set; } = 4;

    [BindProperty]
    public int JugBCapacity { get; set; } = 3;

    [BindProperty]
    public int TargetAmount { get; set; } = 2;

    public SolveResponseDto? Solution { get; set; }

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostSolveAsync()
    {
        try
        {
            var client = HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            
            var request = new SolveRequestDto
            {
                JugACapacity = JugACapacity,
                JugBCapacity = JugBCapacity,
                TargetAmount = TargetAmount
            };

            var json = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync($"{baseUrl}/api/waterjug/solve", content);
            
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

        return Page();
    }
}