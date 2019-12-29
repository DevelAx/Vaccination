using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Vaccination.App.CQRS;
using Vaccination.App.CQRS.Patients.Commands.CreatePatient;
using Vaccination.App.CQRS.Patients.Commands.UpdatePatient;
using Vaccination.App.CQRS.Patients.Queries.FindPatients;
using Vaccination.App.CQRS.Patients.Queries.GetAllVaccines;
using Vaccination.App.CQRS.Patients.Queries.GetEditPaitient;
using Vaccination.App.CQRS.Patients.Queries.GetPatient;
using WebUI.Controllers.Base;
using WebUI.Inner.Constants.Routing;

namespace WebUI.Controllers
{
    [Route(PatientsRoutes.controller)]
    public class PatientsController : BaseController<PatientsController>
    {
        public PatientsController(IServiceProvider services) 
            : base(services)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet(PatientsRoutes.search)]
        public async Task<IActionResult> PatientsList(string fullName, string insuranceNumber, int searchId, int page = 1)
        {
            var vm = await Mediator.Send(new FindPatientsQuery(fullName, insuranceNumber, searchId, page));
            return Json(vm);
        }

        [HttpGet(PatientsRoutes.patient + "/{id}")]
        public async Task<IActionResult> Patient(Guid id)
        {
            var result = await Mediator.Send(new PaitientQuery(id));
            return View(result);
        }

		#region Create patient

		[HttpGet(PatientsRoutes.create)]
        public IActionResult CreatePatient()
        {
            //var vm = new CreatePatientVM
            //{

            //};
            return View();
        }

        [HttpPost(PatientsRoutes.create)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatient(CreatePatientVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await Mediator.Send(new CreatePatientCommand(vm));
            ProcessResultBeforeRedirect(result);

            if (result.HasError)
                return View(vm);

            vm = result.TypedData;
            return Redirect(Url.Action(PatientsRoutes.patient, PatientsRoutes.controller, new { vm.Id }));
        }

        #endregion Create patient
        #region Edit patient

        [HttpGet(PatientsRoutes.edit + "/{id}")]
        public async Task<IActionResult> EditPatient(Guid id)
        {
            var result = await Mediator.Send(new EditPaitientQuery(id));
            return View(result);
        }

        [HttpPost(PatientsRoutes.edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(EditPatientVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllVaccines = await Mediator.Send(new AllVaccinesQuery());
                return View(vm);
            }
                
            var result = await Mediator.Send(new UpdatePatientCommand(vm));
            ProcessResultBeforeRedirect(result);
            return Redirect(Url.Action(PatientsRoutes.patient, PatientsRoutes.controller, new { vm.Id }));
        }

		#endregion Edit patient
		#region Helpers

		private void ProcessResultBeforeRedirect(RequestResult result)
        {
            if (result.HasError)
                TempData["error-info"] = result.Error.Message;
            else
                TempData["success-info"] = "Данные сохранены";
        }

		#endregion Helpers
	}
}