# Virtual Assistant API - Complete Documentation Index

## 📚 Documentation Overview

Welcome to the Virtual Assistant API! This comprehensive REST API provides intelligent access to code repositories, deployments, configurations, and more.

### Quick Links to Documentation

1. **[QUICK_REFERENCE.md](./QUICK_REFERENCE.md)** ⚡ 
   - Start here! Quick commands, endpoints, and common use cases
   - Perfect for getting started quickly

2. **[KNOWLEDGE_SOURCES.md](./KNOWLEDGE_SOURCES.md)** 📖
   - Complete API documentation with detailed endpoint specifications
   - Response examples for all endpoints
   - Use cases and integration patterns

3. **[CODE_SUMMARY.md](./CODE_SUMMARY.md)** 💻
   - Detailed breakdown of all code components
   - Service architecture and implementation details
   - Data models and flow diagrams

4. **[IMPLEMENTATION_GUIDE.md](./IMPLEMENTATION_GUIDE.md)** 🔧
   - Step-by-step implementation examples
   - Real API call examples with responses
   - Integration workflows

5. **[KNOWLEDGE_SOURCES_README.md](./README.md)** 📝
   - Project overview and setup instructions
   - Deployment information
   - Getting started guide

---

## 🎯 Quick Start (5 Minutes)

### 1. Start the API
```bash
dotnet run --project VirtualAssistant.API.csproj
```

### 2. Access Swagger Documentation
```
http://localhost:5206/swagger
```

### 3. Try Your First Query
```bash
curl "http://localhost:5206/api/knowledge/query?q=production+url"
```

### 4. Get Repository Information
```bash
curl "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot"
```

### 5. List All API Endpoints
```bash
curl "http://localhost:5206/api/knowledge/api-endpoints"
```

---

## 📋 Feature Overview

### ✅ What's Included

#### 1. Repository Connection
- GitHub repository URLs
- GitLab repository URLs
- Documentation links
- Repository metadata and branches

#### 2. Deployment Management
- Multiple environment URLs (Development, Staging, Production)
- Deployment status tracking
- Build status monitoring
- Last deployment timestamps

#### 3. API Endpoint Catalog
- Complete endpoint documentation
- HTTP methods and routes
- Parameter specifications
- Return type information

#### 4. Code Module Indexing
- Module file paths and descriptions
- Dependencies tracking
- Related API endpoints
- Language information

#### 5. Configuration Management
- Environment-specific settings
- Sensitive data masking
- Configuration descriptions
- Development/Production separation

#### 6. Semantic Query System
- Intelligent keyword recognition
- Multi-source search
- Related resource linking
- Context-aware responses

#### 7. Actionable Commands
- Open repository action
- Fetch deployment URL action
- Show build status action
- Execute custom actions

---

## 🚀 Main Endpoints

### Knowledge Sources
| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/knowledge/sources` | GET | List all knowledge sources |
| `/api/knowledge/sources/type/{type}` | GET | Filter sources by type |

### Repository Information
| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/knowledge/repository/{repoName}` | GET | Get repository with all links |
| `/api/knowledge/repository/{repoName}/deployments` | GET | List all deployments |
| `/api/knowledge/repository/{repoName}/deployment/{environment}` | GET | Get specific deployment URL |

### API Documentation
| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/knowledge/api-endpoints` | GET | List all API endpoints |
| `/api/knowledge/api-endpoints?controller={name}` | GET | Filter by controller |

### Code Modules
| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/knowledge/modules` | GET | List all code modules |
| `/api/knowledge/modules?search={keyword}` | GET | Search modules |

### Configuration
| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/knowledge/configurations` | GET | List all configurations |
| `/api/knowledge/configurations?environment={env}` | GET | Filter by environment |

### Query & Search
| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/knowledge/query?q={query}` | GET | Semantic search |

