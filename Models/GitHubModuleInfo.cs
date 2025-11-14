public class GitHubModuleInfo
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Url { get; set; }
}

public class ApplicationGitHubResponse
{
    public string ApplicationName { get; set; }
    public List<GitHubModuleInfo> Modules { get; set; }
    public GitHubModuleInfo DocumentUploadModule { get; set; }
    public List<GitHubModuleInfo> Microservices { get; set; }
}
