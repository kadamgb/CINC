using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Services.Infrastructure;
using NQR.CINC.Services.PatientService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NQR.CINC.Api.Controllers
{
    [RoutePrefix("api/Patient")]
    public class PatientController : BaseApiController
    {
        protected IPatientRepository _patientRepository;
        public PatientController(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._patientRepository = unitOfWork.PatientRepositoryInstance;
        }
        //[Authorize]
        [Route("all")]
        public IHttpActionResult Get()
        {
            List<Patient> patients = _patientRepository.GetAllPatients();
            return Ok(patients);
        }
    }
}