### Build Status
| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/knowledge/repository/{repoName}/build-status` | GET | Get build status |

### Actions
| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/knowledge/action/open-repo/{repoName}` | GET | Get repository URL |
| `/api/knowledge/action/fetch-deployment/{repoName}/{environment}` | GET | Get deployment URL |
| `/api/knowledge/action/show-build-status/{repoName}` | GET | Show build status |
| `/api/knowledge/action/execute` | POST | Execute custom action |

---

## 🔍 Query Examples

### Ask About Deployments
```bash
# Production URL
curl "http://localhost:5206/api/knowledge/query?q=what+is+the+production+url"

# All deployment URLs
curl "http://localhost:5206/api/knowledge/query?q=show+me+all+deployment+environments"
```

### Ask About API
```bash
# Available endpoints
curl "http://localhost:5206/api/knowledge/query?q=what+API+endpoints+are+available"

# GitHub endpoints
curl "http://localhost:5206/api/knowledge/query?q=show+me+GitHub+API+endpoints"
```

### Ask About Code
```bash
# GitHub integration
curl "http://localhost:5206/api/knowledge/query?q=how+is+GitHub+integrated"

# Database module
curl "http://localhost:5206/api/knowledge/query?q=show+me+the+database+context"
```

### Ask About Configuration
```bash
# GitHub config
curl "http://localhost:5206/api/knowledge/query?q=how+is+GitHub+configured"

# All settings
curl "http://localhost:5206/api/knowledge/query?q=show+me+all+configuration"
```

---

## 📁 Project Structure

```
VirtualAssistant.API/
├── Controllers/
│   ├── GitHubController.cs              # GitHub API integration
│   └── KnowledgeController.cs           # NEW: Knowledge base endpoints
├── Services/
│   ├── GitHubService.cs                 # GitHub API client
│   └── KnowledgeSourceService.cs        # NEW: Knowledge base management
├── Models/
│   ├── DeploymentDto.cs                 # Deployment model
│   ├── RepositoryDto.cs                 # Repository model
│   ├── KnowledgeSource.cs               # NEW: Knowledge source models
│   └── KnowledgeBaseModels.cs           # NEW: Additional models
├── Data/
│   └── ApplicationDbContext.cs           # Entity Framework context
├── Documentation/
│   ├── QUICK_REFERENCE.md               # Quick start guide
│   ├── KNOWLEDGE_SOURCES.md             # API documentation
│   ├── CODE_SUMMARY.md                  # Code overview
│   ├── IMPLEMENTATION_GUIDE.md          # Implementation details
│   └── README.md                        # Project README
└── Program.cs                           # Application startup
```

---

## 🛠️ Key Technologies

- **Framework:** .NET 8
- **Web API:** ASP.NET Core
- **Database:** Entity Framework Core + SQL Server
- **API Docs:** Swagger/OpenAPI
- **GitHub Integration:** Direct GitHub API
- **Container:** Docker & Docker Compose
- **Deployment:** Render.com

---

## 🌐 Deployment URLs

### Development
- **Base URL:** http://localhost:5206
- **Swagger:** http://localhost:5206/swagger

### Production
- **Base URL:** https://virtual-assistant-bot.onrender.com
- **Swagger:** https://virtual-assistant-bot.onrender.com/swagger
- **Repository:** https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot

---

## 📚 Understanding the System

### Data Flow

```
User Request
    ↓
KnowledgeController (REST endpoint)
    ↓
KnowledgeSourceService (Business Logic)
    ↓
Data Models (KnowledgeSource, Repository, Deployment, etc.)
    ↓
GitHub Service (External API calls if needed)
    ↓
Response JSON
```

### Knowledge Base Structure

```
Knowledge Base
├── Knowledge Sources
│   ├── GitHub Repositories
│   ├── GitLab Repositories
│   ├── Documentation Links
│   └── Deployment URLs
├── Code Modules
│   ├── Module 1: GitHub Integration
│   ├── Module 2: Database Context
│   └── Module 3: API Configuration
├── API Endpoints
│   ├── GitHub Controller endpoints
│   ├── Knowledge Controller endpoints
│   └── Other endpoints
├── Configurations
│   ├── Development settings
│   ├── Production settings
│   └── Shared settings
└── Search Index (for queries)
    ├── Keywords
    ├── Descriptions
    └── Metadata
```

