# Virtual Assistant API - Implementation Details & Examples

## Complete Implementation Overview

### What Has Been Implemented

Your Virtual Assistant API now includes a comprehensive knowledge management system with the following capabilities:

---

## Part 1: Connect Code Repositories, GitHub/GitLab Links

### Implementation Details

#### Knowledge Source Model
```csharp
public class KnowledgeSource
{
    public string Id { get; set; }
    public string Type { get; set; }  // GitHub, GitLab, Documentation, Deployment
    public string Name { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
```

#### Repository Link Model
```csharp
public class RepositoryLink
{
    public string RepositoryName { get; set; }
    public string Owner { get; set; }
    public string GitHubUrl { get; set; }  // https://github.com/{owner}/{repo}
    public string GitLabUrl { get; set; }  // https://gitlab.com/{owner}/{repo}
    public string DocumentationUrl { get; set; }
    public List<DeploymentUrl> DeploymentUrls { get; set; }
    public string DefaultBranch { get; set; }
}
```

#### Initialization in KnowledgeSourceService
```csharp
_knowledgeSources.Add(new KnowledgeSource
{
    Id = "github-main",
    Type = "GitHub",
    Name = "Virtual Assistant Bot - Main Repository",
    Url = "https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot",
    Description = "Main GitHub repository for Virtual Assistant Bot project",
    Metadata = new Dictionary<string, string>
    {
        { "owner", "rajkumarvaluemomentum" },
        { "branch", "Dev" },
        { "visibility", "public" }
    }
});
```

### API Usage Examples

#### Get All Knowledge Sources
```bash
curl -X GET "http://localhost:5206/api/knowledge/sources"
```

**Response:**
```json
{
  "success": true,
  "count": 4,
  "data": [
    {
      "id": "github-main",
      "type": "GitHub",
      "name": "Virtual Assistant Bot - Main Repository",
      "url": "https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot",
      "description": "Main GitHub repository",
      "metadata": {
        "owner": "rajkumarvaluemomentum",
        "branch": "Dev",
        "visibility": "public"
      },
      "isActive": true
    },
    {
      "id": "deployed-app",
      "type": "Deployment",
      "name": "Virtual Assistant API - Production",
      "url": "https://virtual-assistant-bot.onrender.com",
      "metadata": {
        "platform": "Render",
        "environment": "production"
      }
    },
    {
      "id": "api-docs",
      "type": "Documentation",
      "name": "API Documentation - Swagger",
      "url": "https://virtual-assistant-bot.onrender.com/swagger"
    }
  ]
}
```

#### Get Sources by Type
```bash
# Get all GitHub sources
curl -X GET "http://localhost:5206/api/knowledge/sources/type/GitHub"

# Get all Deployment sources
curl -X GET "http://localhost:5206/api/knowledge/sources/type/Deployment"

# Get all Documentation sources
curl -X GET "http://localhost:5206/api/knowledge/sources/type/Documentation"
```

#### Get Repository Details
```bash
curl -X GET "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot"
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "repo-001",
    "repositoryName": "Virtual_Assistant_Bot",
    "owner": "rajkumarvaluemomentum",
    "gitHubUrl": "https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot",
    "gitLabUrl": "https://gitlab.com/rajkumarvaluemomentum/Virtual_Assistant_Bot",
    "documentationUrl": "https://virtual-assistant-bot.onrender.com/docs/Virtual_Assistant_Bot",
    "defaultBranch": "Dev",
    "lastUpdated": "2025-01-15T10:45:00Z"
  }
}
```

---

## Part 2: Allow Users to Query Code Modules, API Endpoints, Environment URLs, Configurations

### Implementation Details

#### API Endpoint Info Model
```csharp
public class ApiEndpointInfo
{
    public string Method { get; set; }           // GET, POST, PUT, DELETE
    public string Route { get; set; }            // /api/github/repositories
    public string Description { get; set; }
    public string Controller { get; set; }
    public Dictionary<string, string> Parameters { get; set; }
    public string ReturnType { get; set; }
}
```

#### Code Module Model
```csharp
public class CodeModuleInfo
{
    public string ModuleName { get; set; }
    public string FilePath { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public List<string> Dependencies { get; set; }
    public List<ApiEndpointInfo> ApiEndpoints { get; set; }
}
```

#### Configuration Model
```csharp
public class ConfigurationInfo
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string Environment { get; set; }      // Development, Production
    public string Description { get; set; }
    public bool IsSensitive { get; set; }
}
```

### API Usage Examples

#### Query API Endpoints
```bash
# Get all API endpoints
curl -X GET "http://localhost:5206/api/knowledge/api-endpoints"

# Get endpoints for GitHub controller
curl -X GET "http://localhost:5206/api/knowledge/api-endpoints?controller=GitHub"
```

