# Virtual Assistant API - Knowledge Source Integration

## Overview

This document describes the comprehensive knowledge source integration system that allows the Virtual Assistant API to:

1. **Connect Code Repositories** - GitHub/GitLab links and repository metadata
2. **Query Code Modules & Endpoints** - Access detailed information about API endpoints and code modules
3. **Environment & Configuration Management** - Retrieve deployment URLs and configuration details
4. **Actionable Commands** - Execute commands like opening repositories and fetching build status

---

## Architecture

### Components

#### 1. **KnowledgeSourceService** (`Services/KnowledgeSourceService.cs`)
Core service managing all knowledge sources, repositories, modules, and configurations.

**Key Methods:**
- `GetAllKnowledgeSources()` - Retrieve all active knowledge sources
- `GetKnowledgeSourcesByType(type)` - Filter sources by type (GitHub, GitLab, Documentation, Deployment)
- `GetRepositoryLink(repositoryName)` - Get complete repository information with deployment URLs
- `SearchModules(keyword)` - Search code modules by keyword
- `GetApiEndpoints(controllerName)` - Retrieve API endpoint information
- `QueryKnowledgeBase(query)` - Smart semantic search across knowledge base
- `GetBuildStatusAsync(repoName)` - Fetch latest build status from GitHub

#### 2. **KnowledgeController** (`Controllers/KnowledgeController.cs`)
REST API endpoints exposing knowledge base functionality.

#### 3. **Models** (`Models/KnowledgeSource.cs`, `Models/KnowledgeBaseModels.cs`)
Data structures for knowledge sources, repositories, deployments, modules, and configurations.

---

## API Endpoints

### Knowledge Sources

#### Get All Knowledge Sources
```
GET /api/knowledge/sources
```
Returns all active knowledge sources including repositories, documentation, and deployment URLs.

**Response Example:**
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
      "description": "Main GitHub repository for Virtual Assistant Bot project",
      "metadata": {
        "owner": "rajkumarvaluemomentum",
        "branch": "Dev",
        "visibility": "public"
      }
    }
  ]
}
```

#### Get Sources by Type
```
GET /api/knowledge/sources/type/{type}
```
Filter knowledge sources by type: `GitHub`, `GitLab`, `Documentation`, `Deployment`

**Example:**
```
GET /api/knowledge/sources/type/GitHub
```

---

### Repository Information

#### Get Repository Details
```
GET /api/knowledge/repository/{repoName}
```
Retrieve complete repository information including all deployment URLs and branches.

**Response Example:**
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
    "deploymentUrls": [
      {
        "environment": "Development",
        "url": "http://localhost:5206",
        "status": "Active",
        "buildStatus": "Success",
        "lastDeployed": "2025-01-15T10:30:00Z"
      },
      {
        "environment": "Production",
        "url": "https://virtual-assistant-bot.onrender.com",
        "status": "Active",
        "buildStatus": "Success",
        "lastDeployed": "2025-01-15T08:15:00Z"
      }
    ]
  }
}
```

#### Get All Deployments
```
GET /api/knowledge/repository/{repoName}/deployments
```
Retrieve all deployment environments and URLs for a repository.

#### Get Specific Deployment URL
```
GET /api/knowledge/repository/{repoName}/deployment/{environment}
```
Get deployment URL for a specific environment (Development, Staging, Production).

**Example:**
```
GET /api/knowledge/repository/Virtual_Assistant_Bot/deployment/Production
```

**Response:**
```json
{
  "success": true,
  "repository": "Virtual_Assistant_Bot",
  "environment": "Production",
  "url": "https://virtual-assistant-bot.onrender.com"
}
```

---

### API Endpoints Information

#### Get All API Endpoints
```
GET /api/knowledge/api-endpoints
```
Retrieve information about all API endpoints in the application.

#### Get Endpoints for Specific Controller
```
GET /api/knowledge/api-endpoints?controller={controllerName}
```

**Response Example:**
```json
{
  "success": true,
  "count": 2,
  "controller": null,
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

---

### Code Modules

#### Get All Modules
```
GET /api/knowledge/modules
```
Retrieve all code modules with their descriptions and dependencies.

#### Search Modules
```
GET /api/knowledge/modules?search={keyword}
```
Search for modules by keyword.

**Response Example:**
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
          "description": "Fetch all GitHub repositories for the authenticated user",
          "controller": "GitHubController",
          "returnType": "IEnumerable<string>"
        }
      ]
    }
  ]
}
```

---

### Configurations

