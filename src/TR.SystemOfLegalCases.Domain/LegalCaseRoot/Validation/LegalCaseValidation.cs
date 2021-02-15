using FluentValidation;

namespace TR.SystemOfLegalCases.Domain.LegalCaseRoot.Validation
{
    public class LegalCaseValidation : AbstractValidator<LegalCase>
    {
        public LegalCaseValidation()
        {
            RuleFor(c => c.CaseNumber)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(25).WithMessage("The field {PropertyName} must have 25 characters.");
                ///.Matches(@"/^[0-9]{7}-?[0-9]{2}.?[0-9]{4}.?[0-9]{1}.?[0-9]{2}.?[0-9]{4}/").WithMessage("The field {PropertyName} must be valid format like 'NNNNNNN-NN.NNNN.N.NN.NNNN'.");                

            RuleFor(c => c.CourtName)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(3, 150).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength} characters.");

            RuleFor(c => c.LawyerResponsible)
                .NotEmpty().WithMessage("The field {PropertyName} is required.")
                .Length(3, 200).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength} characters.");

            RuleFor(c => c.RegistrationDate)
                .NotEmpty().WithMessage("The field {PropertyName} is required.");
        }
    }
}
