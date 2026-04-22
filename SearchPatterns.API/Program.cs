var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register services for Water Jug functionality
builder.Services.AddScoped<SearchPatterns.Domain.WaterJug.Interfaces.IBfsSolver, SearchPatterns.Application.WaterJug.Services.BfsSolverService>();
builder.Services.AddScoped<SearchPatterns.Application.WaterJug.Validators.IWaterJugValidator, SearchPatterns.Application.WaterJug.Validators.WaterJugValidator>();

// Register services for City Routing functionality
builder.Services.AddScoped<SearchPatterns.Domain.CityRouting.Interfaces.IShortestPathStrategy, SearchPatterns.Application.CityRouting.Strategies.DijkstraStrategy>();
builder.Services.AddScoped<SearchPatterns.Domain.CityRouting.Interfaces.IShortestPathStrategy, SearchPatterns.Application.CityRouting.Strategies.AStarStrategy>();
builder.Services.AddScoped<SearchPatterns.Application.CityRouting.Services.ICityRoutingService, SearchPatterns.Application.CityRouting.Services.CityRoutingService>();

//registe services for FarmerPuzzle
builder.Services.AddScoped<SearchPatterns.Domain.FarmerPuzzle.Interfaces.IFarmerBfsSolver,
	SearchPatterns.Application.FarmerPuzzle.Services.FarmerBfsSolverService>();

// Add CORS to allow local front-end requests (required if Blazor makes client-side requests or for Swagger UI)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
