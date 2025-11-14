# Virtual Assistant API - Complete Code Summary

## Project Overview

**Virtual Assistant Bot** is a .NET 8 REST API that integrates with GitHub to provide intelligent assistance for code repositories, deployments, and configurations.

**Repository:** https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot
**Current Branch:** Dev
**Framework:** .NET 8
**Language:** C#

---

## Project Structure

```
VirtualAssistant.API/
├── Controllers/
│   ├── GitHubController.cs          # GitHub integration endpoints
│   └── KnowledgeController.cs       # Knowledge source API endpoints
├── Services/
│   ├── GitHubService.cs             # GitHub API client
│   └── KnowledgeSourceService.cs    # Knowledge base management
├── Models/
│   ├── DeploymentDto.cs             # Deployment data model
│   ├── RepositoryDto.cs             # Repository data model
│   ├── KnowledgeSource.cs           # Knowledge source models
│   └── KnowledgeBaseModels.cs       # Additional knowledge base models
├── Data/
│   └── ApplicationDbContext.cs       # Entity Framework Core context
├── Properties/
│   └── launchSettings.json           # Development settings
├── Program.cs                        # Application startup
├── appsettings.json                 # Configuration
├── appsettings.Development.json     # Development configuration
├── Dockerfile                       # Docker configuration
├── compose.yaml                     # Docker Compose production
├── compose.debug.yaml               # Docker Compose debug
└── VirtualAssistant.API.http        # REST client requests
```

---

## Core Components

### 1. Program.cs - Application Startup

**Purpose:** Bootstrap the application, configure services, and set up middleware.

**Key Configurations:**
- ASP.NET Core web builder setup
- CORS policy for all origins
- Swagger/OpenAPI documentation
- GitHub service registration
- Knowledge Source service registration
- Port configuration for Render deployment (10000)

**Main Features:**
```csharp
// Service Registration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<GitHubService>();
builder.Services.AddScoped<GitHubService>();
builder.Services.AddScoped<KnowledgeSourceService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Production Port Configuration
if (!builder.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}
```

---

### 2. Controllers

#### GitHubController.cs
**Route:** `/api/github`

**Endpoints:**
1. `GET /api/github/repositories`
   - Fetches all repositories for authenticated GitHub user
   - Returns: `IEnumerable<string>`
   - Error Handling: 401 Unauthorized, 502 Bad Gateway

2. `GET /api/github/repositories/{repoName}/deployments`
   - Fetches deployment history for a specific repository
   - Parameters: `repoName` (string)
   - Returns: `IEnumerable<object>` (deployment objects)

**Implementation:**
- Dependency injection of `GitHubService`
- Try-catch error handling with appropriate HTTP status codes
- Detailed error messages for debugging

```csharp
[ApiController]
[Route("api/[controller]")]
public class GitHubController : ControllerBase
{
    private readonly GitHubService _gitHubService;
    
    [HttpGet("repositories")]
    public async Task<IActionResult> GetRepositories()
    {
        try
        {
            var repos = await _gitHubService.GetRepositoriesAsync();
            return Ok(repos);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}
```

#### KnowledgeController.cs
**Route:** `/api/knowledge`

**Endpoints:** (20+ endpoints)

**Main Categories:**

1. **Knowledge Sources**
   - `GET /api/knowledge/sources` - All sources
   - `GET /api/knowledge/sources/type/{type}` - Filtered by type

2. **Repository Information**
   - `GET /api/knowledge/repository/{repoName}` - Full repository info
   - `GET /api/knowledge/repository/{repoName}/deployments` - All deployments
   - `GET /api/knowledge/repository/{repoName}/deployment/{environment}` - Specific deployment

3. **API Endpoints**
   - `GET /api/knowledge/api-endpoints` - All endpoints
   - `GET /api/knowledge/api-endpoints?controller=X` - Filtered by controller

4. **Code Modules**
   - `GET /api/knowledge/modules` - All modules
   - `GET /api/knowledge/modules?search=X` - Search modules

5. **Configuration**
   - `GET /api/knowledge/configurations` - All configs
   - `GET /api/knowledge/configurations?environment=X` - Filtered by environment

6. **Query & Search**
   - `GET /api/knowledge/query?q=X` - Semantic search

7. **Build Status**
   - `GET /api/knowledge/repository/{repoName}/build-status` - Build status

