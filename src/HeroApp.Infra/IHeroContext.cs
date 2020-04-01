using HeroApp.Domain;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HeroApp.Infra
{
    public interface IHeroContext
    {
        DbSet<Ong> Ongs { get; set; }
        DbSet<Incident> Incidents { get; set; }




     

      

        int SaveChanges();

 
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();


         Task CommitTransactionAsync();


         void RollbackTransaction();
       
    }



}