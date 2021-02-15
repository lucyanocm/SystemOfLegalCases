using Microsoft.Extensions.DependencyInjection;
using TR.SystemOfLegalCases.Application.Interfaces.LegalCases;
using TR.SystemOfLegalCases.Application.Notifications;
using TR.SystemOfLegalCases.Application.Notifications.Interfaces;
using TR.SystemOfLegalCases.Application.Services.LegalCases;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot.Repository;
using TR.SystemOfLegalCases.Infra.Data.Context;
using TR.SystemOfLegalCases.Infra.Data.Repository.LegalCases;

namespace TR.SystemOfLegalCases.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<SLCContext>();

            services.AddScoped<INotifier, Notifier>();

            services.AddScoped<ILegalCaseService, LegalCaseService>();
            services.AddScoped<ILegalCasesRepository, LegalCasesRepository>();
        }
    }
}
