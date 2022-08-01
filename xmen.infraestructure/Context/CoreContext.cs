using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using xmen.common.Helpers;

namespace xmen.Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class CoreContext : DbContext, IDataContext
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Initializes a new instance of the <see cref="CoreContext" /> class.
        /// </summary>
        /// <param name="dbContextOptions">The database context options.</param>

        public CoreContext(DbContextOptions dbContextOptions, IConnectionFactory connectionFactory, IConfiguration configuration)
            : base(dbContextOptions)
        {
            _connectionFactory = connectionFactory;

            _configuration = configuration;

            //Desactivar la carga lenta de Entitie Framework
            ChangeTracker.LazyLoadingEnabled = false;

            //Desactivar consulta con seguimiento
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }


        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int Save()
        {
            try
            {
                var count = SaveChanges();
                return count;
            }
            catch
            {
                Clear();
                throw;
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public async Task<int> SaveAsync()
        {
            try
            {
                var count = await SaveChangesAsync().ConfigureAwait(false);
                return count;
            }
            catch
            {
                Clear();
                throw;
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            var clearables = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted).ToList();

            clearables.ForEach(x => x.State = EntityState.Detached);
        }

        public void RollBack()
        {
            ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //var stringConnect = "Data Source=tcp:P-JPAVON\\SQLSERVER2019;Initial Catalog=xmen_db;Persist Security Info=False;User ID=xmen_user;Password=Xmen1234-;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;";
                var stringConnect = _configuration.GetConnectionString("CnxDbContext");

                if (string.IsNullOrEmpty(_connectionFactory.SqlConnectionConfig))
                {
                    optionsBuilder.UseSqlServer(stringConnect, providerOptions => providerOptions.EnableRetryOnFailure());
                }
            }
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }


    }
}