#### Get All Configurations
```
GET /api/knowledge/configurations
```
Retrieve application configuration information (sensitive values are masked).

#### Get Configurations by Environment
```
GET /api/knowledge/configurations?environment={environment}
```
Filter configurations by environment (Development, Staging, Production).

**Response Example:**
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
    }
  ]
}
```

---

### Build Status

#### Get Build Status
```
GET /api/knowledge/repository/{repoName}/build-status
```
Retrieve the latest build status for a repository from GitHub deployments.

**Response Example:**
```json
{
  "success": true,
  "data": {
    "repositoryName": "Virtual_Assistant_Bot",
    "latestDeployments": [...],
    "status": "Success",
    "lastUpdated": "2025-01-15T10:45:00Z"
  }
}
```

---

### Query & Search

#### Semantic Query
```
GET /api/knowledge/query?q={query}
```
Perform semantic search across the knowledge base. The query system understands keywords like:
- "repo", "repository", "github" → Returns repository information
- "deploy", "url", "environment" → Returns deployment URLs
- "api", "endpoint" → Returns API endpoint information
- "config", "setting" → Returns configuration information
- "module", "code", "implementation" → Returns code module details

**Examples:**
```
GET /api/knowledge/query?q=show+me+the+production+deployment+url
GET /api/knowledge/query?q=what+API+endpoints+are+available
GET /api/knowledge/query?q=how+is+the+GitHub+API+configured
```

**Response Example:**
```json
{
  "success": true,
  "message": "Found deployment URLs for various environments",
  "data": [...],
  "codeSnippets": [],
  "relatedResources": [
    "http://localhost:5206",
    "https://virtual-assistant-bot.onrender.com"
  ]
}
```

---

### Actionable Commands

#### Open Repository
```
GET /api/knowledge/action/open-repo/{repoName}
```
Get the GitHub repository URL to open in browser.

#### Fetch Deployment URL
```
GET /api/knowledge/action/fetch-deployment/{repoName}/{environment}
```
Get deployment URL for a specific environment.

#### Show Build Status
```
GET /api/knowledge/action/show-build-status/{repoName}
```
Retrieve the latest build status.

#### Execute Action (POST)
```
POST /api/knowledge/action/execute
```
Execute a custom action with parameters.

**Request Body Example:**
```json
{
  "action": "fetch-deployment",
  "repositoryName": "Virtual_Assistant_Bot",
  "environment": "Production"
}
```

**Supported Actions:**
- `open-repo` - Opens repository URL
- `fetch-deployment` - Gets deployment URL
- `show-build-status` - Shows build status
- `query` - Executes a knowledge base query

---

## Data Models

### KnowledgeSource
```csharp
public class KnowledgeSource
{
    public string Id { get; set; }
    public string Type { get; set; } // GitHub, GitLab, Documentation, Deployment
    public string Name { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
```

### RepositoryLink
```csharp
public class RepositoryLink
{
    public string Id { get; set; }
    public string RepositoryName { get; set; }
    public string Owner { get; set; }
    public string GitHubUrl { get; set; }
    public string GitLabUrl { get; set; }
    public string DocumentationUrl { get; set; }
    public List<DeploymentUrl> DeploymentUrls { get; set; }
    public string DefaultBranch { get; set; }
    public DateTime LastUpdated { get; set; }
}
```

### DeploymentUrl
```csharp
public class DeploymentUrl
{
    public string Environment { get; set; } // Development, Staging, Production
    public string Url { get; set; }
    public string Status { get; set; } // Active, Inactive, Maintenance
    public string BuildStatus { get; set; } // Success, Failed, InProgress
    public DateTime LastDeployed { get; set; }
    public string DeploymentDetailsUrl { get; set; }
}
```

### CodeModuleInfo
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

### ApiEndpointInfo
```csharp
public class ApiEndpointInfo
{
    public string Method { get; set; } // GET, POST, PUT, DELETE
    public string Route { get; set; }
    public string Description { get; set; }
    public string Controller { get; set; }
    public Dictionary<string, string> Parameters { get; set; }
    public string ReturnType { get; set; }
}
```

---

## Use Cases

### 1. Query Repository Information
```bash
# Get all information about a repository
GET /api/knowledge/repository/Virtual_Assistant_Bot

# Get specific deployment URL
GET /api/knowledge/repository/Virtual_Assistant_Bot/deployment/Production
```

### 2. Find API Documentation
```bash
# Get all API endpoints
GET /api/knowledge/api-endpoints

# Get GitHub controller endpoints
GET /api/knowledge/api-endpoints?controller=GitHub
```

### 3. Search Code Modules
```bash
# Search for GitHub integration module
GET /api/knowledge/modules?search=github

# Search for database-related modules
GET /api/knowledge/modules?search=database
```

### 4. Query Knowledge Base
```bash
# Ask for production deployment URL
GET /api/knowledge/query?q=what+is+the+production+URL

# Ask for available API endpoints
GET /api/knowledge/query?q=show+me+all+API+endpoints

# Ask for configuration details
GET /api/knowledge/query?q=how+is+GitHub+configured
```

### 5. Execute Actions
```bash
# Open repository
GET /api/knowledge/action/open-repo/Virtual_Assistant_Bot

# Get production deployment URL
GET /api/knowledge/action/fetch-deployment/Virtual_Assistant_Bot/Production

# Show build status
GET /api/knowledge/action/show-build-status/Virtual_Assistant_Bot

# Execute complex action via POST
POST /api/knowledge/action/execute
{
  "action": "fetch-deployment",
  "repositoryName": "Virtual_Assistant_Bot",
  "environment": "Production"
}
```

---

## Integration with Existing Services

### GitHub Service Integration
The Knowledge Source Service integrates with the existing `GitHubService` to:
- Fetch repositories from GitHub API
- Retrieve deployment information
- Update build status

```csharp
public async Task<object> GetBuildStatusAsync(string repoName)
{
    var deployments = await _gitHubService.GetDeploymentsAsync(repoName);
    return new { /* build status info */ };
}
```

---

## Configuration

The system reads configuration from:
1. **appsettings.json** - Application configuration including GitHub credentials
2. **Environment Variables** - For sensitive data in production
3. **In-memory storage** - For knowledge sources and module information

**appsettings.json:**
```json
{
  "GitHub": {
    "Username": "rajkumarvaluemomentum",
    "Token": "github_pat_..."
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=..."
  }
}
```

---

## Security Considerations

1. **Sensitive Configuration Masking** - GitHub tokens and connection strings are masked in API responses
2. **CORS Policy** - Configured to allow cross-origin requests for web clients
3. **HTTP/HTTPS** - Production deployments enforce HTTPS
4. **Authentication** - GitHub token-based authentication for API calls

---

## Future Enhancements

1. **Database Persistence** - Store knowledge sources in database instead of in-memory
2. **GitLab Integration** - Add support for GitLab repositories
3. **Advanced Search** - Implement full-text search with indexing
4. **Caching** - Add caching for frequently accessed data
5. **Web UI** - Create dashboard for browsing knowledge sources
6. **Webhook Support** - Auto-update knowledge sources on repository changes
7. **Audit Logging** - Track all knowledge base queries and actions
8. **AI Integration** - Use AI to enhance semantic search and query understanding

---

## Testing

### Example API Calls

#### cURL Examples
```bash
# Get all knowledge sources
curl -X GET "http://localhost:5206/api/knowledge/sources"

# Query knowledge base
curl -X GET "http://localhost:5206/api/knowledge/query?q=show+production+url"

# Get repository info
curl -X GET "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot"

# Fetch deployment URL
curl -X GET "http://localhost:5206/api/knowledge/action/fetch-deployment/Virtual_Assistant_Bot/Production"

# Execute action via POST
curl -X POST "http://localhost:5206/api/knowledge/action/execute" \
  -H "Content-Type: application/json" \
  -d '{"action":"fetch-deployment","repositoryName":"Virtual_Assistant_Bot","environment":"Production"}'
```

#### Swagger Testing
Navigate to `http://localhost:5206/swagger` to test all endpoints interactively.

---

## Summary

The Virtual Assistant API now includes a comprehensive knowledge source integration system that:

✅ **Connects Repositories** - GitHub/GitLab links with full metadata
✅ **Manages Deployments** - Multiple environment URLs and statuses
✅ **Catalogs API Endpoints** - Detailed endpoint documentation
✅ **Indexes Code Modules** - Searchable module information
✅ **Stores Configurations** - Environment-specific settings
✅ **Enables Smart Queries** - Semantic search across knowledge base
✅ **Provides Actions** - Executable commands for common operations
✅ **Integrates Existing Services** - Works with GitHub service
✅ **Masks Sensitive Data** - Secure configuration handling
✅ **RESTful API** - Complete REST interface for integration

This system enables the Virtual Assistant to answer questions about code, deployments, configurations, and provide actionable commands for developers and DevOps teams.
