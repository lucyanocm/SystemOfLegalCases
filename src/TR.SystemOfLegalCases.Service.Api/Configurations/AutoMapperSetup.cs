using Microsoft.Extensions.DependencyInjection;
using System;
using TR.SystemOfLegalCases.Application.AutoMapper;

namespace TR.SystemOfLegalCases.Service.Api.Configurations
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