8. **Actions**
   - `GET /api/knowledge/action/open-repo/{repoName}` - Get repository URL
   - `GET /api/knowledge/action/fetch-deployment/{repoName}/{environment}` - Get deployment URL
   - `GET /api/knowledge/action/show-build-status/{repoName}` - Show build status
   - `POST /api/knowledge/action/execute` - Execute custom action

---

### 3. Services

#### GitHubService.cs
**Location:** `Services/GitHubService.cs`

**Purpose:** Encapsulates GitHub API interactions using HttpClient.

**Key Features:**
- GitHub API authentication via personal token
- Support for both environment variables and appsettings.json configuration
- User agent header for GitHub API compliance
- Error handling with detailed messages

**Methods:**

1. **GetRepositoriesAsync()**
   ```csharp
   public async Task<IEnumerable<string>> GetRepositoriesAsync()
   ```
   - Fetches all repositories for the authenticated user
   - GitHub API endpoint: `GET /users/{username}/repos`
   - Returns repository names
   - Throws `UnauthorizedAccessException` for 401 errors
   - Throws `HttpRequestException` for other errors

2. **GetDeploymentsAsync(string repoName)**
   ```csharp
   public async Task<IEnumerable<object>> GetDeploymentsAsync(string repoName)
   ```
   - Fetches deployment objects for a repository
   - GitHub API endpoint: `GET /repos/{owner}/{repo}/deployments`
   - Returns deserialized deployment objects
   - Comprehensive error handling

**Authentication:**
```csharp
public GitHubService(HttpClient httpClient, IConfiguration configuration)
{
    _token = Environment.GetEnvironmentVariable("GITHUB_TOKEN")
        ?? configuration["GitHub:Token"];
    
    _username = configuration["GitHub:Username"];
    
    _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("token", _token);
}
```

#### KnowledgeSourceService.cs
**Location:** `Services/KnowledgeSourceService.cs`

**Purpose:** Central service managing the knowledge base including repositories, modules, endpoints, and configurations.

**Initialization:**
- Loads default knowledge sources from configuration
- Initializes code modules with API endpoint information
- Sets up configuration entries for different environments

**Key Methods:**

1. **GetAllKnowledgeSources()**
   - Returns all active knowledge sources
   - Filters by `IsActive` flag

2. **GetKnowledgeSourcesByType(string type)**
   - Filters sources by type (GitHub, GitLab, Documentation, Deployment)
   - Returns: `IEnumerable<KnowledgeSource>`

3. **GetRepositoryLink(string repositoryName)**
   - Creates or retrieves repository link with deployments
   - Generates GitHub/GitLab URLs
   - Includes development and production deployment URLs

4. **SearchModules(string keyword)**
   - Searches code modules by keyword
   - Searches in module name, description, and file path
   - Case-insensitive matching

5. **GetApiEndpoints(string controllerName = null)**
   - Returns all API endpoints
   - Filters by controller if specified
   - Includes method, route, parameters, return type

6. **GetConfigurations(string environment = null)**
   - Returns configuration entries
   - Filters by environment (Development, Production, etc.)
   - Masks sensitive values

7. **QueryKnowledgeBase(string query)**
   - Semantic query processing
   - Understands keywords: repo, deploy, api, config, module, code
   - Returns comprehensive `QueryResponse` with related resources

8. **GetBuildStatusAsync(string repoName)**
   - Fetches build status from GitHub deployments
   - Async operation using GitHub service
   - Returns latest deployment information

9. **GetRepositoryOpenUrl(string repositoryName)**
   - Returns GitHub repository URL for opening

10. **GetDeploymentUrl(string repositoryName, string environment)**
    - Gets deployment URL for specific environment
    - Returns: `string` (URL or "Not found")

11. **GetAllDeployments(string repositoryName)**
    - Returns list of all deployment environments with URLs and status

12. **GetCodeSnippet(string filePath, int startLine = -1, int endLine = -1)**
    - Creates code snippet metadata
    - Auto-detects language from file extension

**Data Initialization:**

The service initializes with:

**Knowledge Sources:**
- GitHub Main Repository
- Production Deployment (Render)
- API Documentation (Swagger)
- Local Development Environment

**Code Modules:**
1. GitHub Integration (Services/GitHubService.cs)
   - Dependency: Octokit, HttpClient
   - Endpoints: repositories, deployments

2. Database Context (Data/ApplicationDbContext.cs)
   - Dependency: Entity Framework Core, SQL Server

