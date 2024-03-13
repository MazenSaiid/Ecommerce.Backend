using Ecom.Backend.API.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System.Reflection;

namespace Ecom.Backend.API.Extensions
{
    public static class APIRegistration
    {
        public static IServiceCollection AddAPIRegistration(this IServiceCollection services)
        {
            //Configure AutoMapper

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Configure IFileProvider

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
                ));

            //Configure Controller
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorResponse = new ValidationError
                    {
                        Errors = context.ModelState.Where(x => x.Value
                        .Errors.Count > 0).SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToArray()
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            //Configure and Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
