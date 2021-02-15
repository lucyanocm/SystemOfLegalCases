using System.ComponentModel.DataAnnotations;

namespace TR.SystemOfLegalCases.Application.ViewModels.LegalCases
{
    public class LegalCaseAddViewModel
    {
        [Required(ErrorMessage = "CaseNumber is required.")]
        public string CaseNumber { get; set; }

        [Required(ErrorMessage = "CourtName is required.")]
        public string CourtName { get; set; }

        [Required(ErrorMessage = "LawyerResponsible is required.")]
        public string LawyerResponsible { get; set; }
    }
}