3. API Configuration (Program.cs)
   - Dependency: Swagger, CORS, Entity Framework

**Configurations:**
- GitHub:Username
- GitHub:Token (masked)
- ConnectionStrings:DefaultConnection
- PORT
- ASPNETCORE_ENVIRONMENT

---

### 4. Models

#### DeploymentDto.cs
```csharp
public class DeploymentDto
{
    public long Id { get; set; }
    public string Sha { get; set; }              // Commit SHA
    public string Ref { get; set; }              // Branch/Tag reference
    public string Environment { get; set; }       // Deployment environment
    public string Creator { get; set; }           // User who created deployment
    public string CreatedAt { get; set; }         // Creation timestamp
    public string Status { get; set; }            // Deployment status
}
```

#### RepositoryDto.cs
```csharp
public class RepositoryDto
{
    public string Name { get; set; }
    public string HtmlUrl { get; set; }          // GitHub URL
    public string Description { get; set; }
    public string DefaultBranch { get; set; }
}
```

#### KnowledgeSource.cs
Comprehensive models for knowledge base:

**KnowledgeSource** - Base knowledge source model
**RepositoryLink** - Repository with deployment URLs
**DeploymentUrl** - Deployment information
**CodeModuleInfo** - Code module metadata
**ApiEndpointInfo** - API endpoint details
**ConfigurationInfo** - Configuration entries
**CodeSnippet** - Code snippet metadata
**QueryResponse** - Query result response

#### KnowledgeBaseModels.cs
Additional models:

**RepositoryConfiguration** - Repository config and metadata
**EnvironmentConfig** - Environment-specific configuration
**KnowledgeBaseSummary** - Knowledge base statistics
**SearchResult** - Search result structure
**ModuleDocumentation** - Module documentation
**CodeExample** - Code example

---

## Data & Configuration

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=VSP-8VPLD74;Database=VirtualAssistantDB;..."
  },
  "GitHub": {
    "Username": "rajkumarvaluemomentum",
    "Token": "github_pat_..."
  }
}
```

### ApplicationDbContext.cs
Entity Framework Core database context for application data.

### launchSettings.json

**Profiles:**
1. **http** - HTTP development server (port 5206)
2. **https** - HTTPS development server (port 7206)
3. **IIS Express** - IIS Express development

All profiles launch Swagger UI on startup.

---

## Deployment

### Docker Configuration

**Dockerfile:**
- Multi-stage build
- Base image: mcr.microsoft.com/dotnet/aspnet:8.0
- Target frameworks: net8.0

**Compose Files:**
- `compose.yaml` - Production deployment
- `compose.debug.yaml` - Debug deployment

### Render Deployment
- Platform: Render.com
- Port: 10000 (configurable via PORT env variable)
- URL: https://virtual-assistant-bot.onrender.com
- Auto-deploys on Git push to Dev branch

---

## API Flows

### Repository Query Flow
```
User Request
    ↓
KnowledgeController.GetRepositoryInfo()
    ↓
KnowledgeSourceService.GetRepositoryLink()
    ↓
Returns RepositoryLink with:
  - GitHub URL
  - GitLab URL
  - Documentation URL
  - All Deployment URLs
```

### Build Status Flow
```
User Request
    ↓
KnowledgeController.GetBuildStatus()
    ↓
KnowledgeSourceService.GetBuildStatusAsync()
    ↓
GitHubService.GetDeploymentsAsync()
    ↓
GitHub API: GET /repos/{owner}/{repo}/deployments
    ↓
Returns Latest Deployments & Status
```

### Query Flow
```
User Query: "show production URL"
    ↓
KnowledgeController.QueryKnowledgeBase()
    ↓
KnowledgeSourceService.QueryKnowledgeBase()
    ↓
Keyword Analysis: "deploy", "url" → DeploymentUrls
    ↓
Returns QueryResponse with:
  - Related DeploymentUrl objects
  - Relevant URLs in RelatedResources
  - Code snippets if applicable
```

### Action Execution Flow
```
POST /api/knowledge/action/execute
{
  "action": "fetch-deployment",
  "repositoryName": "Virtual_Assistant_Bot",
  "environment": "Production"
}
    ↓
KnowledgeController.ExecuteAction()
    ↓
Switch on action type → fetch-deployment
    ↓
KnowledgeSourceService.GetDeploymentUrl()
    ↓
