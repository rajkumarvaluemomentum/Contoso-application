using Microsoft.EntityFrameworkCore;
using VirtualAssistant.API.Models;

namespace VirtualAssistant.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<RepositoryDto> Repositories { get; set; }
        public DbSet<DeploymentDto> Deployments { get; set; }
    }
}