---

## 🔐 Security Features

✅ **Authentication** - GitHub token-based authentication
✅ **CORS** - Cross-origin requests enabled
✅ **HTTPS** - SSL/TLS in production
✅ **Sensitive Data Masking** - Tokens and passwords hidden in responses
✅ **Configuration Security** - Environment variables for secrets

---

## 🧪 Testing the API

### Using Swagger UI (Recommended)
1. Navigate to http://localhost:5206/swagger
2. Click on each endpoint to see details
3. Click "Try it out" to test

### Using cURL
```bash
# Get knowledge sources
curl http://localhost:5206/api/knowledge/sources

# Query knowledge base
curl "http://localhost:5206/api/knowledge/query?q=production+url"

# Get repository
curl http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot

# Execute action
curl http://localhost:5206/api/knowledge/action/fetch-deployment/Virtual_Assistant_Bot/Production
```

### Using Postman
1. Import the endpoints from Swagger
2. Create requests for each endpoint
3. Save as collection for reuse

### Using REST Client Extension (VS Code)
Use the included `VirtualAssistant.API.http` file to test endpoints directly in VS Code.

---

## 🚀 Getting Started Guide

### Step 1: Prerequisites
- .NET 8 SDK installed
- Visual Studio Code or Visual Studio
- SQL Server (local or remote)

### Step 2: Clone Repository
```bash
git clone https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot.git
cd Virtual_Assistant_Bot
```

### Step 3: Update Configuration
Edit `appsettings.json`:
```json
{
  "GitHub": {
    "Username": "your-github-username",
    "Token": "your-github-token"
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string"
  }
}
```

### Step 4: Build Project
```bash
dotnet build
```

### Step 5: Run Application
```bash
dotnet run
```

### Step 6: Access API
- **Swagger UI:** http://localhost:5206/swagger
- **API Base:** http://localhost:5206

---

## 📞 Common Tasks

### Find Production URL
```bash
# Option 1: Direct query
curl "http://localhost:5206/api/knowledge/query?q=production+url"

# Option 2: Get deployment
curl "http://localhost:5206/api/knowledge/action/fetch-deployment/Virtual_Assistant_Bot/Production"

# Option 3: List deployments
curl "http://localhost:5206/api/knowledge/repository/Virtual_Assistant_Bot/deployments"
```

### Find API Documentation
```bash
# List all endpoints
curl "http://localhost:5206/api/knowledge/api-endpoints"

# Find GitHub endpoints
curl "http://localhost:5206/api/knowledge/api-endpoints?controller=GitHub"
```

### Find Implementation Details
```bash
# Search for GitHub module
curl "http://localhost:5206/api/knowledge/modules?search=github"

# Query for module details
curl "http://localhost:5206/api/knowledge/query?q=github+integration"
```

### Check Configuration
```bash
# Get all configurations
curl "http://localhost:5206/api/knowledge/configurations"

# Get development configs
curl "http://localhost:5206/api/knowledge/configurations?environment=Development"
```

### Get Build Status
```bash
curl "http://localhost:5206/api/knowledge/action/show-build-status/Virtual_Assistant_Bot"
```

---

## 🤔 Troubleshooting

### Build Fails
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

### Port Already in Use
```bash
# Change port in launchSettings.json
# Or use: set PORT=8080
```

### GitHub API Errors
- Check GitHub token in configuration
- Verify token has necessary permissions
- Check GitHub API rate limits

### Database Connection Error
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database exists

---

## 📖 Complete Documentation Files

### QUICK_REFERENCE.md
- Quick start commands
- Common endpoint patterns
- Use case examples
- Troubleshooting tips

