namespace Contoso_application.Models
{
    public class RepositoryDeploymentDetails
    {
        public string RepositoryName { get; set; }  // The name of the repository (e.g., Contoso-application)

        // Deployment URLs for multiple environments
        public string DevelopmentUrl { get; set; }  // Development environment URL
        public string UatUrl { get; set; }          // UAT environment URL
        public string StagingUrl { get; set; }      // Staging environment URL
        public string ProductionUrl { get; set; }   // Production environment URL
    }
}
