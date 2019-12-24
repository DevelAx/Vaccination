using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Vaccination.App.CQRS.Patients.Queries.FindPatients;
using Vaccination.App.CQRS.Patients.Queries.GetPatient;
using Vaccination.Infastructure.Config.Sections;
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
            _log.LogDebug(MethodBase.GetCurrentMethod().Name);
            return View();
        }

        [HttpGet(PatientsRoutes.search)]
        public async Task<IActionResult> PatientsList([FromServices]IOptions<AppSettings> settings, string fullName, string insuranceNumber, int searchId, int page = 1)
        {
            var vm = await Mediator.Send(new FindPatientsQuery(fullName, insuranceNumber, searchId, page));
            return Json(vm);
        }

        [HttpGet(PatientsRoutes.patient + "/{id:int}")]
        public async Task<IActionResult> Patient(int id)
        {
            _log.LogDebug(MethodBase.GetCurrentMethod().Name);
            var vm = await Mediator.Send(new GetPaitientQuery(id));
            return View(vm);
        }
    }
}