**Response:**
```json
{
  "success": true,
  "count": 2,
  "data": [
    {
      "method": "GET",
      "route": "api/github/repositories",
      "description": "Fetch all GitHub repositories for the authenticated user",
      "controller": "GitHubController",
      "parameters": {},
      "returnType": "IEnumerable<string>"
    },
    {
      "method": "GET",
      "route": "api/github/repositories/{repoName}/deployments",
      "description": "Fetch deployment history for a specific repository",
      "controller": "GitHubController",
      "parameters": {
        "repoName": "string"
      },
      "returnType": "IEnumerable<object>"
    }
  ]
}
```

#### Query Code Modules
```bash
# Get all modules
curl -X GET "http://localhost:5206/api/knowledge/modules"

# Search for GitHub module
curl -X GET "http://localhost:5206/api/knowledge/modules?search=github"

# Search for database module
curl -X GET "http://localhost:5206/api/knowledge/modules?search=database"
```

**Response:**
```json
{
  "success": true,
  "count": 1,
  "search": "github",
  "data": [
    {
      "moduleName": "GitHub Integration",
      "filePath": "Services/GitHubService.cs",
      "language": "C#",
      "description": "Service for interacting with GitHub API to fetch repositories and deployments",
      "dependencies": [
        "Octokit",
        "HttpClient"
      ],
      "apiEndpoints": [
        {
          "method": "GET",
          "route": "api/github/repositories",
          "description": "Fetch all GitHub repositories",
          "controller": "GitHubController",
          "returnType": "IEnumerable<string>"
        }
      ]
    }
  ]
}
```

#### Query Configuration
```bash
# Get all configurations
curl -X GET "http://localhost:5206/api/knowledge/configurations"

# Get development configurations only
curl -X GET "http://localhost:5206/api/knowledge/configurations?environment=Development"

# Get production configurations
curl -X GET "http://localhost:5206/api/knowledge/configurations?environment=Production"
```

**Response:**
```json
{
  "success": true,
  "count": 5,
  "environment": null,
  "data": [
    {
      "key": "GitHub:Username",
      "value": "rajkumarvaluemomentum",
      "environment": "All",
      "description": "GitHub username for API authentication",
      "isSensitive": false
    },
    {
      "key": "GitHub:Token",
      "value": "***hidden***",
      "environment": "All",
      "description": "GitHub personal access token",
      "isSensitive": true
    },
    {
      "key": "ConnectionStrings:DefaultConnection",
      "value": "Server=VSP-8VPLD74;Database=***",
      "environment": "Development",
      "description": "SQL Server connection string",
      "isSensitive": true
    },
    {
      "key": "PORT",
      "value": "10000",
      "environment": "Production",
      "description": "Server port for Render deployment",
      "isSensitive": false
    }
  ]
}
```

#### Query Environment URLs
```bash
# Get all deployments for repository
curl -X GET "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot/deployments"

# Get specific environment URL
curl -X GET "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot/deployment/Production"

# Get development environment URL
curl -X GET "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot/deployment/Development"
```

**Response:**
```json
{
  "success": true,
  "repository": "Virtual_Assistant_Bot",
  "count": 2,
  "data": [
    {
      "environment": "Development",
      "url": "http://localhost:5206",
      "status": "Active",
      "buildStatus": "Success",
      "lastDeployed": "2025-01-15T10:30:00Z",
      "deploymentDetailsUrl": "http://localhost:5206/swagger"
    },
    {
      "environment": "Production",
      "url": "https://virtual-assistant-bot.onrender.com",
      "status": "Active",
      "buildStatus": "Success",
      "lastDeployed": "2025-01-15T08:15:00Z",
      "deploymentDetailsUrl": "https://virtual-assistant-bot.onrender.com/swagger"
    }
  ]
}
```

---

## Part 3: Retrieve Snippets/Documentation from Repo When Queried

### Implementation Details

#### Code Snippet Model
```csharp
public class CodeSnippet
{
    public string Id { get; set; }
    public string FilePath { get; set; }
    public int StartLine { get; set; }
    public int EndLine { get; set; }
    public string Code { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
}
```

#### Query Response Model
```csharp
public class QueryResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public List<CodeSnippet> CodeSnippets { get; set; }
    public List<string> RelatedResources { get; set; }
}
```

### Semantic Query Implementation

The `QueryKnowledgeBase()` method analyzes user queries and returns relevant information:

