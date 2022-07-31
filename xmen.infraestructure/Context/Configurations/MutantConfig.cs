using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xmen.infraestructure.Entities;
using xmen.Infrastructure.Context.Configurations.Base;

namespace xmen.infraestructure.Context.Configurations
{
    [Obsolete]
    [ExcludeFromCodeCoverage]
    public class MutantConfig : BaseEntityConfig<Mutant>
    {
        public override void Configure(EntityTypeBuilder<Mutant> builder)
        {
            builder.ToTable("TBL_Mutant");

            builder.HasKey(e => e.Id).HasName("PK_TBL_Mutant");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.Adn)
                .HasColumnName("And")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(e => e.Humans)
                .HasColumnName("Humans")
                .IsRequired();

            builder.Property(e => e.Mutants)
                .HasColumnName("Mutants")
                .IsRequired();


            builder.Property(e => e.Radio)
                .HasColumnType("numeric(18,2)")
                .HasColumnName("Radio")
                .IsRequired();

            base.Configure(builder);
        }
    }
}
