using Contoso_application.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.Reflection.Metadata;
using VirtualAssistant.API.Models;
using VirtualAssistant.API.Services;
using static VirtualAssistant.API.Services.GitHubService;

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
        private readonly ILogger<GitHubController> _logger;
        private readonly DeploymentService _deploymentService;

        public GitHubController(GitHubService gitHubService, ILogger<GitHubController> logger, DeploymentService deploymentService)
        {
            _gitHubService = gitHubService;
            _logger = logger;
            _deploymentService = deploymentService;
        }

        #region Get all repositories from github
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
        #endregion

        #region GetRepositoryLink by repository name
        // Endpoint to fetch a specific repository link by passing the repository name
        // Endpoint to fetch a specific repository link by passing the repository name
        [HttpGet("GetRepositoryLink/{repoName}")]
        public async Task<IActionResult> GetRepositoryLink(string repoName)
        {
            if (string.IsNullOrWhiteSpace(repoName))
                return BadRequest("Repository name cannot be null or empty.");

            var repoResponse = await _gitHubService.GetRepositoryLinkAsync(repoName);

            if (repoResponse == null || string.IsNullOrEmpty(repoResponse.Result))
                return NotFound($"Repository '{repoName}' not found on GitHub.");

            // Return JSON with repo name & link
            return Ok(new
            {
                RepositoryName = repoName,
                RepositoryLink = repoResponse.Result
            });
        }


        #endregion



        //GET /api/github/query?applicationName=Contoso-application&query=microservices
        /// <summary>
        /// For Contoso application I need the list of modules.”
        //“For Contoso application I need the module that handles document upload.”
        //“For Contoso application I need the microservices list
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getModulesByRepoName")]
        public async Task<IActionResult> getModulesByRepoName([FromQuery] string applicationName, [FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
                return BadRequest("Application name cannot be empty.");

            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query cannot be empty.");

            var data = await _gitHubService.GetApplicationStructure(applicationName);

            if (data == null)
                return NotFound($"Application '{applicationName}' not found in GitHub.");

            query = query.ToLower();

            if (query.Contains("modules"))
                return Ok(data.Modules);

            if (query.Contains("document"))
                return Ok(data.DocumentUploadModule);

            if (query.Contains("microservices"))
                return Ok(data.Microservices);

            return BadRequest("Unknown query. Try: modules, document upload, microservices.");
        }


        /// <summary>
        //GET /api/github-endpoints/actions? repoName = Contoso - Application & controllerName =GithubController
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetApiEndpointsByreponame")]
        public async Task<IActionResult> GetApiEndpointsByreponame(
        [FromQuery] string repoName,
        [FromQuery] string controllerName)
        {
            if (string.IsNullOrEmpty(repoName) || string.IsNullOrEmpty(controllerName))
                return BadRequest("repoName and controllerName are required.");

            var result = await _gitHubService.GetEndpoints(repoName, controllerName);

            if (result.Count == 0)
                return NotFound("No controller or action methods found.");

            return Ok(result);
        }


        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings([FromQuery] string repoName, [FromQuery] string query)
        {
            if (string.IsNullOrEmpty(repoName) || string.IsNullOrEmpty(query))
                return BadRequest("repoName and query are required.");

            AppConfiguration? config;

            query = query.ToLower();

            if (query.Contains("prod"))
                config = await _gitHubService.GetConfiguration(repoName, "prod");
            else if (query.Contains("uat"))
                config = await _gitHubService.GetConfiguration(repoName, "uat");
            else
                config = await _gitHubService.GetConfiguration(repoName, "default");

            if (config == null)
                return NotFound("Configuration file not found.");

            // Optionally filter API base URL
            if (query.Contains("api base url"))
            {
                var baseUrl = config.Settings.FirstOrDefault(kv => kv.Key.ToLower().Contains("baseurl")).Value;
                return Ok(new { ApiBaseUrl = baseUrl });
            }

            return Ok(config.Settings);
        }

        [HttpGet("GetConfigFiles")]
        public async Task<IActionResult> GetConfigFiles([FromQuery] string repoName)
        {
            if (string.IsNullOrEmpty(repoName))
                return BadRequest("repoName is required.");

            var files = await _gitHubService.GetConfigFiles(repoName);

            if (!files.Any())
                return NotFound("No configuration files found in the repository.");

            return Ok(files);
        }


        //GET /api/github-code/snippet?repoName=Contoso-App&query=document upload

        [HttpGet("snippet")]
        public async Task<IActionResult> GetSnippet([FromQuery] string repoName, [FromQuery] string query)
        {
            if (string.IsNullOrEmpty(repoName) || string.IsNullOrEmpty(query))
                return BadRequest("repoName and query are required.");

            // Map natural-language queries to keyword
            string keyword = query.ToLower() switch
            {
                var s when s.Contains("document upload") => "Upload",
                var s when s.Contains("quote api") => "QuoteController",
                var s when s.Contains("sample request payload") => "SampleRequest",
                _ => query
            };

            var snippets = await _gitHubService.SearchCodeSnippets(repoName, keyword);

            if (!snippets.Any())
                return NotFound("No code snippet found matching the query.");

            return Ok(snippets);
        }
        // GET /api/contoso/repo-details?repoName=Contoso-App

        [HttpGet("repo-details")]
        public async Task<IActionResult> GetRepoDetails([FromQuery] string repoName)
        {
            var details = await _gitHubService.GetRepoDetails(repoName);
            if (details == null) return NotFound("Repository not found");
            return Ok(details);
        }
        // GET /api/contoso/latest-build?repoName=Contoso-App

        [HttpGet("latest-build")]
        public async Task<IActionResult> GetLatestBuild([FromQuery] string repoName)
        {
            var build = await _gitHubService.GetLatestBuildStatus(repoName);
            if (build == null) return NotFound("Build info not found");
            return Ok(build);
        }

        // GET /api/contoso/deployment-url?repoName=Contoso-App&environment=UAT

        [HttpGet("deployment-url")]
        public IActionResult GetDeploymentUrl([FromQuery] string repoName, [FromQuery] string environment)
        {
            var deployment = _gitHubService.GetDeploymentUrl(repoName, environment);
            if (deployment == null) return NotFound("Deployment URL not found");
            return Ok(deployment);
        }
        // GET /api/contoso/service-health?serviceName=QuoteService

        [HttpGet("service-health")]
        public IActionResult GetServiceHealth([FromQuery] string serviceName)
        {
            var health = _gitHubService.GetServiceHealth(serviceName);
            return Ok(health);
        }

        /// <summary>
        /// Get API endpoints information
        /// </summary>
        [HttpGet("api-endpoints")]
        public IActionResult GetApiEndpoints([FromQuery] string controller = null)
        {
            try
            {
                var endpoints = _gitHubService.GetApiEndpoints(controller);
                return Ok(new
                {
                    success = true,
                    count = endpoints.Count,
                    controller = controller,
                    data = endpoints
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving API endpoints");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get Deployment URL by Repo & Environment
        /// Example: GET /api/deployment/url?repoName=Contoso-application&envName=dev
        /// </summary>
        [HttpGet("GetDeploymentUrlByreponadevn")]
        public IActionResult GetDeploymentUrlByreponadevn([FromQuery] string repoName, [FromQuery] string envName)
        {
            if (string.IsNullOrWhiteSpace(repoName) || string.IsNullOrWhiteSpace(envName))
                return BadRequest("repoName and envName are required.");

            var url = _deploymentService.GetDeploymentUrl(repoName, envName);

            if (url == null)
                return NotFound("No matching repository or environment found.");

            return Ok(new { deploymentUrl = url });  // ✔ JSON response
        }

    }
}
  
