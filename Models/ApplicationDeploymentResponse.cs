namespace Contoso_application.Models
{
    public class ApplicationDeploymentResponse
    {
        public string ApplicationName { get; set; }
        public List<DeploymentEnvironment> Environments { get; set; }
    }
}
