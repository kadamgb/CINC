using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Services.Infrastructure;
using System.Web.Mvc;

namespace NQR.CINC.Web.Controllers
{
    public class BaseMvcController : Controller
    {
        private const string _unitOfWorkKey = "UnitOfWork";
        protected UnitOfWork UnitOfWork { get; set; }
        public BaseMvcController(UnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            //Save UnitOfWork instance in HTTPContext, so that it can be used through out the application
            AppContext.AddItem(_unitOfWorkKey, unitOfWork);
        }
    }
}
