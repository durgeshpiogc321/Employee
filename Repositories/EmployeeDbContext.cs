using Microsoft.EntityFrameworkCore;
using Repositories.DatabaseModels.ResultSet;
using Shared.Common;

namespace Repositories.DatabaseModels
{
    public partial class EmployeeDbContext:DbContext
    {
        public virtual DbSet<GetAllEmployee_Result> GetAllEmployee_Results { get; set; }
        public virtual DbSet<GetAllEmployee_Result> GetEmployee_Results { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationKeys.ConnectionString, options=> options.UseNetTopologySuite());
            }
        }
    }
}
