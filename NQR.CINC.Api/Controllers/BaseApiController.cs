
using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NQR.CINC.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        private const string _unitOfWorkKey = "UnitOfWork";
        protected UnitOfWork UnitOfWork { get; set; }
        public BaseApiController(UnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            //Save UnitOfWork instance in HTTPContext, so that it can be used thru out the application
            AppContext.AddItem(_unitOfWorkKey, unitOfWork);
        }
    }
}
