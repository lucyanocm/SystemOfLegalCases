using System;
using System.ComponentModel.DataAnnotations;
using TR.SystemOfLegalCases.Application.ViewModels.Base;

namespace TR.SystemOfLegalCases.Application.ViewModels.LegalCases
{
    public class LegalCaseUpdateViewModel : BaseViewModelRegister
    {
        [Required(ErrorMessage = "Id is required.")]
        public override Guid Id { get; set; }

        [Required(ErrorMessage = "CaseNumber is required.")]
        public string CaseNumber { get; set; }

        [Required(ErrorMessage = "CourtName is required.")]
        public string CourtName { get; set; }

        [Required(ErrorMessage = "LawyerResponsible is required.")]
        public string LawyerResponsible { get; set; }
    }
}