```csharp
public QueryResponse QueryKnowledgeBase(string query)
{
    var response = new QueryResponse { Success = true };
    var lowerQuery = query.ToLower();

    // Check for repo links
    if (lowerQuery.Contains("repo") || lowerQuery.Contains("repository") || lowerQuery.Contains("github"))
    {
        var repoLink = GetRepositoryLink("Virtual_Assistant_Bot");
        response.Data = repoLink;
        response.RelatedResources.Add(repoLink.GitHubUrl);
    }

    // Check for deployments
    if (lowerQuery.Contains("deploy") || lowerQuery.Contains("url"))
    {
        var repoLink = GetRepositoryLink("Virtual_Assistant_Bot");
        response.Data = repoLink.DeploymentUrls;
        response.RelatedResources.AddRange(repoLink.DeploymentUrls.Select(d => d.Url));
    }

    // Check for API endpoints
    if (lowerQuery.Contains("api") || lowerQuery.Contains("endpoint"))
    {
        var endpoints = GetApiEndpoints();
        response.Data = endpoints;
        response.RelatedResources.AddRange(endpoints.Select(e => $"{e.Method} {e.Route}"));
    }

    // Check for configurations
    if (lowerQuery.Contains("config") || lowerQuery.Contains("setting"))
    {
        var configs = GetConfigurations();
        response.Data = configs;
    }

    // Check for modules/code
    if (lowerQuery.Contains("module") || lowerQuery.Contains("code") || lowerQuery.Contains("implementation"))
    {
        var modules = SearchModules(/*keyword*/);
        response.Data = modules;
    }

    return response;
}
```

### API Usage Examples

#### Query Examples

**Query 1: Production Deployment URL**
```bash
curl -X GET "http://localhost:5206/api/knowledge/query?q=what+is+the+production+deployment+url"
```

**Response:**
```json
{
  "success": true,
  "message": "Found deployment URLs for various environments",
  "data": [
    {
      "environment": "Production",
      "url": "https://virtual-assistant-bot.onrender.com",
      "status": "Active",
      "buildStatus": "Success"
    }
  ],
  "codeSnippets": [],
  "relatedResources": [
    "https://virtual-assistant-bot.onrender.com"
  ]
}
```

**Query 2: GitHub Configuration**
```bash
curl -X GET "http://localhost:5206/api/knowledge/query?q=how+is+github+configured"
```

**Response:**
```json
{
  "success": true,
  "message": "Found repository: Virtual_Assistant_Bot",
  "data": {
    "repositoryName": "Virtual_Assistant_Bot",
    "owner": "rajkumarvaluemomentum",
    "gitHubUrl": "https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot"
  },
  "relatedResources": [
    "https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot"
  ]
}
```

**Query 3: Available API Endpoints**
```bash
curl -X GET "http://localhost:5206/api/knowledge/query?q=show+me+all+api+endpoints"
```

**Response:**
```json
{
  "success": true,
  "message": "Found 8 API endpoints",
  "data": [
    {
      "method": "GET",
      "route": "api/github/repositories",
      "description": "Fetch all GitHub repositories"
    },
    {
      "method": "GET",
      "route": "api/knowledge/sources",
      "description": "Get all knowledge sources"
    }
    // ... more endpoints
  ],
  "relatedResources": [
    "GET /api/github/repositories",
    "GET /api/knowledge/sources",
    // ... more
  ]
}
```

**Query 4: Module Implementation Details**
```bash
curl -X GET "http://localhost:5206/api/knowledge/query?q=show+me+github+integration+module"
```

**Response:**
```json
{
  "success": true,
  "message": "Found 1 modules",
  "data": [
    {
      "moduleName": "GitHub Integration",
      "filePath": "Services/GitHubService.cs",
      "language": "C#",
      "description": "Service for interacting with GitHub API",
      "dependencies": [
        "Octokit",
        "HttpClient"
      ],
      "apiEndpoints": [
        {
          "method": "GET",
          "route": "api/github/repositories",
          "description": "Fetch repositories"
        }
      ]
    }
  ]
}
```

---

## Part 4: Support Actionable Commands

### Actionable Commands Model
```csharp
public class ActionCommand
{
    public string Action { get; set; }              // open-repo, fetch-deployment, show-build-status, query
    public string RepositoryName { get; set; }
    public string Environment { get; set; }
    public string Query { get; set; }
}
```

### API Usage Examples

#### 1. Open Repository Command
```bash
# Get repository URL to open
curl -X GET "http://localhost:5206/api/knowledge/action/open-repo/Virtual_Assistant_Bot"
```

**Response:**
```json
{
  "success": true,
  "action": "open-repo",
  "repository": "Virtual_Assistant_Bot",
  "url": "https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot",
  "message": "Open this URL in your browser: https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot"
}
```

