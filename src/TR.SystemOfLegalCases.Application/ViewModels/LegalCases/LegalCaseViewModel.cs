using System;
using TR.SystemOfLegalCases.Application.ViewModels.Base;

namespace TR.SystemOfLegalCases.Application.ViewModels.LegalCases
{
    public class LegalCaseViewModel : BaseViewModelRegister
    {
        public string CaseNumber { get; set; }
        public string CourtName { get; set; }
        public string LawyerResponsible { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
