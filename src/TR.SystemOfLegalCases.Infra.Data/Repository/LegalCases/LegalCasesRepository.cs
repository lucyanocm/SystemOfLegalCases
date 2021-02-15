using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Domain;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot.Repository;
using TR.SystemOfLegalCases.Infra.Data.Context;
using TR.SystemOfLegalCases.Infra.Data.Repository.Base;

namespace TR.SystemOfLegalCases.Infra.Data.Repository.LegalCases
{
    public class LegalCasesRepository : Repository<LegalCase>, ILegalCasesRepository
    {
        public LegalCasesRepository(SLCContext context) : base(context) { }

        public async Task<PagedList<LegalCase>> FindByAllFields(Guid? id, string casenumber, string courtname, string lawyerresponsible, 
            DateTime? initial_registrationdate, DateTime? final_registrationdate, int? page, int? pagesize, string fieldOrder)
        {
            var _query = ReturnIQueryable();

            if (id.HasValue)
                _query = _query.Where(e => e.Id == id.Value);

            if (!string.IsNullOrEmpty(casenumber))
                _query = _query.Where(c => EF.Functions.Like(c.CaseNumber, casenumber.ToScape()));

            if (!string.IsNullOrEmpty(courtname))
                _query = _query.Where(c => EF.Functions.Like(c.CourtName, courtname.ToScape()));

            if (!string.IsNullOrEmpty(lawyerresponsible))
                _query = _query.Where(c => EF.Functions.Like(c.LawyerResponsible, lawyerresponsible.ToScape()));

            if (initial_registrationdate.HasValue)
                _query = _query.Where(e => e.RegistrationDate.Date >= initial_registrationdate.Value.Date);

            if (final_registrationdate.HasValue)
                _query = _query.Where(e => e.RegistrationDate.Date <= final_registrationdate.Value.Date);

            _query = _query.OrderBy(e => e.CaseNumber);
            _query = _query.OrderByNew(fieldOrder);

            _paginated = await ReturnPaginatedList(_query, page, pagesize);

            return new PagedList<LegalCase>(_paginated, _paginated.PageIndex, _paginated.TotalPages, _paginated.PageSize, _paginated.TotalItens);
        }
    }
}
