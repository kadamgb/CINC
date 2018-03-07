using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQR.CINC.Services.PatientService
{
    
    public class PatientRepository:BaseRepository,IPatientRepository
    {
        public List<Patient>GetAllPatients()
        {
            List<Patient> allPatients = new List<Patient>();
            allPatients = UnitOfWorkInstance.PatientEntity.Get().ToList();
            return allPatients;
        }
    }
}
