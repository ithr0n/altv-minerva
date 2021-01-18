using Microsoft.EntityFrameworkCore;
using Minerva.Server.Core.Contracts.Models.Database;

namespace Minerva.Server.DataAccessLayer.Context
{
    public class DatabaseContext
        : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        #region Entities

        public DbSet<Account> Accounts { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountCharacterAccess> BankAccountCharacterAccesses { get; set; }
        public DbSet<BankAccountGroupAccess> BankAccountGroupAccesses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemData> ItemData { get; set; }
        public DbSet<ItemDefinition> ItemDefinitions { get; set; }
        public DbSet<NpcShop> NpcShops { get; set; }
        public DbSet<NpcShopEntry> NpcShopEntries { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // combined primary key can only be configured here
            modelBuilder.Entity<BankAccountCharacterAccess>()
                .HasKey(c => new { c.BankAccountId, c.CharacterId });

            modelBuilder.Entity<BankAccountGroupAccess>()
                .HasKey(c => new { c.BankAccountId, c.GroupId });

            modelBuilder.Entity<GroupMember>()
                .HasKey(c => new { c.GroupId, c.CharacterId });

            modelBuilder.Entity<ItemData>()
                .HasKey(c => new { c.ItemId, c.Key });

            modelBuilder.Entity<NpcShopEntry>()
                .HasKey(c => new { c.NpcShopId, c.ItemDefinitionId, c.Type });
        }
    }
}
