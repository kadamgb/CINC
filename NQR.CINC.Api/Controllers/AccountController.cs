using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Entities.Models;
using NQR.CINC.Services.Infrastructure;
using NQR.CINC.Services.UserService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace NQR.CINC.Api.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        //private AuthRepository _repo = null;
        private IUserRepository _userRepository { get; set; }

        public AccountController(UnitOfWork unitOfwork) : base(unitOfwork)
        {
            _userRepository = unitOfwork.UserRepositoryInstance;
        }       
        [Authorize]
        [Route("all")]
        public IHttpActionResult Get()
        {
            List<User> users = _userRepository.GetAllUsers();
            //return Ok( new string[] { "value1", "value2" });
            return Ok(users);
        }
        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register(UserRegistrationModel userRegistrationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = new User()
            {
                UserName = userRegistrationModel.UserName,
                Password = userRegistrationModel.Password,
                Email = userRegistrationModel.Email,
                Phone = userRegistrationModel.Phone
            };
            User result = _userRepository.RegisterUser(user);
            UnitOfWork.Save();
           

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        

        private IHttpActionResult GetErrorResult(User result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result == null && result.Id < 0)
            {
                //if (result.Errors != null)
                //{
                //    foreach (string error in result.Errors)
                //    {
                ModelState.AddModelError("", "Error occured!");
                //    }
                //}

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
