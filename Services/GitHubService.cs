using Contoso_application.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace VirtualAssistant.API.Services
{
    public class GitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly string _username;
        private readonly Dictionary<string, List<DeploymentInfo>> _deploymentData = new()
    {
        {
            "Contoso-App", new List<DeploymentInfo>
            {
                new DeploymentInfo{ EnvironmentName="Dev", DeploymentUrl="virtualassistantapi20251114050156-ctcwaafdd6gmhyfa.canadacentral-01.azurewebsites.net"},
                new DeploymentInfo{ EnvironmentName="Stagging",DeploymentUrl="https://uat.contoso.com"},
                new DeploymentInfo{ EnvironmentName="UAT", DeploymentUrl="https://uat.contoso.com"},
                new DeploymentInfo{ EnvironmentName="Production", DeploymentUrl="https://www.contoso.com"}
            }
        }
    };
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

        #region // Fetch all repositories
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
        #endregion

        #region // GetRepositoryLink  by  repository Name 
        // Check if a repository exists and return its URL
        // Check if a repository exists and return its simplified URL data
        public async Task<RepoLink> GetRepositoryLinkAsync(string repoName)
        {
            var response = await _httpClient.GetAsync($"https://api.github.com/repos/{_username}/{repoName}");

            // If the repository does not exist or any error occurs, return null
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            // Create a simplified object for the repository link
            return new RepoLink
            {
                Result = $"https://github.com/{_username}/{repoName}",
                Id = 243 // You can fetch the actual repository ID here if needed
            };
        }


        // Fetches files/folders from GitHub repo root
        private async Task<JArray?> GetRepositoryRoot(string repoName)
        {
            var url = $"https://api.github.com/repos/{_username}/{repoName}/contents";
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("request");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JArray.Parse(json);
        }

        public async Task<ApplicationGitHubResponse?> GetApplicationStructure(string appName)
        {
            var contents = await GetRepositoryRoot(appName);
            if (contents == null) return null;

            // Get all top-level folders as modules
            var modules = contents
                .Where(item => item["type"]?.ToString() == "dir")
                .Select(item => new GitHubModuleInfo
                {
                    Name = item["name"]?.ToString(),
                    Path = item["path"]?.ToString(),
                    Url = item["html_url"]?.ToString()
                })
                .ToList();

            // Detect document upload module by keywords
            var uploadModule = modules.FirstOrDefault(m =>
                m.Name.Contains("upload", StringComparison.OrdinalIgnoreCase) ||
                m.Name.Contains("document", StringComparison.OrdinalIgnoreCase) ||
                m.Name.Contains("file", StringComparison.OrdinalIgnoreCase));

            // Detect microservices folder
            var microservices = modules
                .Where(m =>
                    m.Name.Contains("service", StringComparison.OrdinalIgnoreCase) ||
                    m.Name.Contains("api", StringComparison.OrdinalIgnoreCase))
                .ToList();

            return new ApplicationGitHubResponse
            {
                ApplicationName = appName,
                Modules = modules,
                DocumentUploadModule = uploadModule,
                Microservices = microservices
            };
        }

        #endregion

        #region //Fetches API endpoints from GitHub using repository names
        //✔ Fetches Swagger URL
        //✔ Fetches authentication API endpoint

        // Read raw controller file
        private async Task<string?> GetControllerCode(string repo, string controllerName)
        {
            var url = $"https://api.github.com/repos/{_username}/{repo}/contents";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<JsonElement>(json);

            foreach (var item in items.EnumerateArray())
            {
                string fileName = item.GetProperty("name").GetString();
                if (fileName.Equals(controllerName + ".cs", StringComparison.OrdinalIgnoreCase))
                {
                    string fileUrl = item.GetProperty("download_url").GetString();
                    return await _httpClient.GetStringAsync(fileUrl);
                }
            }

            return null;
        }

        public async Task<List<ActionEndpointInfo>> GetEndpoints(string repo, string controller)
        {
            var list = new List<ActionEndpointInfo>();

            string? code = await GetControllerCode(repo, controller);
            if (code == null) return list;

            // Regex to extract HTTP attributes and method names
            var methodRegex = new Regex(@"\[(HttpGet|HttpPost|HttpPut|HttpDelete)(?:\(""?(.*?)""?\))?\]\s*public.*?(\w+)\s*\(", RegexOptions.Singleline);

            foreach (Match match in methodRegex.Matches(code))
            {
                string httpMethod = match.Groups[1].Value;
                string routeFromAttribute = match.Groups[2].Value;
                string actionMethod = match.Groups[3].Value;

                string baseRoute = $"/{controller.Replace("Controller", "").ToLower()}";

                string finalRoute;

                if (!string.IsNullOrEmpty(routeFromAttribute))
                    finalRoute = $"{baseRoute}/{routeFromAttribute}";
                else
                    finalRoute = $"{baseRoute}/{actionMethod}";

                list.Add(new ActionEndpointInfo
                {
                    ActionName = actionMethod,
                    HttpMethod = httpMethod.ToUpper(),
                    Route = finalRoute
                });
            }

            return list;
        }
        #endregion


        private async Task<string?> GetFileContent(string repo, string filePath)
        {
            string url = $"https://api.github.com/repos/{_username}/{repo}/contents/{filePath}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var downloadUrl = doc.RootElement.GetProperty("download_url").GetString();
            if (downloadUrl == null) return null;

            return await _httpClient.GetStringAsync(downloadUrl);
        }

        public async Task<AppConfiguration?> GetConfiguration(string repoName, string environment)
        {
            // Determine the file name based on environment
            string fileName = environment.ToLower() switch
            {
                "prod" => "appsettings.Production.json",
                "uat" => "appsettings.UAT.json",
                _ => "appsettings.json"
            };

            var fileContent = await GetFileContent(repoName, fileName);
            if (fileContent == null) return null;

            var settings = new Dictionary<string, string>();
            using var doc = JsonDocument.Parse(fileContent);
            FlattenJson(doc.RootElement, "", settings);

            return new AppConfiguration
            {
                ApplicationName = repoName,
                Settings = settings
            };
        }

        // Flatten nested JSON to key-value pairs
        private void FlattenJson(JsonElement element, string prefix, Dictionary<string, string> dict)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var prop in element.EnumerateObject())
                    {
                        FlattenJson(prop.Value, string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}:{prop.Name}", dict);
                    }
                    break;
                case JsonValueKind.Array:
                    int index = 0;
                    foreach (var item in element.EnumerateArray())
                    {
                        FlattenJson(item, $"{prefix}:{index}", dict);
                        index++;
                    }
                    break;
                default:
                    dict[prefix] = element.ToString()!;
                    break;
            }
        }
        // List all files in the root folder of the repo
        private async Task<JsonElement?> GetRepoContent(string repo, string path = "")
        {
            string url = $"https://api.github.com/repos/{_username}/{repo}/contents/{path}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonElement>(json);
        }

        public async Task<List<RepoConfigFile>> GetConfigFiles(string repo)
        {
            var result = new List<RepoConfigFile>();

            var rootContents = await GetRepoContents(repo);
            if (rootContents == null) return result;

            foreach (var item in rootContents.Value.EnumerateArray())
            {
                string type = item.GetProperty("type").GetString()!;
                string name = item.GetProperty("name").GetString()!;
                string path = item.GetProperty("path").GetString()!;
                string downloadUrl = item.GetProperty("download_url").GetString() ?? "";

                // Filter for common config files
                if (type == "file" &&
                    (name.StartsWith("appsettings") && name.EndsWith(".json") ||
                     name.EndsWith(".env") || name.EndsWith(".config")))
                {
                    result.Add(new RepoConfigFile
                    {
                        FileName = name,
                        FilePath = path,
                        DownloadUrl = downloadUrl
                    });
                }
            }

            return result;
        }
        // Get repository root files
        private async Task<JsonElement?> GetRepoContents(string repo, string path = "")
        {
            string url = $"https://api.github.com/repos/{_username}/{repo}/contents/{path}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonElement>(json);
        }

        // Search files by keyword in file name
        public async Task<List<CodeSnippet>> SearchCodeSnippets(string repo, string keyword)
        {
            var result = new List<CodeSnippet>();

            var rootContents = await GetRepoContents(repo);
            if (rootContents == null) return result;

            foreach (var item in rootContents.Value.EnumerateArray())
            {
                string type = item.GetProperty("type").GetString()!;
                string name = item.GetProperty("name").GetString()!;
                string path = item.GetProperty("path").GetString()!;
                string downloadUrl = item.GetProperty("download_url").GetString()!;

                // If it's a folder, recursively scan (optional)
                if (type == "dir")
                {
                    var nestedResults = await SearchCodeSnippetsRecursive(repo, path, keyword);
                    result.AddRange(nestedResults);
                }

                // Match keyword in file name (case-insensitive)
                if (type == "file" && name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    string content = await _httpClient.GetStringAsync(downloadUrl);
                    result.Add(new CodeSnippet
                    {
                        FileName = name,
                        FilePath = path,
                        DownloadUrl = downloadUrl,
                        Content = content
                    });
                }
            }

            return result;
        }

        // Recursive scan for nested folders
        private async Task<List<CodeSnippet>> SearchCodeSnippetsRecursive(string repo, string path, string keyword)
        {
            var result = new List<CodeSnippet>();

            var contents = await GetRepoContents(repo, path);
            if (contents == null) return result;

            foreach (var item in contents.Value.EnumerateArray())
            {
                string type = item.GetProperty("type").GetString()!;
                string name = item.GetProperty("name").GetString()!;
                string filePath = item.GetProperty("path").GetString()!;
                string downloadUrl = item.GetProperty("download_url").GetString()!;

                if (type == "dir")
                {
                    var nestedResults = await SearchCodeSnippetsRecursive(repo, filePath, keyword);
                    result.AddRange(nestedResults);
                }

                if (type == "file" && name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    string content = await _httpClient.GetStringAsync(downloadUrl);
                    result.Add(new CodeSnippet
                    {
                        FileName = name,
                        FilePath = filePath,
                        DownloadUrl = downloadUrl,
                        Content = content
                    });
                }
            }

            return result;
        }


        // Get repo details
        public async Task<RepositoryDetails?> GetRepoDetails(string repo)
        {
            string url = $"https://api.github.com/repos/{_username}/{repo}";
            var resp = await _httpClient.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new RepositoryDetails
            {
                RepoName = root.GetProperty("name").GetString(),
                Description = root.GetProperty("description").GetString(),
                DefaultBranch = root.GetProperty("default_branch").GetString(),
                HtmlUrl = root.GetProperty("html_url").GetString(),
                CloneUrl = root.GetProperty("clone_url").GetString()
            };
        }

        // Get latest build status from GitHub Actions
        public async Task<BuildStatus?> GetLatestBuildStatus(string repo)
        {
            string url = $"https://api.github.com/repos/{_username}/{repo}/actions/runs?per_page=1";
            var resp = await _httpClient.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var run = doc.RootElement.GetProperty("workflow_runs")[0];

            return new BuildStatus
            {
                Status = run.GetProperty("status").GetString(),
                Conclusion = run.GetProperty("conclusion").GetString(),
                UpdatedAt = run.GetProperty("updated_at").GetDateTime()
            };
        }

        // Get deployment URL
        public DeploymentInfo? GetDeploymentUrl(string repo, string environment)
        {
            if (!_deploymentData.ContainsKey(repo)) return null;

            return _deploymentData[repo].FirstOrDefault(e =>
                e.EnvironmentName.Equals(environment, StringComparison.OrdinalIgnoreCase));
        }

        // GET /api/contoso/service-health?serviceName=QuoteService

        public ServiceHealth GetServiceHealth(string serviceName)
        {
            // For demo, you can implement real health check using HTTP requests
            return new ServiceHealth
            {
                ServiceName = serviceName,
                Status = "Healthy",
                CheckedAt = DateTime.UtcNow
            };
        }
    }
}
