using NQR.CINC.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQR.CINC.Services.Infrastructure
{
    public class BaseRepository : IDisposable
    {
        private const string _unitOfWorkKey = "UnitOfWork";

        public UnitOfWork UnitOfWorkInstance
        {
            get
            {
                //Try to get the database connection (dbcontext) from the AppContext dictionary
               // UnitOfWork unitOfWork = AppContext.GetItem(_unitOfWorkKey) as UnitOfWork;
               UnitOfWork unitOfWork= NQR.CINC.Entities.EntityFramework.AppContext.GetItem(_unitOfWorkKey) as UnitOfWork;

                ////If not present in the dictionary
                if (unitOfWork == null)
                {
                    //create a new connection
                    unitOfWork = new UnitOfWork();
                    //add the connection to the AppContext dictionary so that the same connection can be reused by different repositories for the entire http request
                    NQR.CINC.Entities.EntityFramework.AppContext.AddItem(_unitOfWorkKey, unitOfWork);
                }

                return unitOfWork;
            }
        }
        public void Dispose()
        {

        }

    }
}
