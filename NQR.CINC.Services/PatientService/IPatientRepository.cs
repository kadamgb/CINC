using NQR.CINC.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQR.CINC.Services.PatientService
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
    }
}
