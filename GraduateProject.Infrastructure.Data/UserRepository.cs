using GraduateProject.Domain.Core;
using GraduateProject.Domain.Interfaces;

namespace GraduateProject.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private ApplicationContext _db;
        public UserRepository(ApplicationContext db) {
            _db = db;
        }
        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public User? GetUserByLoginAndPassword(string login, string password)
        {
            return _db.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
