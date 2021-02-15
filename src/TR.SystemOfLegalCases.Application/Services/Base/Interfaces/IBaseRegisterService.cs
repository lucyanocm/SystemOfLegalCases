using FluentValidation;
using System;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Application.ViewModels.Base;
using TR.SystemOfLegalCases.Domain.AbstractBaseModel;

namespace TR.SystemOfLegalCases.Application.Services.Base.Interfaces
{
    public interface IBaseRegisterService<TModel, TViewModel, TViewModelAdd, TViewModelUpdate, TValidator>
        where TModel : Entity, new()
        where TValidator : AbstractValidator<TModel>, new()
        where TViewModelUpdate : BaseViewModelRegister, new()
    {
        bool ValidateModel(TModel model);
        TModel MapDomain(TViewModelAdd viewmodel);
        Task<bool> Add(TModel model);
        Task<bool> Update(TViewModelUpdate viewmodel);
        Task<bool> Remove(Guid id);
    }
}
