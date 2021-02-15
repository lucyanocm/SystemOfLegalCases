using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Application.Notifications.Interfaces;
using TR.SystemOfLegalCases.Application.Services.Base.Interfaces;
using TR.SystemOfLegalCases.Application.ViewModels.Base;
using TR.SystemOfLegalCases.Domain.AbstractBaseModel;
using TR.SystemOfLegalCases.Domain.Interfaces;

namespace TR.SystemOfLegalCases.Service.Api.Controllers.Base
{
    public class BaseRegisterController<TModel, TViewModel, TViewModelAdd,
        TViewModelUpdate, TValidator> : BaseController
        where TModel : Entity, new()
        where TViewModel : BaseViewModelRegister, new()
        where TViewModelUpdate : BaseViewModelRegister, new()
        where TValidator : AbstractValidator<TModel>, new()
    {
        protected readonly IMapper _mapper;
        protected readonly IBaseRegisterService<TModel, TViewModel, TViewModelAdd, TViewModelUpdate, TValidator> _appService;
        protected readonly IRepository<TModel> _repository;

        protected virtual string _ControllerName { get; set; }
        protected virtual string _ControllerFullName { get; set; }

        public BaseRegisterController(string ControllerName, string ControllerFullName,
                                      INotifier _notifier,
                                      IMapper mapper,
                                      IBaseRegisterService<TModel, TViewModel, TViewModelAdd, TViewModelUpdate, TValidator> appService,
                                      IRepository<TModel> repository) : base(_notifier)
        {
            _ControllerFullName = ControllerFullName;
            _ControllerName = ControllerName;
            _appService = appService;
            _repository = repository;
            _mapper = mapper;
        }

        protected override void Dispose(bool disposing)
        {
            _repository?.Dispose();
        }

        public virtual async Task<IActionResult> Post([FromBody]TViewModelAdd viewmodel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            TModel model = _appService.MapDomain(viewmodel);

            await _appService.Add(model);

            if (!_notifier.HasNotification())
            {
                TModel modelReturn = await _repository.GetById(model.Id);
                TViewModel viewmodelReturn = _mapper.Map<TViewModel>(modelReturn);
                return CustomResponseAdded(string.Format("api/v1/{0}/{1}", _ControllerName, viewmodelReturn.Id), viewmodelReturn);
            }

            return CustomResponse();
        }

        public virtual async Task<IActionResult> Put(Guid id, [FromBody]TViewModelUpdate viewmodel)
        {
            if (id != viewmodel.Id)
            {
                NotifyError("Id is not the same as what was passed in the query.");
                return CustomResponse(viewmodel);
            }

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _appService.Update(viewmodel);

            TViewModel viewmodelReturn = _mapper.Map<TViewModel>(await _repository.GetById(viewmodel.Id));

            if (!_notifier.HasNotification())
            {
                return CustomResponse(viewmodelReturn);
            }

            return CustomResponse(viewmodelReturn);
        }

        public virtual async Task<IActionResult> Delete(Guid id)
        {
            TModel model = await _repository.GetById(id);

            if (model == null)
                return NotFound();

            await _appService.Remove(id);

            if (!_notifier.HasNotification())
            {
                return CustomResponse(new { Message = string.Format("{0} successfully deleted.", _ControllerFullName) });
            }

            return CustomResponse();
        }

        public virtual async Task<IActionResult> Get(Guid id)
        {
            TModel model = await _repository.GetById(id);

            if (model == null)
                return NotFound();

            return CustomResponse(_mapper.Map<TViewModel>(model));
        }
    }
}
