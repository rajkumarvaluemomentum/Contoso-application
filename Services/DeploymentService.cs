public class DeploymentService
{
    private readonly Dictionary<string, List<DeploymentEnvironment>> _deploymentUrls;

    public DeploymentService()
    {
        _deploymentUrls = new Dictionary<string, List<DeploymentEnvironment>>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "Contoso-application", new List<DeploymentEnvironment>
                {
                    new DeploymentEnvironment { EnvironmentName = "UAT", DeploymentUrl = "https://uat.contoso.com" },
                    new DeploymentEnvironment { EnvironmentName = "Development", DeploymentUrl = "https://virtualassistantapi20251114050156-ctcwaafdd6gmhyfa.canadacentral-01.azurewebsites.net/" },
                    new DeploymentEnvironment { EnvironmentName = "Staging", DeploymentUrl = "https://staging.contoso.com" }
                }
            }
        };
    }

    public string GetDeploymentUrl(string repoName, string envName)
    {
        if (!_deploymentUrls.TryGetValue(repoName, out var environments))
            return null;

        var match = environments.FirstOrDefault(e =>
            e.EnvironmentName.Contains(envName, StringComparison.OrdinalIgnoreCase));

        return match?.DeploymentUrl;
    }

}
