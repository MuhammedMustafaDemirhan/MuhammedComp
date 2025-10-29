using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuhammedCo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Repository.Configurations
{
    public class GroupInRoleConfiguration : IEntityTypeConfiguration<GroupInRole>
    {
        public void Configure(EntityTypeBuilder<GroupInRole> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Status).HasDefaultValue(true);

            builder.HasQueryFilter(x => x.Status);
        }
    }
}
