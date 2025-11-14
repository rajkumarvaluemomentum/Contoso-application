using Microsoft.AspNetCore.Mvc;
using VirtualAssistant.API.Services;
using VirtualAssistant.API.Models;

namespace VirtualAssistant.API.Controllers
{
    /// <summary>
    /// GitHub Controller - Handles GitHub API integration and repository operations
    /// Enhanced with Knowledge Source integration for comprehensive repository queries
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly GitHubService _gitHubService;
        private readonly KnowledgeSourceService _knowledgeService;
        private readonly ILogger<GitHubController> _logger;

        public GitHubController(GitHubService gitHubService, KnowledgeSourceService knowledgeService, ILogger<GitHubController> logger)
        {
            _gitHubService = gitHubService;
            _knowledgeService = knowledgeService;
            _logger = logger;
        }

        [HttpGet("repositories")]
        public async Task<IActionResult> GetRepositories()
        {
            try
            {
                var repos = await _gitHubService.GetRepositoriesAsync();
                return Ok(repos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { error = ex.Message });
            }
        }

        [HttpGet("repositories/{repoName}/deployments")]
        public async Task<IActionResult> GetDeployments(string repoName)
        {
            try
            {
                var deployments = await _gitHubService.GetDeploymentsAsync(repoName);
                return Ok(deployments);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get enhanced repository information with knowledge base integration
        /// Returns repository metadata along with deployment URLs and configuration
        /// </summary>
        [HttpGet("repositories/{repoName}/info")]
        public IActionResult GetRepositoryInfo(string repoName)
        {
            try
            {
                var repoLink = _knowledgeService.GetRepositoryLink(repoName);
                return Ok(new
                {
                    success = true,
                    repository = repoName,
                    data = repoLink,
                    message = $"Retrieved complete repository information for {repoName}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving repository info for {repoName}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get repository with deployment URLs
        /// Combines GitHub repository info with deployment information from knowledge base
        /// </summary>
        [HttpGet("repositories/{repoName}/with-deployments")]
        public async Task<IActionResult> GetRepositoryWithDeployments(string repoName)
        {
            try
            {
                // Get deployments from GitHub
                var deployments = await _gitHubService.GetDeploymentsAsync(repoName);
                
                // Get repository link with URLs from knowledge base
                var repoLink = _knowledgeService.GetRepositoryLink(repoName);

                return Ok(new
                {
                    success = true,
                    repository = repoName,
                    gitHubDeployments = deployments,
                    deploymentUrls = repoLink.DeploymentUrls,
                    repositoryLink = new
                    {
                        gitHubUrl = repoLink.GitHubUrl,
                        gitLabUrl = repoLink.GitLabUrl,
                        documentationUrl = repoLink.DocumentationUrl,
                        owner = repoLink.Owner,
                        defaultBranch = repoLink.DefaultBranch
                    },
                    message = "Retrieved repository with deployment information"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "GitHub authentication failed");
                return Unauthorized(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GitHub API request failed");
                return StatusCode(502, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving repository with deployments");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get all repositories with their deployment information
        /// Lists all user repositories with corresponding deployment URLs from knowledge base
        /// </summary>
        [HttpGet("all-repositories-with-info")]
        public async Task<IActionResult> GetAllRepositoriesWithInfo()
        {
            try
            {
                var repos = await _gitHubService.GetRepositoriesAsync();
                var reposWithInfo = new List<object>();

                foreach (var repoName in repos)
                {
                    var repoLink = _knowledgeService.GetRepositoryLink(repoName);
                    reposWithInfo.Add(new
                    {
                        repositoryName = repoName,
                        gitHubUrl = repoLink.GitHubUrl,
                        gitLabUrl = repoLink.GitLabUrl,
                        documentationUrl = repoLink.DocumentationUrl,
                        deploymentUrls = repoLink.DeploymentUrls,
                        owner = repoLink.Owner,
                        defaultBranch = repoLink.DefaultBranch,
                        lastUpdated = repoLink.LastUpdated
                    });
                }

                return Ok(new
                {
                    success = true,
                    count = reposWithInfo.Count,
                    repositories = reposWithInfo,
                    message = $"Retrieved {reposWithInfo.Count} repositories with information"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "GitHub authentication failed");
                return Unauthorized(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GitHub API request failed");
                return StatusCode(502, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving repositories with information");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get deployment status and environment information
        /// Retrieves deployment status from GitHub and environment URLs from knowledge base
        /// </summary>
        [HttpGet("repositories/{repoName}/deployment-status")]
        public async Task<IActionResult> GetDeploymentStatus(string repoName)
        {
            try
            {
                var buildStatus = await _knowledgeService.GetBuildStatusAsync(repoName);
                var deploymentUrls = _knowledgeService.GetAllDeployments(repoName);

                return Ok(new
                {
                    success = true,
                    repository = repoName,
                    buildStatus = buildStatus,
                    deploymentEnvironments = deploymentUrls,
                    message = "Retrieved deployment status and environment information"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving deployment status for {repoName}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get repository deployment by environment
        /// Retrieves specific deployment URL for the requested environment
        /// </summary>
        [HttpGet("repositories/{repoName}/environment/{environment}/url")]
        public IActionResult GetEnvironmentDeploymentUrl(string repoName, string environment)
        {
            try
            {
                var url = _knowledgeService.GetDeploymentUrl(repoName, environment);
                
                if (url == "Not found")
                {
                    return NotFound(new
                    {
                        success = false,
                        repository = repoName,
                        environment = environment,
                        message = $"No deployment URL found for {environment} environment"
                    });
                }

                return Ok(new
                {
                    success = true,
                    repository = repoName,
                    environment = environment,
                    url = url,
                    message = $"Retrieved {environment} deployment URL for {repoName}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving deployment URL for {repoName} - {environment}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Search repositories by name pattern
        /// Searches through all repositories and returns matching ones with their information
        /// </summary>
        [HttpGet("search-repositories")]
        public async Task<IActionResult> SearchRepositories([FromQuery] string pattern)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pattern))
                {
                    return BadRequest(new { error = "Pattern parameter is required" });
                }

                var repos = await _gitHubService.GetRepositoriesAsync();
                var matchingRepos = repos.Where(r => r.Contains(pattern, StringComparison.OrdinalIgnoreCase)).ToList();

                var results = new List<object>();
                foreach (var repoName in matchingRepos)
                {
                    var repoLink = _knowledgeService.GetRepositoryLink(repoName);
                    results.Add(new
                    {
                        repositoryName = repoName,
                        gitHubUrl = repoLink.GitHubUrl,
                        deploymentUrls = repoLink.DeploymentUrls.Select(d => new { d.Environment, d.Url, d.Status })
                    });
                }

                return Ok(new
                {
                    success = true,
                    pattern = pattern,
                    matchCount = results.Count,
                    repositories = results,
                    message = $"Found {results.Count} repositories matching pattern '{pattern}'"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "GitHub authentication failed");
                return Unauthorized(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GitHub API request failed");
                return StatusCode(502, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching repositories");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get repository configuration and metadata
        /// Returns complete repository configuration including branches, deployment info, and settings
        /// </summary>
        [HttpGet("repositories/{repoName}/configuration")]
        public IActionResult GetRepositoryConfiguration(string repoName)
        {
            try
            {
                var repoLink = _knowledgeService.GetRepositoryLink(repoName);
                var configs = _knowledgeService.GetConfigurations();

                return Ok(new
                {
                    success = true,
                    repository = repoName,
                    repositoryInfo = new
                    {
                        name = repoLink.RepositoryName,
                        owner = repoLink.Owner,
                        gitHubUrl = repoLink.GitHubUrl,
                        gitLabUrl = repoLink.GitLabUrl,
                        defaultBranch = repoLink.DefaultBranch,
                        documentationUrl = repoLink.DocumentationUrl
                    },
                    deploymentEnvironments = repoLink.DeploymentUrls.Select(d => new
                    {
                        d.Environment,
                        d.Url,
                        d.Status,
                        d.BuildStatus,
                        d.LastDeployed
                    }),
                    configuration = configs.Where(c => c.Environment == "All" || c.Environment == "Production").ToList(),
                    message = $"Retrieved configuration for {repoName}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving repository configuration for {repoName}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
