using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Services.Infrastructure;
using NQR.CINC.Services.UserService;
using System.Collections.Generic;
using System.Web.Http;

namespace NQR.CINC.Api.Controllers
{
    [RoutePrefix("api/Values")]
    public class ValuesController :  BaseApiController
    {
        private IUserRepository _userRepository;
        public ValuesController(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _userRepository = unitOfWork.UserRepositoryInstance;
        }
        // GET api/values
        [Authorize]
        [Route("all")]
        public IEnumerable<string> Get()
        {
            List<User> users = _userRepository.GetAllUsers();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