### KNOWLEDGE_SOURCES.md
- Full API specification
- All endpoint details
- Response examples
- Integration guide

### CODE_SUMMARY.md
- Complete code overview
- Component descriptions
- Architecture explanation
- API flow diagrams

### IMPLEMENTATION_GUIDE.md
- Detailed API examples
- Real-world usage patterns
- Request/response samples
- Workflow examples

---

## 🎓 Learning Resources

### For API Users
1. Start with QUICK_REFERENCE.md
2. Browse KNOWLEDGE_SOURCES.md for detailed specs
3. Test endpoints using Swagger UI
4. Try IMPLEMENTATION_GUIDE.md examples

### For Developers
1. Review CODE_SUMMARY.md for architecture
2. Study individual service implementations
3. Check Models for data structures
4. Review Controllers for endpoint logic

### For DevOps
1. See deployment information in CODE_SUMMARY.md
2. Check Docker configuration
3. Review environment settings
4. Check launchSettings.json

---

## ✨ Features Summary

| Feature | Status | Documentation |
|---------|--------|---|
| GitHub Repository Links | ✅ Complete | KNOWLEDGE_SOURCES.md |
| GitLab Support | ✅ Complete | KNOWLEDGE_SOURCES.md |
| Deployment URLs | ✅ Complete | QUICK_REFERENCE.md |
| API Endpoints Catalog | ✅ Complete | IMPLEMENTATION_GUIDE.md |
| Code Module Indexing | ✅ Complete | CODE_SUMMARY.md |
| Configuration Management | ✅ Complete | KNOWLEDGE_SOURCES.md |
| Semantic Search | ✅ Complete | KNOWLEDGE_SOURCES.md |
| Build Status | ✅ Complete | QUICK_REFERENCE.md |
| Actionable Commands | ✅ Complete | IMPLEMENTATION_GUIDE.md |
| Swagger Documentation | ✅ Complete | Swagger UI |
| Docker Support | ✅ Complete | Dockerfile |
| Production Deployment | ✅ Complete | Render.com |

---

## 🔗 Related Resources

- **GitHub Repository:** https://github.com/rajkumarvaluemomentum/Virtual_Assistant_Bot
- **Production API:** https://virtual-assistant-bot.onrender.com
- **Swagger Documentation:** https://virtual-assistant-bot.onrender.com/swagger
- **.NET 8 Documentation:** https://docs.microsoft.com/dotnet
- **ASP.NET Core Documentation:** https://docs.microsoft.com/aspnet/core
- **GitHub API Documentation:** https://docs.github.com/rest

---

## 📋 Checklist

Before deploying to production:
- [ ] Update GitHub credentials in environment variables
- [ ] Configure correct database connection string
- [ ] Set PORT environment variable (default: 10000)
- [ ] Enable HTTPS in production
- [ ] Review and update CORS policy if needed
- [ ] Test all endpoints with production URLs
- [ ] Verify sensitive data is masked in responses
- [ ] Check Docker configuration
- [ ] Review security settings
- [ ] Set up monitoring and logging

---

## 🎉 You're All Set!

Your Virtual Assistant API is now fully equipped with:

✅ Complete knowledge source integration
✅ Repository and deployment management
✅ Semantic query system
✅ Code module indexing
✅ Configuration management
✅ Actionable commands
✅ Full REST API
✅ Swagger documentation
✅ Production-ready deployment
✅ Comprehensive documentation

**Start exploring:** http://localhost:5206/swagger

**Need help?** Check the documentation files or the QUICK_REFERENCE.md!

---

## 📞 Support

For questions or issues:
1. Check QUICK_REFERENCE.md for troubleshooting
2. Review KNOWLEDGE_SOURCES.md for API details
3. See IMPLEMENTATION_GUIDE.md for usage examples
4. Check GitHub repository issues
5. Review application logs for errors

---

**Happy coding! 🚀**
