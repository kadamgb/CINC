using NQR.CINC.Entities.EntityFramework;
using NQR.CINC.Entities.Models;
using NQR.CINC.Services.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace NQR.CINC.Services.UserService
{   
    public class UserRepository : BaseRepository, IUserRepository
    {
        public List<User> GetAllUsers()
        {
            List<User> allUsers = new List<User>();
            allUsers = UnitOfWorkInstance.UserEntity.Get().ToList();
            return allUsers;
        }
        public User RegisterUser(User user)
        {
            UnitOfWorkInstance.UserEntity.Insert(user);                        
            return user;
        }
        public User GetUserByNameandPassword(string userName, string password)
        {
            User user = new User();
            user = UnitOfWorkInstance.UserEntity.Get(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            return user;
        }

        public User GetUserByName(string userName)
        {
            User user = new User();
            user = UnitOfWorkInstance.UserEntity.Get(x => x.UserName == userName).FirstOrDefault();
            return user;
        }
        public User GetUserByEmail(string email)
        {
            User user = new User();
            user = UnitOfWorkInstance.UserEntity.Get(x => x.Email == email).FirstOrDefault();
            return user;
        }
        /// <summary>
        /// These are hardcoded roles you can change below logic once role table is ready
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<string> GetUserRoles(string email)
        {
            List<string> roles = new List<string>();
            roles.Add("Manager");
            roles.Add("Supervisor");
            return roles;
        }
    }
}
