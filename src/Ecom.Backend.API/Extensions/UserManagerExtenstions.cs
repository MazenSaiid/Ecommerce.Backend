using Ecom.Backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.Backend.API.Extensions
{
    public static class UserManagerExtenstions
    {
        public async static Task<User> FindUserByClaimPrincipleWithAddress(
            this UserManager<User> userManager , ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(e => e.Email == email);
        }
        public async static Task<User> FindEmailByClaimPrinciple(
            this UserManager<User> userManager, ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await userManager.Users.SingleOrDefaultAsync(e => e.Email == email);
        }
    }
}
