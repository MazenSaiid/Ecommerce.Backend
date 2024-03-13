using Ecom.Backend.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.InfraStructure.Data.Configurations
{
    public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(o => o.ProductItemsOrdered, n => { n.WithOwner(); });

            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        }
    }
}
