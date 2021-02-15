using System;
using TR.SystemOfLegalCases.Application.Services.Base.Interfaces;
using TR.SystemOfLegalCases.Application.ViewModels.LegalCases;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot.Validation;

namespace TR.SystemOfLegalCases.Application.Interfaces.LegalCases
{
    public interface ILegalCaseService : IBaseRegisterService<LegalCase,
        LegalCaseViewModel,
        LegalCaseAddViewModel,
        LegalCaseUpdateViewModel,
        LegalCaseValidation>,
        IDisposable
    {

    }
}
