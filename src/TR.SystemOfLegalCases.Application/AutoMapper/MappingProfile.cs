using AutoMapper;
using TR.SystemOfLegalCases.Application.ViewModels.LegalCases;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot;

namespace TR.SystemOfLegalCases.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LegalCase, LegalCaseViewModel>().ReverseMap();
            CreateMap<LegalCase, LegalCaseAddViewModel>().ReverseMap();
            CreateMap<LegalCase, LegalCaseUpdateViewModel>().ReverseMap();
        }
    }
}
