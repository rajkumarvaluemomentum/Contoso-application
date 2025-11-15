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

