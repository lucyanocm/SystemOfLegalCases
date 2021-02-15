using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TR.SystemOfLegalCases.Application.Interfaces.LegalCases;
using TR.SystemOfLegalCases.Application.Notifications.Interfaces;
using TR.SystemOfLegalCases.Application.ViewModels.LegalCases;
using TR.SystemOfLegalCases.Domain;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot.Repository;
using TR.SystemOfLegalCases.Domain.LegalCaseRoot.Validation;
using TR.SystemOfLegalCases.Service.Api.Controllers.Base;

namespace TR.SystemOfLegalCases.Service.Api.V1.LegalCases
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/legalcase")]
    [ApiController]
    public class LegalCaseController : BaseRegisterController<LegalCase, LegalCaseViewModel, LegalCaseAddViewModel, LegalCaseUpdateViewModel, LegalCaseValidation>
    {
        private readonly ILegalCasesRepository _repository;

        public LegalCaseController(INotifier notifier,
                                   IMapper mapper,
                                   ILegalCasesRepository repository,
                                   ILegalCaseService appService)
        : base("legalcase", "Legal Case", notifier, mapper, appService, repository)
        {
            _repository = repository;
        }

        protected override void Dispose(bool disposing)
        {
            _repository?.Dispose();
        }

        /// <summary>
        /// Add a new Legal Case.
        /// </summary>
        /// <param name="viewmodel">Legal Case fields information.</param>
        /// <returns></returns>
        /// <remarks>
        /// Request example
        /// 
        ///     POST /api/v1/legalcase
        ///     {
        ///         "caseNumber" : "The case number according to the National Council of Justice (CNJ) standard. It has the format: NNNNNNN-NN.NNNN.N.NN.NNNN, where N can be any positive integer.",
        ///         "courtName" : "That represents the name of the court of origin of the case. Example: Supreme Federal Court.",
        ///         "lawyerResponsible" : "Representing the name of the lawyer responsible for the case."
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">New Legal Case created.</response>
        /// <response code="400">Unable create Legal Case record.</response>
        [HttpPost]
        [ProducesResponseType(typeof(LegalCaseViewModel), 201)]
        [ProducesResponseType(typeof(ResponseView), 400)]
        public async override Task<IActionResult> Post([FromBody] LegalCaseAddViewModel viewmodel)
        {
            return await base.Post(viewmodel);
        }

        /// <summary>
        /// Updates a Legal Case record.
        /// </summary>
        /// <param name="id">Legal Case Identifier (guid).</param>
        /// <param name="viewmodel">Legal Case to update fields information.</param>
        /// <returns>Legal Case updated.</returns>
        /// <remarks>
        /// Request example
        /// 
        ///     PUT /api/v1/legalcase/00000000-0000-0000-0000-000000000000
        ///     {
        ///         "id" : "00000000-0000-0000-0000-000000000000",
        ///         "caseNumber" : "The case number according to the National Council of Justice (CNJ) standard. It has the format: NNNNNNN-NN.NNNN.N.NN.NNNN, where N can be any positive integer.",
        ///         "courtName" : "That represents the name of the court of origin of the case. Example: Supreme Federal Court.",
        ///         "lawyerResponsible" : "Representing the name of the lawyer responsible for the case."
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Legal Case updated.</response>
        /// <response code="400">Unable update Legal Case record.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(LegalCaseViewModel), 200)]
        [ProducesResponseType(typeof(ResponseView), 400)]
        public async override Task<IActionResult> Put(Guid id, [FromBody] LegalCaseUpdateViewModel viewmodel)
        {
            return await base.Put(id, viewmodel);
        }

        /// <summary>
        /// Delete Legal Case record.
        /// </summary>
        /// <param name="id">Legal Case Identifier (guid).</param>
        /// <returns>Return success of the operation.</returns>
        /// <response code="200">Record deleted successfully.</response>
        /// <response code="400">The record could not be deleted.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ResponseView), 400)]
        public async override Task<IActionResult> Delete(Guid id)
        {
            return await base.Delete(id);
        }

        /// <summary>
        /// Get Legal Case By Id.
        /// </summary>
        /// <param name="id">Legal Case Identifier (guid).</param>
        /// <returns>Legal Case record.</returns>
        /// <response code="200">Legal Case record.</response>
        /// <response code="404">Legal Case record cannot be found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LegalCaseViewModel), 200)]
        [ProducesResponseType(typeof(ResponseView), 404)]
        public async override Task<IActionResult> Get(Guid id)
        {
            return await base.Get(id);
        }

        /// <summary>
        /// Find Legal Case by all fields.
        /// </summary>
        /// <param name="id">Legal Case Identifier (guid).</param>
        /// <param name="casenumber">The case number according to the National Council of Justice (CNJ) standard. It has the format: NNNNNNN-NN.NNNN.N.NN.NNNN, where N can be any positive integer.</param>
        /// <param name="courtname">That represents the name of the court of origin of the case. Example: Supreme Federal Court.</param>
        /// <param name="lawyerresponsible">Representing the name of the lawyer responsible for the case.</param>
        /// <param name="initial_registrationdate">(Initial date filter) Date on which the case was registered in the system.</param>
        /// <param name="final_registrationdate">(Final date filter) Date on which the case was registered in the system.</param>
        /// <param name="page">Page.</param>
        /// <param name="pagesize">Page size.</param>
        /// <param name="fieldOrder">Field for ordering: CaseNumber, CourtName, LawyerResponsible and RegistrationDate. Add DESC after field name for reverse order.</param>
        /// <returns>Paged List of Legal Case.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<LegalCaseViewModel>), 200)]
        public async Task<IActionResult> FindByAllFields([FromQuery]Guid? id,
            [FromQuery]string casenumber, 
            [FromQuery]string courtname,
            [FromQuery]string lawyerresponsible,
            [FromQuery]DateTime? initial_registrationdate,
            [FromQuery]DateTime? final_registrationdate,
            [FromQuery]int? page,
            [FromQuery]int? pagesize,
            [FromQuery]string fieldOrder)
        {
            var models = await _repository.FindByAllFields(id, casenumber, courtname, lawyerresponsible,
                initial_registrationdate, final_registrationdate, page, pagesize, fieldOrder);

            PagedList<LegalCaseViewModel> viewmodelsReturn = new PagedList<LegalCaseViewModel>(
                _mapper.Map<List<LegalCaseViewModel>>(models.ListReturn),
                models.CurrentPage,
                models.TotalPages,
                models.SizePage,
                models.TotalItems);

            return CustomResponse(viewmodelsReturn);
        }
    }
}
