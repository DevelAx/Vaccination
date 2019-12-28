using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
            var result = await Mediator.Send(new FindPatientsQuery(fullName, insuranceNumber, searchId, page));
            return Json(result.Data);
        }

        [HttpGet(PatientsRoutes.patient + "/{id}")]
        public async Task<IActionResult> Patient(Guid id)
        {
            var result = await Mediator.Send(new PaitientQuery(id));
            return View(result.Data);
        }

        [HttpGet(PatientsRoutes.edit + "/{id}")]
        public async Task<IActionResult> EditPatient(Guid id)
        {
            var result = await Mediator.Send(new EditPaitientQuery(id));
            return View(result.Data);
        }

        [HttpPost(PatientsRoutes.edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(EditPatientVM vm)
        {
            if (!ModelState.IsValid)
            {
                var result = await Mediator.Send(new AllVaccinesQuery());

                //if (result.HasError)
                //    return MyView(result);

                vm.AllVaccines = result.Data;
                return View(vm);
            }
                
            await Mediator.Send(new UpdatePatientCommand(vm));
            return Redirect(Url.Action(PatientsRoutes.patient, PatientsRoutes.controller, new { vm.Id }));
        }
    }
}