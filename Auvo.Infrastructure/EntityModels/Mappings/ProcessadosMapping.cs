using Auvo.Infrastructure.EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auvo.Infrastructure.EntityModels.Mappings
{
    public class ProcessadosMapping : IEntityTypeConfiguration<Processados>
    {
        public void Configure(EntityTypeBuilder<Processados> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.PastaEntrada)
            .IsRequired()
            .HasColumnType("varchar(300)");

            builder.Property(c => c.PastaSaida)
            .IsRequired()
            .HasColumnType("varchar(300)");

            builder.ToTable("PROCESSADOS");
        }
    }
}
