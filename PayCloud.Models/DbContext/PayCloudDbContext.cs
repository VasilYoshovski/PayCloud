using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayCloud.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Data.DbContext
{
    public class PayCloudDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PayCloudUser> PayCloudUsers { get; set; }
        public DbSet<PayCloudAdmin> PayCloudAdmins { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<PayCloudUserAccount> UsersAccounts { get; set; }
        public DbSet<PayCloudUserClient> UsersClients { get; set; }

        public PayCloudDbContext()
        {
        }

        public PayCloudDbContext(DbContextOptions<PayCloudDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                throw new ArgumentException("Database builder is not configured!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relations
            modelBuilder.Entity<PayCloudUserAccount>().HasKey(x => new { x.PayCloudUserId, x.AccountId });
            modelBuilder.Entity<PayCloudUserClient>().HasKey(x => new { x.PayCloudUserId, x.ClientId });

            modelBuilder.Entity<PayCloudUserClient>().HasOne<PayCloudUser>(pcu => pcu.PayCloudUser).WithMany(p => p.UserClients);
            modelBuilder.Entity<PayCloudUserClient>().HasOne<Client>(pcu => pcu.Client).WithMany(c => c.ClientUsers);

            modelBuilder.Entity<PayCloudUserAccount>().HasOne<PayCloudUser>(pcu => pcu.PayCloudUser).WithMany(p => p.UserAccounts);
            modelBuilder.Entity<PayCloudUserAccount>().HasOne<Account>(pcu => pcu.Account).WithMany(a => a.AccountUsers);

            modelBuilder.Entity<Transaction>().HasOne<Account>(tr => tr.SenderAccount).WithMany(a => a.SentTransactions).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne<Account>(tr => tr.ReceiverAccount).WithMany(a => a.ReciveTransactions).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne<PayCloudUser>(tr => tr.CreatedByUser).WithMany(t => t.UserTransactions).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>().HasOne<Client>(acc => acc.Client).WithMany(c => c.Accounts);

            //Constrains
            modelBuilder.Entity<Account>().HasIndex(acc => acc.AccountNumber).IsUnique();
            modelBuilder.Entity<Client>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<PayCloudUser>().HasIndex(c => c.Username).IsUnique();
            modelBuilder.Entity<PayCloudAdmin>().HasIndex(c => c.Username).IsUnique();



        }
    }
}
