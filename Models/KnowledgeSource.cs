namespace VirtualAssistant.API.Models
{
    public class KnowledgeSource
    {
        public string Id { get; set; }
        public string Type { get; set; } // "GitHub", "GitLab", "Documentation", "Deployment"
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class RepositoryLink
    {
        public string Id { get; set; }
        public string RepositoryName { get; set; }
        public string Owner { get; set; }
        public string GitHubUrl { get; set; }
        public string GitLabUrl { get; set; }
        public string DocumentationUrl { get; set; }
        public List<DeploymentUrl> DeploymentUrls { get; set; } = new();
        public string DefaultBranch { get; set; } = "main";
        public DateTime LastUpdated { get; set; }
    }

    public class DeploymentUrl
    {
        public string Environment { get; set; } // "Development", "Staging", "Production"
        public string Url { get; set; }
        public string Status { get; set; } // "Active", "Inactive", "Maintenance"
        public string BuildStatus { get; set; } // "Success", "Failed", "InProgress"
        public DateTime LastDeployed { get; set; }
        public string DeploymentDetailsUrl { get; set; } // Link to deployment details
    }

    public class CodeModuleInfo
    {
        public string ModuleName { get; set; }
        public string FilePath { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public List<string> Dependencies { get; set; } = new();
        public List<ApiEndpointInfo> ApiEndpoints { get; set; } = new();
    }

    public class ApiEndpointInfo
    {
        public string Method { get; set; } // GET, POST, PUT, DELETE
        public string Route { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
        public string ReturnType { get; set; }
    }

    public class ConfigurationInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Environment { get; set; } // Development, Staging, Production
        public string Description { get; set; }
        public bool IsSensitive { get; set; }
    }

    public class CodeSnippet
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; } = new();
    }

    public class QueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<CodeSnippet> CodeSnippets { get; set; } = new();
        public List<string> RelatedResources { get; set; } = new();
    }
}
