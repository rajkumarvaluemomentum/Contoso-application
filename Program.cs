using VirtualAssistant.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure port for Render deployment (you already have this, which is good)
if (!builder.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Register GitHub service with HttpClient
builder.Services.AddHttpClient<GitHubService>()
    .ConfigureHttpClient((serviceProvider, httpClient) =>
    {
        // This allows GitHubService to use the injected HttpClient (with default configuration)
        httpClient.DefaultRequestHeaders.Add("User-Agent", "VirtualAssistantAPI");
    });

// Register KnowledgeSourceService if needed (assuming it's part of your services)
builder.Services.AddScoped<KnowledgeSourceService>();

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAll");

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI();

// Only use HTTPS redirection in development (can be changed for production if needed)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Use Authorization middleware if your app has authentication
app.UseAuthorization();

// Add a simple root endpoint
app.MapGet("/", () => "Virtual Assistant API is running! Visit /swagger for API documentation.");

// Map controllers for routing
app.MapControllers();

app.Run();