Returns deployment URL + message
```

---

## Security Implementation

### Authentication & Authorization
1. **GitHub Token** - Personal access token for GitHub API
2. **CORS Policy** - Allow all origins (configurable)
3. **HTTP/HTTPS** - HTTPS enforced in production
4. **Sensitive Data Masking** - Tokens masked in API responses

### Configuration Security
- Environment variables for production secrets
- Appsettings.json for development only
- Sensitive flags on configuration entries

---

## Key Technologies & Dependencies

**Framework:** .NET 8
**Web:** ASP.NET Core
**ORM:** Entity Framework Core 8.0
**Database:** SQL Server
**API Documentation:** Swagger (Swashbuckle 10.0)
**GitHub Integration:** Octokit 10.0
**Container:** Docker

---

## Development URLs

- **HTTP:** http://localhost:5206
- **HTTPS:** https://localhost:7206
- **Swagger UI:** http://localhost:5206/swagger
- **IIS Express:** http://localhost:59831

---

## Production URLs

- **API Base:** https://virtual-assistant-bot.onrender.com
- **Swagger UI:** https://virtual-assistant-bot.onrender.com/swagger
- **GitHub Repository:** https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot

---

## Key Features

✅ **GitHub Integration** - Fetch repositories and deployment information
✅ **Knowledge Base** - Central repository for code, configurations, and deployments
✅ **Semantic Query** - Intelligent search across knowledge sources
✅ **API Endpoints Documentation** - Searchable endpoint catalog
✅ **Code Modules Index** - Searchable module information
✅ **Configuration Management** - Environment-specific settings
✅ **Build Status Monitoring** - GitHub deployment tracking
✅ **Actionable Commands** - Open repo, fetch URLs, show status
✅ **RESTful API** - Complete REST interface with Swagger documentation
✅ **Deployment Support** - Docker, Docker Compose, Render
✅ **Error Handling** - Comprehensive error responses
✅ **CORS Support** - Cross-origin requests enabled

---

## API Request Examples

### Query Production URL
```bash
GET /api/knowledge/query?q=production+deployment+url
```

### Get All API Endpoints
```bash
GET /api/knowledge/api-endpoints
```

### Search GitHub Module
```bash
GET /api/knowledge/modules?search=github
```

### Get Repository Info
```bash
GET /api/knowledge/repository/Virtual_Assistant_Bot
```

### Open Repository
```bash
GET /api/knowledge/action/open-repo/Virtual_Assistant_Bot
```

### Execute Custom Action
```bash
POST /api/knowledge/action/execute
{
  "action": "fetch-deployment",
  "repositoryName": "Virtual_Assistant_Bot",
  "environment": "Production"
}
```

---

## Extensibility

The system is designed for easy extension:

1. **Add New Knowledge Sources** - Extend `KnowledgeSourceService`
2. **Add New Endpoints** - Add methods to `ApiEndpointInfo` list
3. **Add New Modules** - Extend `_modules` list in service initialization
4. **Add New Controllers** - Create new controller inheriting from `ControllerBase`
5. **Add Database Models** - Extend `ApplicationDbContext`
6. **Add Custom Queries** - Enhance `QueryKnowledgeBase()` logic

---

## Maintenance & Updates

- **Configuration Updates** - Modify `appsettings.json` or environment variables
- **Module Indexing** - Update `_modules` initialization in `KnowledgeSourceService`
- **Endpoint Tracking** - Keep `ApiEndpointInfo` list synchronized with controllers
- **Deployment URLs** - Update `DeploymentUrls` in `GetRepositoryLink()`
- **Code Snippets** - Add new snippets via `GetCodeSnippet()` method

---

## Summary

The Virtual Assistant API is a comprehensive .NET 8 REST API that provides:

1. **GitHub Integration** - Direct access to repositories and deployments
2. **Knowledge Management** - Centralized knowledge source indexing
3. **Smart Querying** - Semantic search across all knowledge sources
4. **Developer Tools** - API endpoint documentation and code module indexing
5. **Configuration Management** - Environment-specific configuration tracking
6. **Actionable Operations** - Commands to open repos, fetch URLs, check build status
7. **Production Ready** - Docker support, error handling, CORS, Swagger documentation
8. **Extensible Architecture** - Easy to add new features and data sources

The system enables developers and DevOps teams to query code repositories, configuration details, deployment URLs, and execute common operations through an intelligent REST API interface.
