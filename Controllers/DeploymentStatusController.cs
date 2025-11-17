using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/deployment-status")]
public class DeploymentStatusController : ControllerBase
{
    [HttpGet("{env}")]
    public IActionResult GetStatus(string env)
    {
        var status = new
        {
            Environment = env,
            LastDeployed = DateTime.UtcNow,
            Status = "Healthy"
        };

        return Ok(status);
    }
}
