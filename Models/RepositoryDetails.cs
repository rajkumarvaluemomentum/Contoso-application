public class RepositoryDetails
{
    public string RepoName { get; set; }
    public string Description { get; set; }
    public string DefaultBranch { get; set; }
    public string HtmlUrl { get; set; }
    public string CloneUrl { get; set; }
}

public class BuildStatus
{
    public string Status { get; set; }
    public string Conclusion { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class DeploymentInfo
{
    public string EnvironmentName { get; set; }
    public string DeploymentUrl { get; set; }
}

public class ServiceHealth
{
    public string ServiceName { get; set; }
    public string Status { get; set; }
    public DateTime CheckedAt { get; set; }
}
