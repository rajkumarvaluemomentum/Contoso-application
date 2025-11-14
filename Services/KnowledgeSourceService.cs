using VirtualAssistant.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace VirtualAssistant.API.Services
{
    public class KnowledgeSourceService
    {
        private readonly IConfiguration _configuration;
        private readonly GitHubService _gitHubService;
        private readonly List<KnowledgeSource> _knowledgeSources;
        private readonly List<RepositoryLink> _repositoryLinks;
        private readonly List<ConfigurationInfo> _configurations;
        private readonly List<CodeModuleInfo> _modules;

        public KnowledgeSourceService(IConfiguration configuration, GitHubService gitHubService)
        {
            _configuration = configuration;
            _gitHubService = gitHubService;
            _knowledgeSources = new List<KnowledgeSource>();
            _repositoryLinks = new List<RepositoryLink>();
            _configurations = new List<ConfigurationInfo>();
            _modules = new List<CodeModuleInfo>();
            
            InitializeKnowledgeSources();
            InitializeConfigurations();
            InitializeModules();
        }

        private void InitializeKnowledgeSources()
        {
            // Initialize with default knowledge sources
            var gitHubUsername = _configuration["GitHub:Username"];
            
            _knowledgeSources.Add(new KnowledgeSource
            {
                Id = "github-main",
                Type = "GitHub",
                Name = "Virtual Assistant Bot - Main Repository",
                Url = $"https://github.com/{gitHubUsername}/Virtual_Assistant_Bot",
                Description = "Main GitHub repository for Virtual Assistant Bot project",
                Metadata = new Dictionary<string, string>
                {
                    { "owner", gitHubUsername },
                    { "branch", "Dev" },
                    { "visibility", "public" }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            });

            _knowledgeSources.Add(new KnowledgeSource
            {
                Id = "deployed-app",
                Type = "Deployment",
                Name = "Virtual Assistant API - Production",
                Url = "https://virtual-assistant-bot.onrender.com",
                Description = "Production deployment on Render",
                Metadata = new Dictionary<string, string>
                {
                    { "platform", "Render" },
                    { "environment", "production" },
                    { "region", "us-east" }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            });

            _knowledgeSources.Add(new KnowledgeSource
            {
                Id = "api-docs",
                Type = "Documentation",
                Name = "API Documentation - Swagger",
                Url = "https://virtual-assistant-bot.onrender.com/swagger",
                Description = "Interactive API documentation",
                Metadata = new Dictionary<string, string>
                {
                    { "type", "swagger" },
                    { "format", "openapi3" }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            });

            _knowledgeSources.Add(new KnowledgeSource
            {
                Id = "local-dev",
                Type = "Deployment",
                Name = "Local Development Environment",
                Url = "http://localhost:5206",
                Description = "Local development server",
                Metadata = new Dictionary<string, string>
                {
                    { "environment", "development" },
                    { "protocol", "http" },
                    { "port", "5206" }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            });
        }

        private void InitializeConfigurations()
        {
            _configurations.AddRange(new List<ConfigurationInfo>
            {
                new ConfigurationInfo
                {
                    Key = "GitHub:Username",
                    Value = _configuration["GitHub:Username"],
                    Environment = "All",
                    Description = "GitHub username for API authentication",
                    IsSensitive = false
                },
                new ConfigurationInfo
                {
                    Key = "GitHub:Token",
                    Value = "***hidden***",
                    Environment = "All",
                    Description = "GitHub personal access token",
                    IsSensitive = true
                },
                new ConfigurationInfo
                {
                    Key = "ConnectionStrings:DefaultConnection",
                    Value = _configuration["ConnectionStrings:DefaultConnection"]?.Substring(0, 20) + "***",
                    Environment = "Development",
                    Description = "SQL Server connection string",
                    IsSensitive = true
                },
                new ConfigurationInfo
                {
                    Key = "PORT",
                    Value = "10000",
                    Environment = "Production",
                    Description = "Server port for Render deployment",
                    IsSensitive = false
                },
                new ConfigurationInfo
                {
                    Key = "ASPNETCORE_ENVIRONMENT",
                    Value = "Development",
                    Environment = "Development",
                    Description = "ASP.NET Core environment",
                    IsSensitive = false
                }
            });
        }

        private void InitializeModules()
        {
            _modules.AddRange(new List<CodeModuleInfo>
            {
                new CodeModuleInfo
                {
                    ModuleName = "GitHub Integration",
                    FilePath = "Services/GitHubService.cs",
                    Language = "C#",
                    Description = "Service for interacting with GitHub API to fetch repositories and deployments",
                    Dependencies = new List<string> { "Octokit", "HttpClient" },
                    ApiEndpoints = new List<ApiEndpointInfo>
                    {
                        new ApiEndpointInfo
                        {
                            Method = "GET",
                            Route = "api/github/repositories",
                            Description = "Fetch all GitHub repositories for the authenticated user",
                            Controller = "GitHubController",
                            ReturnType = "IEnumerable<string>"
                        },
                        new ApiEndpointInfo
                        {
                            Method = "GET",
                            Route = "api/github/repositories/{repoName}/deployments",
                            Description = "Fetch deployment history for a specific repository",
                            Controller = "GitHubController",
                            Parameters = new Dictionary<string, string> { { "repoName", "string" } },
                            ReturnType = "IEnumerable<object>"
                        }
                    }
                },
                new CodeModuleInfo
                {
                    ModuleName = "Database Context",
                    FilePath = "Data/ApplicationDbContext.cs",
                    Language = "C#",
                    Description = "Entity Framework Core database context for application data",
                    Dependencies = new List<string> { "Entity Framework Core", "SQL Server" },
                    ApiEndpoints = new List<ApiEndpointInfo>()
                },
                new CodeModuleInfo
                {
                    ModuleName = "API Configuration",
                    FilePath = "Program.cs",
                    Language = "C#",
                    Description = "Main application startup and configuration",
                    Dependencies = new List<string> { "Swagger", "CORS", "Entity Framework" },
                    ApiEndpoints = new List<ApiEndpointInfo>
                    {
                        new ApiEndpointInfo
                        {
                            Method = "GET",
                            Route = "/",
                            Description = "Root endpoint with API status",
                            Controller = "Program",
                            ReturnType = "string"
                        },
                        new ApiEndpointInfo
                        {
                            Method = "GET",
                            Route = "/swagger",
                            Description = "Swagger UI for API documentation",
                            Controller = "Swagger",
                            ReturnType = "HTML"
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Get all knowledge sources
        /// </summary>
        public IEnumerable<KnowledgeSource> GetAllKnowledgeSources()
        {
            return _knowledgeSources.Where(k => k.IsActive);
        }

        /// <summary>
        /// Get knowledge sources by type
        /// </summary>
        public IEnumerable<KnowledgeSource> GetKnowledgeSourcesByType(string type)
        {
            return _knowledgeSources.Where(k => k.Type == type && k.IsActive);
        }

        /// <summary>
        /// Get repository links with deployment URLs
        /// </summary>
        public RepositoryLink GetRepositoryLink(string repositoryName)
        {
            var gitHubUsername = _configuration["GitHub:Username"];
            var repoLink = _repositoryLinks.FirstOrDefault(r => r.RepositoryName.Equals(repositoryName, StringComparison.OrdinalIgnoreCase));
            
            if (repoLink == null)
            {
                // Create a new repository link
                repoLink = new RepositoryLink
                {
                    Id = Guid.NewGuid().ToString(),
                    RepositoryName = repositoryName,
                    Owner = gitHubUsername,
                    GitHubUrl = $"https://github.com/{gitHubUsername}/{repositoryName}",
                    GitLabUrl = $"https://gitlab.com/{gitHubUsername}/{repositoryName}",
                    DocumentationUrl = $"https://virtual-assistant-bot.onrender.com/docs/{repositoryName}",
                    DeploymentUrls = new List<DeploymentUrl>
                    {
                        new DeploymentUrl
                        {
                            Environment = "Development",
                            Url = "http://localhost:5206",
                            Status = "Active",
                            BuildStatus = "Success",
                            LastDeployed = DateTime.UtcNow,
                            DeploymentDetailsUrl = "http://localhost:5206/swagger"
                        },
                        new DeploymentUrl
                        {
                            Environment = "Production",
                            Url = "https://virtual-assistant-bot.onrender.com",
                            Status = "Active",
                            BuildStatus = "Success",
                            LastDeployed = DateTime.UtcNow.AddHours(-2),
                            DeploymentDetailsUrl = "https://virtual-assistant-bot.onrender.com/swagger"
                        }
                    },
                    DefaultBranch = "Dev",
                    LastUpdated = DateTime.UtcNow
                };

                _repositoryLinks.Add(repoLink);
            }

            return repoLink;
        }

        /// <summary>
        /// Search for code modules by keyword
        /// </summary>
        public List<CodeModuleInfo> SearchModules(string keyword)
        {
            keyword = keyword.ToLower();
            return _modules.Where(m => 
                m.ModuleName.ToLower().Contains(keyword) ||
                m.Description.ToLower().Contains(keyword) ||
                m.FilePath.ToLower().Contains(keyword)
            ).ToList();
        }

        /// <summary>
        /// Get API endpoints for a specific controller
        /// </summary>
        public List<ApiEndpointInfo> GetApiEndpoints(string controllerName = null)
        {
            var endpoints = new List<ApiEndpointInfo>();
            
            foreach (var module in _modules)
            {
                if (controllerName == null || module.ApiEndpoints.Any(e => e.Controller.Contains(controllerName, StringComparison.OrdinalIgnoreCase)))
                {
                    endpoints.AddRange(module.ApiEndpoints);
                }
            }

            return endpoints;
        }

        /// <summary>
        /// Get environment configurations
        /// </summary>
        public List<ConfigurationInfo> GetConfigurations(string environment = null)
        {
            return _configurations.Where(c => 
                environment == null || c.Environment == environment || c.Environment == "All"
            ).ToList();
        }

        /// <summary>
        /// Query knowledge base with semantic search
        /// </summary>
        public QueryResponse QueryKnowledgeBase(string query)
        {
            var response = new QueryResponse { Success = true };
            var lowerQuery = query.ToLower();

            // Check for repo links
            if (lowerQuery.Contains("repo") || lowerQuery.Contains("repository") || lowerQuery.Contains("github"))
            {
                var repoLink = GetRepositoryLink("Virtual_Assistant_Bot");
                response.Data = repoLink;
                response.Message = $"Found repository: {repoLink.RepositoryName}";
                response.RelatedResources.Add(repoLink.GitHubUrl);
            }

            // Check for deployments
            if (lowerQuery.Contains("deploy") || lowerQuery.Contains("url") || lowerQuery.Contains("environment"))
            {
                var repoLink = GetRepositoryLink("Virtual_Assistant_Bot");
                response.Data = repoLink.DeploymentUrls;
                response.Message = "Found deployment URLs for various environments";
                response.RelatedResources.AddRange(repoLink.DeploymentUrls.Select(d => d.Url));
            }

            // Check for API endpoints
            if (lowerQuery.Contains("api") || lowerQuery.Contains("endpoint"))
            {
                var endpoints = GetApiEndpoints();
                response.Data = endpoints;
                response.Message = $"Found {endpoints.Count} API endpoints";
                response.RelatedResources.AddRange(endpoints.Select(e => $"{e.Method} {e.Route}"));
            }

            // Check for configurations
            if (lowerQuery.Contains("config") || lowerQuery.Contains("setting") || lowerQuery.Contains("environment"))
            {
                var configs = GetConfigurations();
                response.Data = configs;
                response.Message = $"Found {configs.Count} configurations";
            }

            // Check for module/implementation details
            if (lowerQuery.Contains("module") || lowerQuery.Contains("code") || lowerQuery.Contains("implementation"))
            {
                var keyword = lowerQuery.Replace("module", "").Replace("code", "").Replace("implementation", "").Trim();
                var modules = SearchModules(keyword.Length > 0 ? keyword : "");
                response.Data = modules.Any() ? modules : _modules;
                response.Message = $"Found {(modules.Any() ? modules.Count : _modules.Count)} modules";
            }

            return response;
        }

        /// <summary>
        /// Get build status for a repository
        /// </summary>
        public async Task<object> GetBuildStatusAsync(string repoName)
        {
            try
            {
                var deployments = await _gitHubService.GetDeploymentsAsync(repoName);
                return new
                {
                    RepositoryName = repoName,
                    LatestDeployments = deployments.Take(5),
                    Status = "Success",
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    RepositoryName = repoName,
                    Status = "Error",
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Open repository link - returns the URL to open
        /// </summary>
        public string GetRepositoryOpenUrl(string repositoryName)
        {
            var repoLink = GetRepositoryLink(repositoryName);
            return repoLink.GitHubUrl;
        }

        /// <summary>
        /// Get deployment URL for specific environment
        /// </summary>
        public string GetDeploymentUrl(string repositoryName, string environment)
        {
            var repoLink = GetRepositoryLink(repositoryName);
            var deployment = repoLink.DeploymentUrls.FirstOrDefault(d => 
                d.Environment.Equals(environment, StringComparison.OrdinalIgnoreCase)
            );
            
            return deployment?.Url ?? "Not found";
        }

        /// <summary>
        /// Get all deployment information
        /// </summary>
        public List<DeploymentUrl> GetAllDeployments(string repositoryName)
        {
            var repoLink = GetRepositoryLink(repositoryName);
            return repoLink.DeploymentUrls;
        }

        /// <summary>
        /// Get code snippet for a module or file
        /// </summary>
        public CodeSnippet GetCodeSnippet(string filePath, int startLine = -1, int endLine = -1)
        {
            return new CodeSnippet
            {
                Id = Guid.NewGuid().ToString(),
                FilePath = filePath,
                StartLine = startLine,
                EndLine = endLine,
                Language = GetLanguageFromExtension(filePath),
                Description = $"Code snippet from {filePath}",
                Tags = new List<string> { "code-snippet", filePath }
            };
        }

        private string GetLanguageFromExtension(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".cs" => "csharp",
                ".json" => "json",
                ".js" => "javascript",
                ".ts" => "typescript",
                ".py" => "python",
                ".html" => "html",
                ".xml" => "xml",
                _ => "plaintext"
            };
        }
    }
}
