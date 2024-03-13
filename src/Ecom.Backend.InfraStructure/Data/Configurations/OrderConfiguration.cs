using Ecom.Backend.Core.Entities;
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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShippingAddress, n => { n.WithOwner(); });

            builder.Property(x => x.OrderStatus).HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
                );

            builder.HasMany(o=>o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");


        }
    }
}
