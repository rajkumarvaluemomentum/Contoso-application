using Microsoft.AspNetCore.Mvc;
using VirtualAssistant.API.Models;
using VirtualAssistant.API.Services;

namespace VirtualAssistant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KnowledgeController : ControllerBase
    {
        private readonly KnowledgeSourceService _knowledgeService;
        private readonly ILogger<KnowledgeController> _logger;

        public KnowledgeController(KnowledgeSourceService knowledgeService, ILogger<KnowledgeController> logger)
        {
            _knowledgeService = knowledgeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all knowledge sources
        /// </summary>
        [HttpGet("sources")]
        public IActionResult GetAllKnowledgeSources()
        {
            try
            {
                var sources = _knowledgeService.GetAllKnowledgeSources();
                return Ok(new
                {
                    success = true,
                    count = sources.Count(),
                    data = sources
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving knowledge sources");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get knowledge sources by type (GitHub, GitLab, Documentation, Deployment)
        /// </summary>
        [HttpGet("sources/type/{type}")]
        public IActionResult GetKnowledgeSourcesByType(string type)
        {
            try
            {
                var sources = _knowledgeService.GetKnowledgeSourcesByType(type);
                return Ok(new
                {
                    success = true,
                    type = type,
                    count = sources.Count(),
                    data = sources
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving knowledge sources of type {type}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Query the knowledge base (smart search)
        /// </summary>
        [HttpGet("query")]
        public IActionResult QueryKnowledgeBase([FromQuery] string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    return BadRequest(new { error = "Query parameter 'q' is required" });
                }

                var response = _knowledgeService.QueryKnowledgeBase(q);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying knowledge base");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get repository information with links and deployments
        /// </summary>
        [HttpGet("repository/{repoName}")]
        public IActionResult GetRepositoryInfo(string repoName)
        {
            try
            {
                var repoLink = _knowledgeService.GetRepositoryLink(repoName);
                return Ok(new
                {
                    success = true,
                    data = repoLink
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving repository info for {repoName}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get all deployments for a repository
        /// </summary>
        [HttpGet("repository/{repoName}/deployments")]
        public IActionResult GetDeployments(string repoName)
        {
            try
            {
                var deployments = _knowledgeService.GetAllDeployments(repoName);
                return Ok(new
                {
                    success = true,
                    repository = repoName,
                    count = deployments.Count,
                    data = deployments
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving deployments for {repoName}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get deployment URL for a specific environment
        /// </summary>
        [HttpGet("repository/{repoName}/deployment/{environment}")]
        public IActionResult GetDeploymentUrl(string repoName, string environment)
        {
            try
            {
                var url = _knowledgeService.GetDeploymentUrl(repoName, environment);
                return Ok(new
                {
                    success = true,
                    repository = repoName,
                    environment = environment,
                    url = url
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving deployment URL for {repoName} - {environment}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get API endpoints information
        /// </summary>
        [HttpGet("api-endpoints")]
        public IActionResult GetApiEndpoints([FromQuery] string controller = null)
        {
            try
            {
                var endpoints = _knowledgeService.GetApiEndpoints(controller);
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
        /// Get code modules information
        /// </summary>
        [HttpGet("modules")]
        public IActionResult GetModules([FromQuery] string search = null)
        {
            try
            {
                List<CodeModuleInfo> modules;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    modules = _knowledgeService.SearchModules(search);
                }
                else
                {
                    modules = _knowledgeService.SearchModules("");
                }

                return Ok(new
                {
                    success = true,
                    count = modules.Count,
                    search = search,
                    data = modules
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving modules");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get configuration information
        /// </summary>
        [HttpGet("configurations")]
        public IActionResult GetConfigurations([FromQuery] string environment = null)
        {
            try
            {
                var configs = _knowledgeService.GetConfigurations(environment);
                return Ok(new
                {
                    success = true,
                    count = configs.Count,
                    environment = environment,
                    data = configs
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving configurations");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get build status for a repository
        /// </summary>
        [HttpGet("repository/{repoName}/build-status")]
        public async Task<IActionResult> GetBuildStatus(string repoName)
        {
            try
            {
                var status = await _knowledgeService.GetBuildStatusAsync(repoName);
                return Ok(new
                {
                    success = true,
                    data = status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving build status for {repoName}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Open repository action - returns the GitHub URL
        /// </summary>
        [HttpGet("action/open-repo/{repoName}")]
        public IActionResult OpenRepository(string repoName)
        {
            try
            {
                var url = _knowledgeService.GetRepositoryOpenUrl(repoName);
                return Ok(new
                {
                    success = true,
                    action = "open-repo",
                    repository = repoName,
                    url = url,
                    message = $"Open this URL in your browser: {url}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error opening repository {repoName}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Fetch deployment URL action
        /// </summary>
        [HttpGet("action/fetch-deployment/{repoName}/{environment}")]
        public IActionResult FetchDeploymentUrl(string repoName, string environment)
        {
            try
            {
                var url = _knowledgeService.GetDeploymentUrl(repoName, environment);
                return Ok(new
                {
                    success = true,
                    action = "fetch-deployment",
                    repository = repoName,
                    environment = environment,
                    url = url,
                    message = $"Deployment URL for {environment}: {url}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching deployment URL for {repoName} - {environment}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Show latest build status action
        /// </summary>
        [HttpGet("action/show-build-status/{repoName}")]
        public async Task<IActionResult> ShowBuildStatus(string repoName)
        {
            try
            {
                var status = await _knowledgeService.GetBuildStatusAsync(repoName);
                return Ok(new
                {
                    success = true,
                    action = "show-build-status",
                    data = status,
                    message = $"Latest build status for {repoName}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error showing build status for {repoName}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Multi-action command handler
        /// </summary>
        [HttpPost("action/execute")]
        public async Task<IActionResult> ExecuteAction([FromBody] ActionCommand command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command?.Action))
                {
                    return BadRequest(new { error = "Action is required" });
                }

                object result = null;
                string message = "";

                switch (command.Action.ToLower())
                {
                    case "open-repo":
                        result = _knowledgeService.GetRepositoryOpenUrl(command.RepositoryName);
                        message = $"Repository URL: {result}";
                        break;

                    case "fetch-deployment":
                        result = _knowledgeService.GetDeploymentUrl(command.RepositoryName, command.Environment);
                        message = $"Deployment URL for {command.Environment}: {result}";
                        break;

                    case "show-build-status":
                        result = await _knowledgeService.GetBuildStatusAsync(command.RepositoryName);
                        message = "Build status retrieved";
                        break;

                    case "query":
                        result = _knowledgeService.QueryKnowledgeBase(command.Query);
                        message = "Query executed";
                        break;

                    default:
                        return BadRequest(new { error = $"Unknown action: {command.Action}" });
                }

                return Ok(new
                {
                    success = true,
                    action = command.Action,
                    message = message,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing action");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Action command model for batch operations
    /// </summary>
    public class ActionCommand
    {
        public string Action { get; set; }
        public string RepositoryName { get; set; }
        public string Environment { get; set; }
        public string Query { get; set; }
    }
}
