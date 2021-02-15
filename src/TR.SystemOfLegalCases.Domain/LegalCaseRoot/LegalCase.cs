using System;
using TR.SystemOfLegalCases.Domain.AbstractBaseModel;

namespace TR.SystemOfLegalCases.Domain.LegalCaseRoot
{
    public class LegalCase : Entity
    {
        public string CaseNumber { get; set; }
        public string CourtName { get; set; }
        public string LawyerResponsible { get; set; }
        public DateTime RegistrationDate { get; set; }

        public LegalCase()
        {
            RegistrationDate = DateTime.Now.Date;
        }

        public LegalCase(string caseNumber, string courtName, string lawyerResponsible, DateTime registrationDate)
        {
            CaseNumber = caseNumber;
            CourtName = courtName;
            LawyerResponsible = lawyerResponsible;
            RegistrationDate = registrationDate;
        }
    }
}
