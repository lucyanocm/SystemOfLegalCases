using AutoMapper;
using FluentValidation;
using System;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Application.Notifications.Interfaces;
using TR.SystemOfLegalCases.Application.Services.Base.Interfaces;
using TR.SystemOfLegalCases.Application.ViewModels.Base;
using TR.SystemOfLegalCases.Domain.AbstractBaseModel;
using TR.SystemOfLegalCases.Domain.Interfaces;

namespace TR.SystemOfLegalCases.Application.Services.Base
{
    public abstract class BaseRegisterService<TModel, TViewModel, TViewModelAdd, TViewModelUpdate, TValidator>
        : BaseService,
        IBaseRegisterService<TModel, TViewModel, TViewModelAdd, TViewModelUpdate, TValidator>
        where TModel : Entity, new()
        where TValidator : AbstractValidator<TModel>, new()
        where TViewModelUpdate : BaseViewModelRegister, new()
    {
        protected readonly IRepository<TModel> _repository;
        protected readonly IMapper _mapper;
        protected string _DomainName { get; private set; }

        public BaseRegisterService(string DomainName,
                                   INotifier notifier,
                                   IRepository<TModel> repository,
                                   IMapper mapper)
            : base(notifier)
        {
            _repository = repository;
            _DomainName = DomainName;
            _mapper = mapper;
        }

        

        /// <summary>
        /// Valida model com base na nas especificações feitas com FlentValidation.
        /// </summary>
        /// <param name="model">Model a ser validado.</param>
        /// <returns>True model passou por todas validações, False model inválido.</returns>
        public virtual bool ValidateModel(TModel model)
        {            
            return RunValidation(new TValidator(), model);
        }

        /// <summary>
        /// Mapea domínio com base em um ViewModel utilizando AutoMapper.
        /// Método aceita ovverride para especificar ações.
        /// </summary>        
        /// <param name="viewmodel">ViewModel representando o Model.</param>
        /// <returns></returns>
        public virtual TModel MapDomain(TViewModelAdd viewmodel)
        {
            return _mapper.Map<TModel>(viewmodel);
        }

        /// <summary>
        /// Valida ações para adicionar um Model, utilizado para verificar caso já exista um domínio
        /// com mesma descrição ou outro campo com mesmo valor.
        /// 
        /// Caso tenha especificidade o método deve ser substituido. 
        /// </summary>
        /// <param name="model">Model a ser validado para adicionar.</param>
        /// <returns>True o model é valido para acidionar, Falso model inválido.</returns>
        public virtual bool ValidateAddModel(TModel model)
        {
            return true;
        }

        public virtual async Task<bool> Add(TModel model)
        {
            if (!ValidateModel(model))
                return false;

            if (!ValidateAddModel(model))
                return false;

            try
            {
                await _repository.Add(model);
            }
            catch (Exception ex)
            {
                Notify("It's Can't add {0}.", _DomainName);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Mapeia atualizações no Model com base em um ViewModel utilizando métodos em reflection.
        /// Somente é alterado campos com valores diferentes entre o Model e o ViewModel, marcando a flag
        /// </summary>
        /// <param name="model">Model a ser atualizado.</param>
        /// <param name="viewmodel">ViewModel com os campos e valores para a atualização.</param>
        /// <returns>True se houve alguma alteraççao no Model, False não ocorreu nenhuma atualização.</returns>
        public virtual bool MapUpdate(TModel model, TViewModelUpdate viewmodel)
        {
            return _repository.UpdateValeuWithViewModel(model, viewmodel);
        }

        public virtual async Task<bool> Update(TViewModelUpdate viewmodel)
        {
            if (!_repository.DomainExist(viewmodel.Id))
            {
                Notify("{0} does not exist.", _DomainName);
                return false;
            }

            TModel model = await _repository.GetById(viewmodel.Id);

            bool hasUpdate = MapUpdate(model, viewmodel);

            if (!hasUpdate)
            {
                Notify("There are no changes in the {0}.", _DomainName);
                return false;
            }

            if (!ValidateModel(model))
                return false;

            try
            {
                await _repository.Update(model);
                return true;
            }
            catch (Exception ex)
            {
                Notify("The record {0} could not be updated.", _DomainName);
                return false;
            }
        }

        /// <summary>
        /// Valida se é possível ser excluído um model. Utilizado para ser adicionado condições que não deixe
        /// o model ser excluído com base em regras de negocio, por exemplo: se Status = Processado não é possível excluir.        
        /// </summary>
        /// <param name="model">Model a ser validado.</param>
        /// <returns>True o model é valido para exclusão, Falso model não pode ser excluído.</returns>
        public virtual bool ValidateDeletion(TModel model)
        {
            return true;
        }

        /// <summary>
        /// Validação se há relacinamento com chave estrangeira do Model com outro que dependa dele.
        /// Utilizado para verificar as relações no momento da exclusão.
        /// </summary>
        /// <param name="id">Id do Model a ser verificado.</param>
        /// <returns>True não existe relacinamento estrageiro, Falso model tem relacionamento.</returns>
        public virtual bool ValidateModelWithoutRelationships(Guid id)
        {
            return true;
        }

        public virtual async Task<bool> Remove(Guid id)
        {
            if (!_repository.DomainExist(id))
            {
                Notify("{0} does not exist.", _DomainName);
                return false;
            }

            TModel model = await _repository.GetById(id);

            if (!ValidateDeletion(model))
            {
                return false;
            }

            if (!ValidateModelWithoutRelationships(id))
            {
                Notify("It was not possible to delete {0} because there is a related record.", _DomainName);
                return false;
            }

            try
            {
                await _repository.Remove(id);
                return true;
            }
            catch (Exception ex)
            {
                Notify("It's cannot delete {0}.", _DomainName);
                return false;
            }
        }
    }
}
