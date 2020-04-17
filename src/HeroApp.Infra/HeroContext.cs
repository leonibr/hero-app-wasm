using HeroApp.Domain;
using HeroApp.Domain.Interfaces;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Options;
using Marques.EFCore.SnakeCase;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HeroApp.Infra
{
    public class HeroContext: IdentityDbContext<AppUser, AppRole, string>, IHeroContext
    {
        private IDbContextTransaction _currentTransaction;

        public DbSet<Ong> Ongs { get; set; }
        public DbSet<Incident> Incidents { get; set; }

        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public HeroContext(
            DbContextOptions<HeroContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        //public HeroContext(DbContextOptions<HeroContext> options)
        //: base(options)
        //{
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ong>(table =>
            {
                table.HasKey(p => p.Id);
                table.HasOne(p => p.Owner)
                    .WithMany(c => c.MyOngs)
                    .HasForeignKey(c => c.AppUserId);
            });


            modelBuilder.Entity<Incident>(table =>
            {
                table.HasKey(p => p.Id);
                table.HasOne(p => p.Ong)
                    .WithMany(p => p.Incidents)
                    .HasForeignKey(f => f.Ong_Id);
            });

            base.OnModelCreating(modelBuilder);
            modelBuilder.ToSnakeCase();
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }


    }
}
