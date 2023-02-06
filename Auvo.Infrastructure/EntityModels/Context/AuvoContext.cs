using Auvo.Infrastructure.EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Infrastructure.EntityModels.Context
{
    public class AuvoContext :DbContext
    {

        public AuvoContext(DbContextOptions options) : base(options) { }

        public DbSet<Processados> Processados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //se esquecer de mapear algum campo do tipo string no banco vira como varchar(100) como default, e nao varchar(max)
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                    .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuvoContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            base.OnModelCreating(modelBuilder);
        }


    }
}
