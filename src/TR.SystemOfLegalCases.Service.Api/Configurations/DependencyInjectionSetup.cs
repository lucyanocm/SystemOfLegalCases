using Microsoft.Extensions.DependencyInjection;
using System;
using TR.SystemOfLegalCases.CrossCutting.IoC;

namespace TR.SystemOfLegalCases.Service.Api.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjectionSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
