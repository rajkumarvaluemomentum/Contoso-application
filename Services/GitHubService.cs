using System.Net.Http.Headers;
using System.Text.Json;
 
namespace VirtualAssistant.API.Services
{
    public class GitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly string _username;
 
        public GitHubService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
           
            // Get credentials from environment variables first, then configuration
            _token = Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? configuration["GitHub:Token"];
            _username = Environment.GetEnvironmentVariable("GITHUB_USERNAME") ?? configuration["GitHub:Username"];
 
            _httpClient.BaseAddress = new Uri("https://api.github.com/");
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("VirtualAssistantAPI", "1.0"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
 
        // Fetch all repositories
        public async Task<IEnumerable<string>> GetRepositoriesAsync()
        {
            var response = await _httpClient.GetAsync($"users/{_username}/repos");
            response.EnsureSuccessStatusCode();
 
            var json = await response.Content.ReadAsStringAsync();
 
            // Deserialize only the "name" property from each repo
            using var document = JsonDocument.Parse(json);
            var repoNames = document.RootElement
                .EnumerateArray()
                .Select(repo => repo.GetProperty("name").GetString()!)
                .ToList();
 
            return repoNames;
        }
 
        // Fetch deployments for a specific repo
        public async Task<IEnumerable<object>> GetDeploymentsAsync(string repoName)
        {
            var response = await _httpClient.GetAsync($"repos/{_username}/{repoName}/deployments");
            response.EnsureSuccessStatusCode();
 
            var json = await response.Content.ReadAsStringAsync();
            var deployments = JsonSerializer.Deserialize<IEnumerable<object>>(json);
            return deployments ?? Enumerable.Empty<object>();
        }
    }
}