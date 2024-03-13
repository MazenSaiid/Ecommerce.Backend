using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Interfaces;
using Ecom.Backend.Core.Services;
using Ecom.Backend.InfraStructure.Data.Configurations;
using Ecom.Backend.InfraStructure.Data.Context;
using Ecom.Backend.InfraStructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.InfraStructure.Registrations
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection InfraStructureConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Configure Services 

            services.AddScoped<ITokenService,TokenServices>();
            services.AddScoped<IPaymentServices, PaymentServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Application DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),builder =>
                {
                    builder.EnableRetryOnFailure(5,TimeSpan.FromSeconds(10),null);
                });
            });

            //configure identity
            services.AddIdentity<User,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().
                AddDefaultTokenProviders();

            services.AddMemoryCache();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]))
                    };
                });
            return services;
        }
        public static async void InfraStructureConfigMiddleware(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                await IdentitySeed.SeedUserAsync(userManager);
            }
        }
    }
}
