using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Vaccination.App.CQRS.Patients.Queries.GetPatient;
using Vaccination.App.CQRS.Patients.Queries.GetPatients;
using Vaccination.Infastructure.Config.Sections;
using WebUI.Controllers.Base;
using WebUI.Inner.Constants.Routing;

namespace WebUI.Controllers
{
    [Route(PatientsRoutes.Controller)]
    public class PatientsController : BaseController<PatientsController>
    {
        public PatientsController(IServiceProvider services) 
            : base(services)
        {
        }

        [HttpGet("{page:int?}")]
        public async Task<IActionResult> Index([FromServices]IOptions<AppSettings> settings, int page = 1)
        {
            _log.LogDebug(MethodBase.GetCurrentMethod().Name);
            int itemsPerPage = settings.Value.ItemsPerPage;
            var vm = await Mediator.Send(new GetPaitientsQuery(page - 1, itemsPerPage));
            return View(vm);
        }

        [HttpGet(PatientsRoutes.Patient + "/{id:int}")]
        public async Task<IActionResult> Patient(int id)
        {
            _log.LogDebug(MethodBase.GetCurrentMethod().Name);
            var vm = await Mediator.Send(new GetPaitientQuery(id));
            return View(vm);
        }
    }
}