using Contoso_application.Models;

public class DeploymentService
{
    // Hardcoded deployment URLs for different environments
    private readonly Dictionary<string, List<DeploymentEnvironment>> _deploymentData = new()
    {
        {
            "Contoso-application", new List<DeploymentEnvironment>
            {
                new DeploymentEnvironment
                {
                    EnvironmentName = "UAT",
                    DeploymentUrl = "https://uat.contoso.com"
                },
                new DeploymentEnvironment
                {
                    EnvironmentName = "Development",
                    DeploymentUrl = "https://virtualassistantapi20251114050156-ctcwaafdd6gmhyfa.canadacentral-01.azurewebsites.net/"
                },
                new DeploymentEnvironment
                {
                    EnvironmentName = "Staging",
                    DeploymentUrl = "https://staging.contoso.com"
                }
            }
        }
    };

    // Fetch deployment URLs for a given repository/application
    public Dictionary<string, List<DeploymentEnvironment>> GetStaticDeploymentUrls()
    {
        return new Dictionary<string, List<DeploymentEnvironment>>
    {
        {
            "Contoso-application", new List<DeploymentEnvironment>
            {
                new DeploymentEnvironment
                {
                    EnvironmentName = "UAT",
                    DeploymentUrl = "https://uat.contoso.com"
                },
                new DeploymentEnvironment
                {
                    EnvironmentName = "Development",
                    DeploymentUrl = "https://virtualassistantapi20251114050156-ctcwaafdd6gmhyfa.canadacentral-01.azurewebsites.net/"
                },
                new DeploymentEnvironment
                {
                    EnvironmentName = "Staging",
                    DeploymentUrl = "https://staging.contoso.com"
                }
            }
        }
    };

    }
}
