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
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.HasData(
                new DeliveryMethod {Id=1, Name = "DHL", Description = "Fastest Delivery Time", DeliveryTime = "One Day", Price = 20},
                new DeliveryMethod { Id = 2, Name = "Aramex", Description = "Get it within 3 days", DeliveryTime = "3 Days", Price = 10 },
                new DeliveryMethod { Id = 3, Name = "Fedex", Description = "Slower but cheap", DeliveryTime = "Five -Seven Days", Price = 5 },
                new DeliveryMethod { Id = 4, Name = "Jumia", Description = "Free", DeliveryTime = "10 Days", Price = 0 }); 
        }
    }
}
