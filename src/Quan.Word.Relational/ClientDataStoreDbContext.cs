using Microsoft.EntityFrameworkCore;
using Quan.Word.Core;

namespace Quan.Word.Relational
{
    /// <summary>
    /// The database context for the client data store
    /// </summary>
    public class ClientDataStoreDbContext : DbContext
    {
        #region DbSets

        /// <summary>
        /// The client login credentials
        /// </summary>
        public DbSet<UserProfileDetailsApiModel> LoginCredentials { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ClientDataStoreDbContext(DbContextOptions<ClientDataStoreDbContext> options) : base(options) { }

        #endregion

        #region Model Creating

        /// <summary>
        /// Configures the database structure and relationships
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API

            // Configure LoginCredentials
            // --------------------------
            //
            // Set Id as primary key
            modelBuilder.Entity<UserProfileDetailsApiModel>().HasKey(a => a.Id);


            // TODO: Set up limits
            //modelBuilder.Entity<UserProfileDetailsApiModel>().Property(a => a.Firstname).HasMaxLength(50);
        }

        #endregion
    }
}
