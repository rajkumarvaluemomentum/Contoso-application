# Virtual Assistant API - Quick Reference Guide

## What Was Added

### 1. New Services
- **KnowledgeSourceService** (`Services/KnowledgeSourceService.cs`)
  - Manages all knowledge sources, repositories, modules, and configurations
  - Provides semantic search and query capabilities
  - Integrates with GitHub service for build status

### 2. New Controllers
- **KnowledgeController** (`Controllers/KnowledgeController.cs`)
  - 20+ REST endpoints for knowledge base operations
  - Query interface for semantic search
  - Action execution endpoints

### 3. New Models
- **KnowledgeSource.cs** - Complete knowledge base models
- **KnowledgeBaseModels.cs** - Additional support models
- Repository, deployment, module, endpoint, configuration models

### 4. Documentation
- **KNOWLEDGE_SOURCES.md** - Complete API documentation
- **CODE_SUMMARY.md** - Detailed code breakdown

---

## Quick Start

### 1. Start Development Server
```bash
# Using VS Code task
- Press Ctrl+Shift+B to run build task
- Or run: dotnet run --project VirtualAssistant.API.csproj
```

### 2. Access API Documentation
```
http://localhost:5206/swagger
```

### 3. Test Knowledge Base Endpoints
```bash
# Get all knowledge sources
curl http://localhost:5206/api/knowledge/sources

# Query knowledge base
curl "http://localhost:5206/api/knowledge/query?q=what+is+the+production+url"

# Get repository info
curl http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot

# Get API endpoints
curl http://localhost:5206/api/knowledge/api-endpoints

# Get all modules
curl http://localhost:5206/api/knowledge/modules

# Get configurations
curl http://localhost:5206/api/knowledge/configurations

# Execute action
curl http://localhost:5206/api/knowledge/action/open-repo/Virtual_Assistant_Bot
```

---

## Main Endpoint Categories

### 📚 Knowledge Sources
```
GET /api/knowledge/sources                    - All sources
GET /api/knowledge/sources/type/{type}        - Filter by type (GitHub, GitLab, Documentation, Deployment)
```

### 📦 Repository Information
```
GET /api/knowledge/repository/{repoName}                      - Complete repo info
GET /api/knowledge/repository/{repoName}/deployments          - All deployments
GET /api/knowledge/repository/{repoName}/deployment/{env}     - Specific environment
```

### 🔌 API Endpoints
```
GET /api/knowledge/api-endpoints                    - All endpoints
GET /api/knowledge/api-endpoints?controller={name}  - Filter by controller
```

### 📁 Code Modules
```
GET /api/knowledge/modules                      - All modules
GET /api/knowledge/modules?search={keyword}     - Search modules
```

### ⚙️ Configuration
```
GET /api/knowledge/configurations                         - All configurations
GET /api/knowledge/configurations?environment={env}       - Filter by environment
```

### 🔍 Search & Query
```
GET /api/knowledge/query?q={query}              - Semantic search
```

### 📊 Build Status
```
GET /api/knowledge/repository/{repoName}/build-status    - Latest build status
```

### ⚡ Actions
```
GET /api/knowledge/action/open-repo/{repoName}                                  - Get repo URL
GET /api/knowledge/action/fetch-deployment/{repoName}/{environment}             - Get deployment URL
GET /api/knowledge/action/show-build-status/{repoName}                          - Show build status
POST /api/knowledge/action/execute                                              - Execute custom action
```

---

## Example Queries

### Query Production Deployment URL
```bash
curl "http://localhost:5206/api/knowledge/query?q=what+is+the+production+deployment+url"

# Response includes:
# - Production DeploymentUrl objects
# - Actual URLs in RelatedResources
# - Description of what was found
```

### Query GitHub Configuration
```bash
curl "http://localhost:5206/api/knowledge/query?q=how+is+github+configured"

# Response includes:
# - GitHub configuration information
# - Username and token status
# - Usage details
```

### Query Available API Endpoints
```bash
curl "http://localhost:5206/api/knowledge/query?q=show+me+all+API+endpoints"

# Response includes:
# - List of all API endpoints
# - Methods, routes, parameters
# - Controller information
```

### Search for GitHub Module
```bash
curl "http://localhost:5206/api/knowledge/modules?search=github"

# Response includes:
# - GitHub Integration module
# - Dependencies
# - Related API endpoints
```

---

## Common Use Cases

### 1. For Developers
```bash
# Find API documentation
curl http://localhost:5206/api/knowledge/api-endpoints

# Search for specific module
curl "http://localhost:5206/api/knowledge/modules?search=database"

# Get code module details
curl "http://localhost:5206/api/knowledge/query?q=show+me+the+database+context"
```

### 2. For DevOps
```bash
# Get all deployment URLs
curl "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot/deployments"

# Get production URL
curl "http://localhost:5206/api/knowledge/action/fetch-deployment/Virtual_Assistant_Bot/Production"

# Check build status
curl "http://localhost:5206/api/knowledge/action/show-build-status/Virtual_Assistant_Bot"
```

### 3. For Project Managers
```bash
# Get complete project information
curl "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot"

# Get all knowledge sources
curl http://localhost:5206/api/knowledge/sources

# Query for production URL
curl "http://localhost:5206/api/knowledge/query?q=production+url"
```

