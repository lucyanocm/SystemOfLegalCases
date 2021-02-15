using System;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Domain.Interfaces;

namespace TR.SystemOfLegalCases.Domain.LegalCaseRoot.Repository
{
    public interface ILegalCasesRepository : IRepository<LegalCase>
    {
        Task<PagedList<LegalCase>> FindByAllFields(Guid? id, string casenumber, string courtname, string lawyerresponsible,
            DateTime? initial_registrationdate, DateTime? final_registrationdate, int? page, int? pagesize, string fieldOrder);
    }
}