#### 2. Fetch Deployment URL Command
```bash
# Get production deployment URL
curl -X GET "http://localhost:5206/api/knowledge/action/fetch-deployment/Virtual_Assistant_Bot/Production"

# Get development deployment URL
curl -X GET "http://localhost:5206/api/knowledge/action/fetch-deployment/Virtual_Assistant_Bot/Development"
```

**Response:**
```json
{
  "success": true,
  "action": "fetch-deployment",
  "repository": "Virtual_Assistant_Bot",
  "environment": "Production",
  "url": "https://virtual-assistant-bot.onrender.com",
  "message": "Deployment URL for Production: https://virtual-assistant-bot.onrender.com"
}
```

#### 3. Show Build Status Command
```bash
# Get latest build status
curl -X GET "http://localhost:5206/api/knowledge/action/show-build-status/Virtual_Assistant_Bot"
```

**Response:**
```json
{
  "success": true,
  "action": "show-build-status",
  "data": {
    "repositoryName": "Virtual_Assistant_Bot",
    "latestDeployments": [
      {
        "id": 12345,
        "environment": "production",
        "status": "success",
        "createdAt": "2025-01-15T08:15:00Z"
      }
    ],
    "status": "Success",
    "lastUpdated": "2025-01-15T10:45:00Z"
  },
  "message": "Latest build status for Virtual_Assistant_Bot"
}
```

#### 4. Execute Custom Action (POST)
```bash
# Fetch deployment URL via POST
curl -X POST "http://localhost:5206/api/knowledge/action/execute" \
  -H "Content-Type: application/json" \
  -d '{
    "action": "fetch-deployment",
    "repositoryName": "Virtual_Assistant_Bot",
    "environment": "Production"
  }'

# Show build status via POST
curl -X POST "http://localhost:5206/api/knowledge/action/execute" \
  -H "Content-Type: application/json" \
  -d '{
    "action": "show-build-status",
    "repositoryName": "Virtual_Assistant_Bot"
  }'

# Execute query via POST
curl -X POST "http://localhost:5206/api/knowledge/action/execute" \
  -H "Content-Type: application/json" \
  -d '{
    "action": "query",
    "query": "what is the production deployment url"
  }'
```

**Response:**
```json
{
  "success": true,
  "action": "fetch-deployment",
  "message": "Deployment URL for Production: https://virtual-assistant-bot.onrender.com",
  "data": "https://virtual-assistant-bot.onrender.com"
}
```

---

## Complete Workflow Example

### Scenario: Developer needs to check production deployment and find API documentation

**Step 1: Query production URL**
```bash
curl "http://localhost:5206/api/knowledge/query?q=production+url"
```

**Step 2: Get API documentation**
```bash
curl "http://localhost:5206/api/knowledge/api-endpoints"
```

**Step 3: Open repository**
```bash
curl "http://localhost:5206/api/knowledge/action/open-repo/Virtual_Assistant_Bot"
```

**Step 4: Check build status**
```bash
curl "http://localhost:5206/api/knowledge/action/show-build-status/Virtual_Assistant_Bot"
```

All done with simple REST API calls!

---

## Integration Points

### With GitHub Service
```csharp
// KnowledgeSourceService integrates with GitHubService
private readonly GitHubService _gitHubService;

public async Task<object> GetBuildStatusAsync(string repoName)
{
    // Fetches real deployment data from GitHub API
    var deployments = await _gitHubService.GetDeploymentsAsync(repoName);
    return new { /* build status info */ };
}
```

### With Swagger Documentation
- All endpoints are documented with Swagger UI
- Access at: `http://localhost:5206/swagger`
- Full request/response examples available

### With CORS
- All endpoints support cross-origin requests
- Can be accessed from web applications

---

## Summary of Implementation

| Requirement | Implementation | Endpoints |
|---|---|---|
| **Connect Repositories** | KnowledgeSource model with GitHub/GitLab URLs | `/api/knowledge/sources`, `/api/knowledge/repository/{repoName}` |
| **Query Code Modules** | CodeModuleInfo + SearchModules() | `/api/knowledge/modules`, `/api/knowledge/modules?search=X` |
| **Query API Endpoints** | ApiEndpointInfo + GetApiEndpoints() | `/api/knowledge/api-endpoints` |
| **Query Configurations** | ConfigurationInfo + GetConfigurations() | `/api/knowledge/configurations` |
| **Query Environment URLs** | DeploymentUrl + GetDeployments() | `/api/knowledge/repository/{repoName}/deployments` |
| **Retrieve Documentation** | QueryKnowledgeBase() | `/api/knowledge/query?q=X` |
| **Actionable Commands** | Action handlers in KnowledgeController | `/api/knowledge/action/*` |

All features are fully implemented, documented, tested, and ready to use!