### 4. For Integration
```bash
# Execute custom action via POST
curl -X POST http://localhost:5206/api/knowledge/action/execute \
  -H "Content-Type: application/json" \
  -d '{
    "action": "fetch-deployment",
    "repositoryName": "Virtual_Assistant_Bot",
    "environment": "Production"
  }'
```

---

## Data Models at a Glance

### KnowledgeSource
- **Id**: Unique identifier
- **Type**: GitHub, GitLab, Documentation, Deployment
- **Name**: Display name
- **Url**: Link to resource
- **Description**: What it is
- **Metadata**: Additional properties
- **IsActive**: Boolean flag

### RepositoryLink
- **RepositoryName**: Name
- **Owner**: Repository owner
- **GitHubUrl**: GitHub link
- **GitLabUrl**: GitLab link
- **DocumentationUrl**: Docs link
- **DeploymentUrls**: List of deployments
- **DefaultBranch**: Default branch name

### DeploymentUrl
- **Environment**: Development, Staging, Production
- **Url**: Deployment URL
- **Status**: Active, Inactive, Maintenance
- **BuildStatus**: Success, Failed, InProgress
- **LastDeployed**: Timestamp
- **DeploymentDetailsUrl**: Details link

### CodeModuleInfo
- **ModuleName**: Module name
- **FilePath**: File location
- **Language**: Programming language
- **Description**: What it does
- **Dependencies**: Required libraries
- **ApiEndpoints**: Related endpoints

### ApiEndpointInfo
- **Method**: GET, POST, PUT, DELETE
- **Route**: API path
- **Description**: What it does
- **Controller**: Controller name
- **Parameters**: Required parameters
- **ReturnType**: Return type

### ConfigurationInfo
- **Key**: Configuration key
- **Value**: Configuration value (masked if sensitive)
- **Environment**: Development, Production, etc.
- **Description**: What it's for
- **IsSensitive**: Boolean flag

---

## Response Format

All responses follow this format:

```json
{
  "success": true,
  "message": "Description of result",
  "data": { /* actual data */ },
  "codeSnippets": [],
  "relatedResources": ["url1", "url2"]
}
```

---

## Error Responses

```json
{
  "error": "Error message description",
  "status": 400
}
```

---

## Configuration & Deployment

### Development
- **Port**: 5206 (HTTP) or 7206 (HTTPS)
- **Swagger**: http://localhost:5206/swagger
- **Database**: SQL Server (local)

### Production
- **Platform**: Render.com
- **Port**: 10000 (configurable)
- **URL**: https://virtual-assistant-bot.onrender.com
- **Swagger**: https://virtual-assistant-bot.onrender.com/swagger

### Docker
```bash
# Build debug image
docker build -t virtualassistantapi:dev -f Dockerfile --target base .

# Build release image
docker build -t virtualassistantapi:latest -f Dockerfile .

# Run with docker-compose
docker-compose -f compose.yaml up
docker-compose -f compose.debug.yaml up
```

---

## Key Features Summary

✅ **Repository Linking** - GitHub/GitLab URLs with metadata
✅ **Deployment Management** - Multiple environment URLs and status
✅ **API Documentation** - Complete endpoint catalog
✅ **Code Indexing** - Searchable modules with dependencies
✅ **Configuration Tracking** - Environment-specific settings
✅ **Semantic Search** - Intelligent query understanding
✅ **Build Status** - GitHub deployment monitoring
✅ **Actionable Commands** - Execute common operations
✅ **REST API** - Complete RESTful interface
✅ **Swagger UI** - Interactive documentation

---

## Next Steps

1. **Start the API** - Run the application
2. **Open Swagger** - http://localhost:5206/swagger
3. **Test Endpoints** - Use Swagger UI to test
4. **Read Documentation** - See KNOWLEDGE_SOURCES.md for detailed docs
5. **Integrate** - Use the API in your applications

---

## Support & Documentation

- **Full API Documentation**: `KNOWLEDGE_SOURCES.md`
- **Code Overview**: `CODE_SUMMARY.md`
- **Swagger UI**: `http://localhost:5206/swagger`
- **GitHub Repository**: `https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot`

---

## Building & Running

### Build
```bash
dotnet build VirtualAssistant.API.sln
```

### Run
```bash
dotnet run --project VirtualAssistant.API.csproj
```

### Publish
```bash
dotnet publish VirtualAssistant.API.sln
```

### Run in Docker
```bash
docker-compose up
```

---

## Troubleshooting

### Build Fails
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

### Port Already in Use
```bash
# Change port in launchSettings.json
# Or set environment variable: set PORT=8080
```

### Database Connection Error
```bash
# Check connection string in appsettings.json
# Ensure SQL Server is running
# Update connection string as needed
```

### GitHub API Error
```bash
# Check GitHub token in appsettings.json
# Verify token has sufficient permissions
# Check GitHub API rate limits
```

---

## Performance Tips

1. **Use Query Parameters** - Filter data with parameters
   ```bash
   /api/knowledge/configurations?environment=Production
   ```

2. **Search Specific Modules** - Narrow down search
   ```bash
   /api/knowledge/modules?search=github
   ```

3. **Cache Results** - Reuse query results if data doesn't change frequently

4. **Use Specific Endpoints** - Don't query all if you need specific item
   ```bash
   /api/knowledge/repository/{repoName}/deployment/{environment}
   ```

---

This comprehensive implementation provides everything needed to integrate repositories, links, and deployment URLs as knowledge sources with semantic search and actionable commands!
