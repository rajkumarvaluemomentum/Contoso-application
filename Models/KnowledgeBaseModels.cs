namespace VirtualAssistant.API.Models
{
    /// <summary>
    /// Repository configuration and metadata
    /// </summary>
    public class RepositoryConfiguration
    {
        public string RepositoryName { get; set; }
        public string Owner { get; set; }
        public string Platform { get; set; } // GitHub, GitLab
        public string DefaultBranch { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Links { get; set; } = new();
        public List<EnvironmentConfig> Environments { get; set; } = new();
        public List<string> Technologies { get; set; } = new();
        public DateTime LastUpdated { get; set; }
    }

    public class EnvironmentConfig
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string ApiDocumentationUrl { get; set; }
        public Dictionary<string, string> Variables { get; set; } = new();
        public string BuildStatus { get; set; }
        public DateTime LastDeployed { get; set; }
    }

    /// <summary>
    /// Knowledge base summary and statistics
    /// </summary>
    public class KnowledgeBaseSummary
    {
        public int TotalRepositories { get; set; }
        public int TotalApiEndpoints { get; set; }
        public int TotalModules { get; set; }
        public int TotalConfigurations { get; set; }
        public List<string> RepositoryNames { get; set; } = new();
        public List<string> AvailableEnvironments { get; set; } = new();
        public Dictionary<string, int> SourceTypeCount { get; set; } = new();
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// Search result for knowledge base queries
    /// </summary>
    public class SearchResult
    {
        public string Id { get; set; }
        public string Type { get; set; } // Repository, Module, Endpoint, Configuration, Documentation
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public double Relevance { get; set; } // 0.0 to 1.0
        public List<string> Tags { get; set; } = new();
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// API documentation for a module
    /// </summary>
    public class ModuleDocumentation
    {
        public string ModuleName { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string Purpose { get; set; }
        public List<string> Dependencies { get; set; } = new();
        public List<ApiEndpointInfo> Endpoints { get; set; } = new();
        public List<CodeExample> Examples { get; set; } = new();
        public List<string> AuthorNotes { get; set; } = new();
    }

    public class CodeExample
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
