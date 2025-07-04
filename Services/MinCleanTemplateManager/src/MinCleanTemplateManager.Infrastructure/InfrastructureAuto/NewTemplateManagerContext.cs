using GlobalConstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MinCleanTemplateManager.Domain.Entities;

namespace MinCleanTemplateManager.Infrastructure.Persistence
{
    public class MinCleanTemplateManagerContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionStringProvider _connectionStringProvider;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            var constr = _connectionStringProvider.ConnectionString;
            // var constr = GetConnectionstringName.GetConnectionStrName(Environment.MachineName);
            var conn = _configuration.GetConnectionString(constr);

        #if (UseSqlServer)
                  optionsBuilder.UseSqlServer(conn!)
                  .EnableSensitiveDataLogging()
                  .LogTo(Console.WriteLine, LogLevel.Information);
        #endif
        #if (UseMySql)
                   optionsBuilder.UseMySql(conn!, GeneralUtils.GetMySqlVersion())
                   .EnableSensitiveDataLogging()
                   .LogTo(Console.WriteLine, LogLevel.Information);
        #endif
        #if (UsePostgreSql)
                    optionsBuilder.UseNpgsql(conn!)
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
        #endif
        #if( UseSqlite)
                    optionsBuilder.UseSqlite(conn!)
                    .EnableSensitiveDataLogging()
                     .LogTo(Console.WriteLine, LogLevel.Information);
        #endif


            optionsBuilder.EnableSensitiveDataLogging();


        }
        public MinCleanTemplateManagerContext(DbContextOptions<MinCleanTemplateManagerContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MinCleanTemplateManagerContext).Assembly);
        }


        public DbSet<SampleModel> SampleModels { get; private set; }



    }
}