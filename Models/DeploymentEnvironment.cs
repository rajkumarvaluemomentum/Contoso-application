namespace Contoso_application.Models
{
    public class DeploymentEnvironment
    {
        public string EnvironmentName { get; set; }  // UAT, Production, Staging
        public string DeploymentUrl { get; set; }    // The actual deployment URL
    }
}
