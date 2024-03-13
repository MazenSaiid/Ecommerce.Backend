using Ecom.Backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.InfraStructure.Data.Configurations
{
    public class IdentitySeed
    {
        public static async Task SeedUserAsync( UserManager<User> userManager)
        {
            if(!userManager.Users.Any())
            {
                //create new user
                var user = new User
                {
                    DisplayName = "Mazen",
                    Email = "mazen@mazen.com",
                    UserName = "mazen@mazen.com",
                    Address = new Address
                    {
                        FirstName = "mazen",
                        LastName = "mohamed",
                        Street = "Sheikh Zayed",
                        City = "Giza",
                        State = "Egypt",
                        ZipCode = "123"
                    }
                };
                await userManager.CreateAsync(user,"Pass@123");
            }
        }
    }
}
