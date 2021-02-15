using AutoMapper;
using System.Linq;
using TR.SystemOfLegalCases.Application.Interfaces.LegalCases;
using TR.SystemOfLegalCases.Application.Notifications.Interfaces;
using TR.SystemOfLegalCases.Application.Services.Base;
using TR.SystemOfLegalCases.Application.ViewModels.LegalCases;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot.Repository;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot.Validation;

namespace TR.SystemOfLegalCases.Application.Services.LegalCases
{
    public class LegalCaseService : BaseRegisterService<LegalCase,
        LegalCaseViewModel,
        LegalCaseAddViewModel,
        LegalCaseUpdateViewModel,
        LegalCaseValidation>,
        ILegalCaseService
    {
        public LegalCaseService(ILegalCasesRepository repository,
                                INotifier notifier,
                                IMapper mapper)
            : base("LegalCase", notifier, repository, mapper)
        {
     
        }

        public override bool ValidateAddModel(LegalCase model)
        {
            if (_repository.Find(l => l.CaseNumber.Equals(model.CaseNumber)).Result.Any())
            {
                Notify("The legal case number already exists.");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            
        }
    }
}
