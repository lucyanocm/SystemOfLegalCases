using System;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Application.ViewModels.Base;
using TR.SystemOfLegalCases.Domain.AbstractBaseModel;

namespace TR.SystemOfLegalCases.Application.Interfaces.Base
{
    public interface IServiceRegisterBase<TModel, TViewModelUpdate> : IDisposable
        where TModel : Entity, new()
        where TViewModelUpdate : BaseViewModelRegister, new()
    {
        Task<bool> Add(TModel model);
        Task<bool> Update(TViewModelUpdate viewmodel);
        Task<bool> Remove(Guid id);
    }
}
