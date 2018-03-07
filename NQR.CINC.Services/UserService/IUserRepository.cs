using NQR.CINC.Entities.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NQR.CINC.Services.UserService
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User RegisterUser(User user);
        User GetUserByName(string userName);
        User GetUserByEmail(string email);
        List<string> GetUserRoles(string email);
    }
}
