using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using TR.SystemOfLegalCases.Infra.Data.Context;

namespace TR.SystemOfLegalCases.Service.Api.Configurations
{
    public static class DatabaseSetup
    {
        public static void AddDatabaseSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDbContext<SLCContext>(options =>
                options.UseInMemoryDatabase("TR.System.Database"));
        }
    }
}
